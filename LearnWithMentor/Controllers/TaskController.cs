using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Net;
using System.Net.Http;
using LearnWithMentorDTO;
using LearnWithMentorBLL.Interfaces;
using LearnWithMentorBLL.Infrastructure;
using LearnWithMentorBLL.Services;

namespace LearnWithMentor.Controllers
{
    /// <summary>
    /// Controller for working with tasks
    /// </summary>
    public class TaskController : ApiController
    {
        private readonly ITaskService taskService;
        private readonly IMessageService messageService;
        public TaskController()
        {
            taskService = new TaskService();
            messageService = new MessageService();
        }

        /// <summary>
        /// Returns a list of all tasks.
        /// </summary>
        // GET api/task      
        [HttpGet]
        [Route("api/task")]
        public HttpResponseMessage Get()
        {
            var allTasks = taskService.GetAllTasks();
            if (allTasks!=null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, allTasks);
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
            TaskDTO t = taskService.GetTaskById(id);
            if (t == null)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Task with this ID does not exist in database.");
            return Request.CreateResponse(HttpStatusCode.OK, t);
        }

        /// <summary>
        /// Returns tasks with priority and section for defined by ID plan.
        /// </summary>
        /// <param name="taskId">ID of the tast.</param>
        /// <param name="planId">ID of the plan.</param>
        // GET api/task?id={id}&planid={planid}
        [HttpGet]
        [Route("api/task")]
        public HttpResponseMessage Get(int taskId,int planId )
        {
            try
            {
                var t = taskService.GetTaskForPlan(taskId, planId);
                return Request.CreateResponse(HttpStatusCode.OK, t);
            }
            catch (ValidationException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest,ex.Message);
            }
        }

        /// <summary>
        /// Returns UserTasksDTO for task in plan for user.
        /// </summary>
        /// <param name="taskId">ID of the task.</param>
        /// <param name="planId">ID of the plan.</param>
        /// <param name="userId">ID of the user.</param>
        [HttpGet]
        [Route("api/task/usertask")]
        public HttpResponseMessage Get(int taskId, int planId, int userId)
        {
            try
            {
                var ut = taskService.GetUserTaskByUserTaskPlanIds(userId, taskId, planId);
                return Request.CreateResponse(HttpStatusCode.OK, ut);
            }
            catch (ValidationException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        /// <summary>Returns messages for UserTask for task in plan for user./// </summary>
        /// <param name="userId">ID of the user.</param>
        /// /// <param name="taskId">ID of the task.</param>
        /// /// <param name="planId">ID of the plan.</param>
        [HttpGet]
        [Route("api/task/userTask/{userTaskId}/messages")]
        public HttpResponseMessage GetMessages(int userId, int taskId, int planId)//or(userId,taskId,planId)
        {
            try
            {
                var dto= messageService.GetMessages(userId, taskId, planId);
                return Request.CreateResponse(HttpStatusCode.OK, dto);
            }
            catch (ValidationException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        /// <summary>
        /// Creates new UserTask.
        /// </summary>
        /// <param name="newUT">New object userTask.</param>
        [HttpPost]
        [Route("api/task/usertask")]
        public HttpResponseMessage Post([FromBody]UserTaskDTO ut)
        {
            try
            {
                if (!ModelState.IsValid)
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
                bool success = taskService.CreateUserTask(ut);
                if (success)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, $"Succesfully created task for user.");
                }
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Creation error.");
            }
            catch (Exception exception)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, exception);
            }
        }

        /// <summary>
        /// Changes UserTask status.
        /// </summary>
        /// <param name="userTaskId">ID of the userTask to be changed.</param>
        /// /// <param name="newStatus">New userTask.</param>
        [HttpPut]
        [Route("api/task/usertask")]
        public HttpResponseMessage Put(int userId, int taskId, int planId, string newStatus)
        {
            try
            {
                if (!ModelState.IsValid)
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
                bool success = taskService.UpdateUserTaskStatus(userId, taskId, planId, newStatus);
                if (success)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, $"Succesfully updated user task status.");
                }
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Incorrect request syntax or task does not exist.");
            }
            catch (Exception exception)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, exception);
            }
        }

        /// <summary>Returns all tasks states for if array./// </summary>
        [HttpGet]
        [Route("api/tasks/state")] // or get user info from token only for authorized user
        public HttpResponseMessage GetAllTasksState(int user_id, int[] task_ids)
        {
            List<UserTaskStateDTO> dtosList = taskService.GetTaskStatesForUser(task_ids, user_id);
            if (dtosList == null || dtosList.Count == 0)
            {
                var message = "Not created any usertasks.";
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, message);
            }
            return Request.CreateResponse<IEnumerable<UserTaskStateDTO>>(HttpStatusCode.OK, dtosList);
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
            string[] lines = key.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            var dto = taskService.Search(lines);
            return Request.CreateResponse(HttpStatusCode.OK,dto);
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
            string[] lines = key.Split(new char[] { ' ' },StringSplitOptions.RemoveEmptyEntries);
            var dto = taskService.Search(lines, (int)planId);
            if (dto == null)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, $"Incorrect request syntax, plan with ID:{planId} does not exist.");
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
                bool success = taskService.CreateTask(t);
                if (success)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, $"Succesfully created task: {t.Name}.");
                }
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Creation error.");
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
                bool success = taskService.UpdateTaskById(id, t);
                if (success)
                {
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
                bool success = taskService.RemoveTaskById(id);
                if (success)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, $"Succesfully deleted task id: {id}.");
                }
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, $"No task with id: {id} or cannot be deleted because of dependency conflict.");
            }
            catch (Exception exception)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, exception);
            }
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
            //UoW.Comments.Add(value, taskId);
            //UoW.Save();
            return Ok();
        }
    }
}
