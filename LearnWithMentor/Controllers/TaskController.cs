using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Net;
using System.Net.Http;
using LearnWithMentorDAL.Entities;
using LearnWithMentorDAL.UnitOfWork;
using LearnWithMentorDTO;

namespace LearnWithMentor.Controllers
{
    public class TaskController : ApiController
    {
        private IUnitOfWork UoW;
        public TaskController()
        {
            UoW = new UnitOfWork(new LearnWithMentor_DBEntities());
        }

        // GET api/task      
        [HttpGet]
        [Route("api/task")]
        public HttpResponseMessage Get()
        {
            List<TaskDTO> dto = new List<TaskDTO>();
            bool exists = false;
            var tasks = UoW.Tasks.GetAll();
            if(tasks!=null) exists = true;
            foreach (var t in tasks)
            {
                dto.Add(new TaskDTO(t.Id,
                                    t.Name,
                                    t.Description,
                                    t.Private,
                                    t.Create_Id,
                                    UoW.Users.ExtractFullName(t.Create_Id),
                                    t.Mod_Id,
                                    UoW.Users.ExtractFullName(t.Mod_Id),
                                    t.Create_Date,
                                    t.Mod_Date,
                                    null,
                                    null));
            }
            if (exists)
            {
                return Request.CreateResponse(HttpStatusCode.OK, dto);
            }
            return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No tasks in database.");
        }

        // GET api/task/5
        [HttpGet]
        [Route("api/task/{id}")]
        public HttpResponseMessage Get(int id)
        {
            Task t = UoW.Tasks.Get(id);
            if (t != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new TaskDTO(t.Id,
                                                                            t.Name,
                                                                            t.Description,
                                                                            t.Private,
                                                                            t.Create_Id,
                                                                            UoW.Users.ExtractFullName(t.Create_Id),
                                                                            t.Mod_Id,
                                                                            UoW.Users.ExtractFullName(t.Mod_Id),
                                                                            t.Create_Date,
                                                                            t.Mod_Date,
                                                                            null,
                                                                            null));
            }
            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Task with this ID does not exist in database.");
        }

