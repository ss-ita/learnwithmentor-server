using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Net;
using System.Net.Http;
using LearnWithMentorDTO;
using LearnWithMentorBLL.Interfaces;
using LearnWithMentorBLL.Infrastructure;
using LearnWithMentorBLL.Services;
using System.Text.RegularExpressions;

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
        public HttpResponseMessage GetAllTasks()
        {
            try
            {
                var allTasks = taskService.GetAllTasks();
                if (allTasks != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, allTasks);
                }
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No tasks in database.");
        }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message+"Internal server error");
            }
}

        /// <summary>
        /// Returns task by ID.
        /// </summary>
        // GET api/task/5
        [HttpGet]
        [Route("api/task/{taskId}")]
        public HttpResponseMessage GetTaskById(int taskId)
        {
            try
            {
                TaskDTO task = taskService.GetTaskById(taskId);
                if (task == null)
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Task with this ID does not exist in database.");
                return Request.CreateResponse(HttpStatusCode.OK, task);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Internal server error");
            }
        }

        /// <summary>
        /// Returns task with priority and section for defined by ID plan.
        /// </summary>
        /// <param name="taskId">ID of the tast.</param>
        /// <param name="planId">ID of the plan.</param>
        // GET api/task?id={id}&planid={planid}
        [HttpGet]
        [Route("api/task")]
        public HttpResponseMessage GetTaskForPlan(int taskId,int planId ) 
        {
            try
            {
                var task = taskService.GetTaskForPlan(taskId, planId);
                return Request.CreateResponse(HttpStatusCode.OK, task);
            }
            catch (ValidationException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest,ex.Message);
            }
        }

        /// <summary>
        /// Returns tasks with priority and section for defined by PlanTask ID.
        /// </summary>
        /// <param name="planTaskId">ID of the tast.</param>
        // GET api/task?id={id}&planid={planid}
        [HttpGet]
        [Route("api/task")]
        public HttpResponseMessage GetTaskForPlan(int planTaskId)
        {
            try
            {
                var task = taskService.GetTaskForPlan(planTaskId);
                return Request.CreateResponse(HttpStatusCode.OK, task);
            }
            catch (ValidationException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
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
        public HttpResponseMessage GetUserTask(int taskId, int planId, int userId)
        {
            try
            {
                var userTask = taskService.GetUserTaskByUserTaskPlanIds(userId, taskId, planId);
                return Request.CreateResponse(HttpStatusCode.OK, userTask);
            }
            catch (ValidationException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        /// <summary>
        /// Returns UserTasksDTO for task in plan for user.
        /// </summary>
        /// <param name="planTaskId">ID of the planTask.</param>
        /// <param name="userId">ID of the user.</param>
        [HttpGet]
        [Route("api/task/usertask")]
        public HttpResponseMessage GetUserTask(int planTaskId, int userId)
        {
            try
            {
                var userTask = taskService.GetUserTaskByUserTaskPlanId(userId, planTaskId);
                return Request.CreateResponse(HttpStatusCode.OK, userTask);
            }
            catch (ValidationException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        /// <summary>Returns messages for UserTask by id./// </summary>
        /// <param name="userTaskId">ID of the usertask.</param>
        [HttpGet]
        [Route("api/task/userTask/{userTaskId}/messages")]
        public HttpResponseMessage GetUserTaskMessages(int userTaskId)
        {
            try
            {
                var messaList= messageService.GetMessages(userTaskId);
                return Request.CreateResponse(HttpStatusCode.OK, messaList);
            }
            catch (ValidationException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        /// <summary> Creates message for UserTask. </summary>
        /// <param name="userTaskId">ID of the usertask.</param>
        /// <param name="newMessage">New message to be created.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/task/userTask/{userTaskId}/messages")]
        public HttpResponseMessage PostUserTaskMessage(int userTaskId, [FromBody]MessageDTO newMessage)
        {
            try
            {
                if(!ModelState.IsValid)
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
                newMessage.UserTaskId = userTaskId;
                //logic for sender id if needed
                bool success = taskService.CreateMessage(newMessage);
                if (success)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, $"Succesfully created message.");
                }
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Creation error.");
            }
            catch (Exception exception)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, exception);
            }
        }

        /// <summary>
        /// Creates new UserTask.
        /// </summary>
        /// <param name="newUserTask">New userTask object.</param>
        [HttpPost]
        [Route("api/task/usertask")]
        public HttpResponseMessage PostNewUserTask([FromBody]UserTaskDTO newUserTask)
        {
            try
            {
                if (!ModelState.IsValid)
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
                bool success = taskService.CreateUserTask(newUserTask);
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
        
        /// <summary>Changes UserTask status by usertask id./// </summary>
        /// <param name="userTaskId">ID of the userTask status to be changed.</param>
        /// /// <param name="newStatus">New userTask.</param>
        [HttpPut]
        [Route("api/task/usertask")]
        public HttpResponseMessage PutNewUserTaskStatus(int userTaskId, string newStatus)
        {
            try
            {
                if (!Regex.IsMatch(newStatus,ValidationRules.USERTASK_STATE))
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "New Status not valid");
                bool success = taskService.UpdateUserTaskStatus(userTaskId, newStatus);
                if (success)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, $"Succesfully updated user task status.");
                }
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Incorrect request syntax or usertask does not exist.");
            }
            catch (Exception exception)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, exception);
            }
        }

        /// <summary> Changes UserTask result by usertask id. </summary>
        /// <param name="userTaskId">ID of the userTask status to be changed</param>
        /// <param name="newResult">>New userTask result</param>
        /// <returns></returns>
        [HttpPut]
        [Route("api/task/usertask")]
        public HttpResponseMessage PutNewUserTaskResult(int userTaskId, string newResult)
        {
            try
            {
                if (newResult.Length >= ValidationRules.MAX_USERTASK_RESULT_LENGTH)
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "New Result is too long");
                bool success = taskService.UpdateUserTaskResult(userTaskId, newResult);
                if (success)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, $"Succesfully updated user task result.");
                }
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Incorrect request syntax or usertask does not exist.");
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
            List<UserTaskStateDTO> userTaskStateList = taskService.GetTaskStatesForUser(task_ids, user_id);
            if (userTaskStateList == null || userTaskStateList.Count == 0)
            {
                var message = "Not created any usertasks.";
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, message);
            }
            return Request.CreateResponse<IEnumerable<UserTaskStateDTO>>(HttpStatusCode.OK, userTaskStateList);
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
                return GetAllTasks();
            }
            string[] lines = key.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            var taskList = taskService.Search(lines);
            return Request.CreateResponse(HttpStatusCode.OK, taskList);
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
            var taskList = taskService.Search(lines, (int)planId);
            if (taskList == null)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, $"Incorrect request syntax, plan with ID:{planId} does not exist.");
            return Request.CreateResponse(HttpStatusCode.OK, taskList);
        }

        /// <summary>
        /// Creates new task
        /// </summary>
        /// <param name="newTask">Task object for creation.</param>
        // POST api/task
        [HttpPost]
        [Route("api/task")]
        public HttpResponseMessage Post([FromBody]TaskDTO newTask)
        {
            try
            {
                if (!ModelState.IsValid)
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
                bool success = taskService.CreateTask(newTask);
                if (success)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, $"Succesfully created task: {newTask.Name}.");
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
        /// <param name="taskId">Task ID for update.</param>
        /// <param name="task">Modified task object for update.</param>
        // PUT api/task/5
        [HttpPut]
        [Route("api/task/{id}")]
        public HttpResponseMessage Put(int taskId, [FromBody]TaskDTO task)
        {
            try
            {
                if (!ModelState.IsValid)
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
                bool success = taskService.UpdateTaskById(taskId, task);
                if (success)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, $"Succesfully updated task id: {taskId}.");
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
        /// <param name="taskId">Task ID for delete.</param>
        // DELETE api/task/5
        [HttpDelete]
        [Route("api/task/{id}")]
        public HttpResponseMessage Delete(int taskId)
        {
            try
            {
                bool success = taskService.RemoveTaskById(taskId);
                if (success)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, $"Succesfully deleted task id: {taskId}.");
                }
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, $"No task with id: {taskId} or cannot be deleted because of dependency conflict.");
            }
            catch (Exception exception)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, exception);
            }
        }
    }
}
