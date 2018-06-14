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
    /// <summary>
    /// Controller for working with tasks
    /// </summary>
    public class TaskController : ApiController
    {
        private IUnitOfWork UoW;
        public TaskController()
        {
            UoW = new UnitOfWork(new LearnWithMentor_DBEntities());
        }
        /// <summary>
        /// Returns a list of all tasks.
        /// </summary>
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
        /// <summary>
        /// Returns task by ID.
        /// </summary>
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

        /// <summary>
        /// Returns tasks with priority and section for defined by ID plan.
        /// </summary>
        /// <param name="id">ID of the tast.</param>
        /// <param name="planId">ID of the plan.</param>
        // GET api/task?id={id}&planid={planid}
        [HttpGet]
        [Route("api/task")]
        public HttpResponseMessage Get(int? id,int? planId )
        {
            if(id==null || planId == null)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest,
                                    "Incorrect request syntax: Id and planId must be defined.");
            Task t = UoW.Tasks.Get((int)id);
            if (t == null || !UoW.PlanTasks.ContainsTaskInPlan((int)id, (int)planId))
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
                                                                    t.PlanTasks.Where(pt => pt.Task_Id == t.Id && pt.Plan_Id == planId).FirstOrDefault()?.Priority,
                                                                    t.PlanTasks.Where(pt => pt.Task_Id == t.Id && pt.Plan_Id == planId).FirstOrDefault()?.Section_Id));
        }
        /// <summary>
        /// Returns tasks which name contains special string key.
        /// </summary>
        /// <param name="key">Key for search.</param>
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
        /// <summary>
        /// Returns tasks in plan which names contain special string key.
        /// </summary>
        /// <param name="key">Key for search.</param>
        /// <param name="planId">ID of the plan.</param>
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
        /// <summary>
        /// Creates new task
        /// </summary>
        /// <param name="t">Task object for creation.</param>
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
        /// <summary>
        /// Updates task by ID
        /// </summary>
        /// <param name="id">Task ID for update.</param>
        /// <param name="t">Modified task object for update.</param>
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
        /// <summary>
        /// Deletes task by ID
        /// </summary>
        /// <param name="id">Task ID for delete.</param>
        // DELETE api/task/5
        [HttpDelete]
        [Route("api/task/{id}")]
        public HttpResponseMessage Delete(int id)
        {
            try
            {
                if (!UoW.Tasks.IsRemovable(id))
                    return Request.CreateErrorResponse(HttpStatusCode.Conflict, 
                                    $"Task with id: {id} cannot be deleted because of dependency conflict.");
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
        /// <summary>
        /// Returns a list of comments for defined by ID task.
        /// </summary>
        /// <param name="taskId">Task ID.</param>
        [Route("api/task/{taskId}/comment")]
        public IEnumerable<CommentDTO> GetComments(int taskId)
        {
            var comments = UoW.Comments.GetAll().Where(c => c.PlanTask_Id == taskId);
            if (comments == null) return null;
            List<CommentDTO> dto = new List<CommentDTO>();
            foreach (var a in comments)
            {
                dto.Add(new CommentDTO(a.Id, a.Text, a.Create_Id, a.Creator.FirstName, a.Creator.LastName, a.Create_Date, a.Mod_Date));
            }
            return dto;
        }
        /// <summary>
        /// Creates comment for defined by ID task.
        /// </summary>
        /// <param name="value">Comment object for creation.</param>
        /// <param name="taskId">Task ID.</param>
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