        // GET api/task?id={id}&planid={planid}
        [HttpGet]
        [Route("api/task")]
        public HttpResponseMessage Get(int? id,int? planid )
        {
            if(id==null || planid== null)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest,
                                    "Incorrect request syntax: Id and planId must be defined.");
            Task t = UoW.Tasks.Get((int)id);
            if (t == null || !UoW.PlanTasks.ContainsTaskInPlan((int)id, (int)planid))
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, 
                                    "Incorrect request syntax or task in this plan does not exist.");
            return Request.CreateResponse(HttpStatusCode.OK,new TaskDTO(t.Id,
                                                                    t.Name,
                                                                    t.Description,
                                                                    t.Private,
                                                                    t.Create_Id,
                                                                    UoW.Users.ExtractFullName(t.Create_Id),
                                                                    t.Mod_Id,
                                                                    UoW.Users.ExtractFullName(t.Mod_Id),
                                                                    t.Create_Date,
                                                                    t.Mod_Date,
                                                                    t.PlanTasks.Where(pt => pt.Task_Id == t.Id && pt.Plan_Id == planid).FirstOrDefault()?.Priority,
                                                                    t.PlanTasks.Where(pt => pt.Task_Id == t.Id && pt.Plan_Id == planid).FirstOrDefault()?.Section_Id));
        }
        // GET api/task/search?key={key}
        [HttpGet]
        [Route("api/task/search")]
        public HttpResponseMessage Search(string key)
        {
            
            if (key == null)
            {
                return Get();
            }
            string[] lines = key.Split(' ');
            List<TaskDTO> dto = new List<TaskDTO>();
            foreach (var t in UoW.Tasks.Search(lines))
            {
                dto.Add(new TaskDTO(t.Id,
                                    t.Name,
                                    t.Description,
                                    t.Private,
                                    t.Create_Id,
                                    UoW.Users.ExtractFullName(t.Create_Id),
                                    t.Mod_Id,
                                    UoW.Users.ExtractFullName(t.Mod_Id),
                                    t.Create_Date,
                                    t.Mod_Date,
                                    null,
                                    null));
            }
            return Request.CreateResponse(HttpStatusCode.OK, dto);
        }
    
        // GET api/task/search?key={key}&planid={planid}
        [HttpGet]
        [Route("api/task/SearchInPlan")]
        public HttpResponseMessage SearchInPlan(string key, int? planId)
        {
            if (key == null || planId == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Incorrect request syntax.");
            }
            string[] lines = key.Split(' ');
            List<TaskDTO> dto = new List<TaskDTO>();
            var loadedPlans = UoW.Tasks.Search(lines, (int)planId);
            if (loadedPlans == null)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, $"Incorrect request syntax, plan with ID:{planId} does not exist.");
            foreach (var t in loadedPlans)
            {
                dto.Add(new TaskDTO(t.Id,
                                    t.Name,
                                    t.Description,
                                    t.Private,
                                    t.Create_Id,
                                    UoW.Users.ExtractFullName(t.Create_Id),
                                    t.Mod_Id,
                                    UoW.Users.ExtractFullName(t.Mod_Id),
                                    t.Create_Date,
                                    t.Mod_Date,
                                    t.PlanTasks.Where(pt => pt.Task_Id == t.Id && pt.Plan_Id == planId).FirstOrDefault()?.Priority,
                                    t.PlanTasks.Where(pt => pt.Task_Id == t.Id && pt.Plan_Id == planId).FirstOrDefault()?.Section_Id));
            }
            return Request.CreateResponse(HttpStatusCode.OK, dto);
        }
        // POST api/task
        [HttpPost]
        [Route("api/task")]
        public HttpResponseMessage Post([FromBody]TaskDTO t)
        {
            try
            {
                if (!ModelState.IsValid)
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
                bool success = UoW.Tasks.Add(t);
                if (success)
                {
                    UoW.Save();
                    return Request.CreateResponse(HttpStatusCode.OK, $"Succesfully created task: {t.Name}.");
                }
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Incorrect request syntax.");
            }
            catch (Exception exception)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, exception);
            }

        }

        // PUT api/task/5
        [HttpPut]
        [Route("api/task/{id}")]
        public HttpResponseMessage Put(int id, [FromBody]TaskDTO t)
        {
            try
            {
                if (!ModelState.IsValid)
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
                bool success = UoW.Tasks.UpdateById(id, t);
                if (success)
                {
                    UoW.Save();
                    return Request.CreateResponse(HttpStatusCode.OK, $"Succesfully updated task id: {id}.");
                }
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Incorrect request syntax or task does not exist.");
            }
            catch (Exception exception)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, exception);
            }
        }

        // DELETE api/task/5
        [HttpDelete]
        [Route("api/task/{id}")]
        public HttpResponseMessage Delete(int id)
        {
            try
            {
                bool success = UoW.Tasks.RemoveById(id);
                if (success)
                {
                    UoW.Save();
                    return Request.CreateResponse(HttpStatusCode.OK, $"Succesfully deleted task id: {id}.");
                }
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, $"No task with id: {id}.");
            }
            catch (Exception exception)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, exception);
            }
        }

        [Route("api/task/{taskId}/comment")]
        public IEnumerable<CommentDTO> GetComments(int taskId)
        {
            var comments = UoW.Comments.GetAll().Where(c => c.PlanTask_Id == taskId);
            if (comments == null) return null;
            List<CommentDTO> dto = new List<CommentDTO>();
            foreach (var a in comments)
            {
                dto.Add(new CommentDTO(a.Id, a.Text, a.Create_Id, a.Users.FirstName, a.Users.LastName, a.Create_Date, a.Mod_Date));
            }
            return dto;
        }

        [HttpPost]
        [Route("api/task/{taskId}/comment")]
        public IHttpActionResult AddComment([FromBody]CommentDTO value, int taskId)
        {
            UoW.Comments.Add(value, taskId);
            UoW.Save();
            return Ok();
        }
    }
}
