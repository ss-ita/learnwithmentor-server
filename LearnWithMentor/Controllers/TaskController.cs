using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
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
                                    UoW.Users.ExtractFullName(t.Create_Id),
                                    t.Mod_Id,
                                    UoW.Users.ExtractFullName(t.Mod_Id),
                                    t.Create_Date,
                                    t.Mod_Date,
                                    null,
                                    null));
            }
            if (dto == null) return null;
            return dto;
        }

        // GET api/task/5
        [HttpGet]
        [Route("api/task")]
        public TaskDTO Get(int id)
        {
            Task t = UoW.Tasks.Get(id);
            if (t == null) return null;
            return new TaskDTO(t.Id,
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
                               null);
        }

        // GET api/task/{id}/plan/{plan_id}
        [HttpGet]
        [Route("api/task")]
        public TaskDTO Get(int id,int planid )
        {
            Task t = UoW.Tasks.Get(id);
            if (t == null || !UoW.PlanTasks.ContainsTaskInPlan(id,planid)) return null;
            return new TaskDTO(t.Id,
                               t.Name,
                               t.Description,
                               t.Private,
                               t.Create_Id,
                               UoW.Users.ExtractFullName(t.Create_Id),
                               t.Mod_Id,
                               UoW.Users.ExtractFullName(t.Mod_Id),
                               t.Create_Date,
                               t.Mod_Date,
                               t.PlanTasks.Where(pt => pt.Task_Id == t.Id && pt.Plan_Id == planid).FirstOrDefault().Priority,
                               t.PlanTasks.Where(pt => pt.Task_Id == t.Id && pt.Plan_Id == planid).FirstOrDefault().Section_Id);
        }
        [HttpGet]
        [Route("api/task/search")]
        public IEnumerable<TaskDTO> Search(string key, int? planId)
        {
            if (key == null)
            {
                return Get();
            }
            else
            {
                string[] lines = key.Split(' ');
                List<TaskDTO> dto = new List<TaskDTO>();
                foreach (var t in UoW.Tasks.Search(lines, planId))
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
                                       t.PlanTasks.Where(pt => pt.Task_Id == t.Id && pt.Plan_Id == planId).FirstOrDefault().Priority,
                                       t.PlanTasks.Where(pt => pt.Task_Id == t.Id && pt.Plan_Id == planId).FirstOrDefault().Section_Id));
                }
                return dto;
            }
        }
        // POST api/task
        [HttpPost]
        public IHttpActionResult Post([FromBody]TaskDTO t)
        {
            UoW.Tasks.Add(t);
            UoW.Save();
            return Ok();
        }

        // PUT api/task/5
        [HttpPut]
        public IHttpActionResult Put(int id, [FromBody]TaskDTO t)
        {
            UoW.Tasks.UpdateById(id,t);
            UoW.Save();
            return Ok();
        }

        // DELETE api/task/5
        [HttpDelete]
        public IHttpActionResult Delete(int id)
        {
            UoW.Tasks.RemoveById(id);
            UoW.Save();
            return Ok();
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
    }
}
