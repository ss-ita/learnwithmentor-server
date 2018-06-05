using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using LearnWithMentorDAL;
using LearnWithMentorDAL.Entities;
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

        [HttpGet]
        // GET api/Task
        public IEnumerable<TaskDTO> Get()
        {
            List<TaskDTO> dto = new List<TaskDTO>();
            foreach (var t in UoW.Tasks.GetAll())
            {
                dto.Add(new TaskDTO(t.Id,
                                    t.Name,
                                    t.Description,
                                    t.Private,
                                    t.Create_Id,
                                    t.Mod_Id,
                                    t.Create_Date,
                                    t.Mod_Date,
                                    null,
                                    null));
            }
            if (dto == null) return null;
            return dto;
        }

        [HttpGet]
        // GET api/Task/5
        public TaskDTO Get(int id)
        {
            Task t = UoW.Tasks.Get(id);
            if (t == null) return null;
            return new TaskDTO(t.Id,
                               t.Name,
                               t.Description,
                               t.Private,
                               t.Create_Id,
                               t.Mod_Id,
                               t.Create_Date,
                               t.Mod_Date,
                               null,
                               null);
        }

        [HttpGet]
        // GET api/Task/{plan_id}/5
        public TaskDTO Get(int _plan_id, int id)
        {
            Task t = UoW.Tasks.Get(id);
            if (t == null || _plan_id < 1) return null;
            return new TaskDTO(t.Id,
                               t.Name,
                               t.Description,
                               t.Private,
                               t.Create_Id,
                               t.Mod_Id,
                               t.Create_Date,
                               t.Mod_Date,
                               t.PlanTasks.Where(pt => pt.Task_Id == t.Id && pt.Plan_Id == _plan_id).First().Priority,
                               t.PlanTasks.Where(pt => pt.Task_Id == t.Id && pt.Plan_Id == _plan_id).First().Section_Id);
        }
        // POST api/Task
        [HttpPost]
        public void Post([FromBody]TaskDTO t)
        {
            Task new_task = new Task()
            {
                Id = t.Id,
                Name = t.Name,
                Description = t.Description,
                Private = t.Private,
                Create_Id = t.Creator_Id,
                Mod_Id = t.Modifier_Id,
                Create_Date = t.Create_Date,
                Mod_Date = t.Mod_Date
            };
            UoW.Tasks.Add(new_task);
            UoW.Save();
        }

        // PUT api/Task/5
        [HttpPut]
        public void Put(int id, [FromBody]TaskDTO t)
        {
            Task new_task = new Task()
            {
                Id = t.Id,
                Name = t.Name,
                Description = t.Description,
                Private = t.Private,
                Create_Id = t.Creator_Id,
                Mod_Id = t.Modifier_Id,
                Create_Date = t.Create_Date,
                Mod_Date = t.Mod_Date
            };
            UoW.Tasks.Update(new_task);
            UoW.Save();
        }

        // DELETE api/Task/5
        [HttpDelete]
        public void Delete(int id)
        {
            Task t = UoW.Tasks.Get(id);
            UoW.Tasks.Remove(t);
            UoW.Save();
        }

        [Route("api/task/{taskId}/comment")]
        public IEnumerable<CommentDTO> GetComments(int taskId)
        {
            var comments = UoW.Comments.GetAll().Where(c => c.Task_Id == taskId);
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

        [HttpPut]
        [Route("api/task/{taskId}/comment")]
        public IHttpActionResult PutComment([FromBody]CommentDTO value, int taskId)
        {
            UoW.Comments.UpdateById(value, taskId);
            UoW.Save();
            return Ok();
        }
    }
}
