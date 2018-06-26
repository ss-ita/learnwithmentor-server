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
using LearnWithMentor.Filters;

namespace LearnWithMentor.Controllers
{
    /// <summary> Controller for working with tasks </summary>
    [Authorize]
    [JwtAuthentication]
    public class TaskController : ApiController
    {
        /// <summary> Services for work with different DB parts </summary>
        private readonly ITaskService taskService;
        private readonly IMessageService messageService;
        /// <summary> Services initiation </summary>
        public TaskController()
        {
            taskService = new TaskService();
            messageService = new MessageService();
        }

        /// <summary>
        /// Returns a list of all tasks in database.
        /// </summary>
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
                return Request.CreateErrorResponse(HttpStatusCode.NoContent, "There are no tasks in database.");
        }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
}

        /// <summary>
        /// Returns task by Id.
        /// </summary>
        [HttpGet]
        [Route("api/task/{taskId}")]
        public HttpResponseMessage GetTaskById(int taskId)
        {
            try
            {
                TaskDTO task = taskService.GetTaskById(taskId);
                if (task == null)
                    return Request.CreateErrorResponse(HttpStatusCode.NoContent, "This task does not exist in database.");
                return Request.CreateResponse(HttpStatusCode.OK, task);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        /// <summary>
        /// Returns tasks with priority and section for by PlanTask Id.
        /// </summary>
        /// <param name="planTaskId">Id of the planTask.</param>
        [HttpGet]
        [Route("api/task/plantask/{planTaskId}")] 
        public HttpResponseMessage GetTaskForPlan(int planTaskId)
        {
            try
            {
                var task = taskService.GetTaskForPlan(planTaskId);
                if(task!=null)
                    return Request.CreateResponse(HttpStatusCode.OK, task);
                return Request.CreateErrorResponse(HttpStatusCode.NoContent, "This task does not exist in database.");
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        /// <summary>
        /// Returns UserTask for task in plan for user.
        /// </summary>
        /// <param name="planTaskId">Id of the planTask.</param>
        /// <param name="userId">Id of the user.</param>
        [HttpGet]
        [Route("api/task/usertask")] 
        public HttpResponseMessage GetUserTask(int planTaskId, int userId)
        {
            try
            {
                var userTask = taskService.GetUserTaskByUserPlanTaskId(userId, planTaskId);
                if(userTask!=null)
                    return Request.CreateResponse(HttpStatusCode.OK, userTask);
                return Request.CreateErrorResponse(HttpStatusCode.NoContent, "Task for this user does not exist in database.");
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        /// <summary>Returns messages for UserTask by its id.</summary>
        /// <param name="userTaskId">Id of the usertask.</param>
        [HttpGet]
        [Route("api/task/userTask/{userTaskId}/messages")] 
        public HttpResponseMessage GetUserTaskMessages(int userTaskId)
        {
            try
            {
                var messageList= messageService.GetMessages(userTaskId);
                if(messageList!=null)
                    return Request.CreateResponse(HttpStatusCode.OK, messageList);
                return Request.CreateErrorResponse(HttpStatusCode.NoContent, "Messages for this user does not exist in database.");
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        /// <summary> Creates message for UserTask. </summary>
        /// <param name="userTaskId">Id of the usertask.</param>
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
                bool success = messageService.SendMessage(newMessage);
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
                    return Request.CreateResponse(HttpStatusCode.OK, "Succesfully created task for user.");
                }
                return Request.CreateErrorResponse(HttpStatusCode.NoContent, "There is no user or task in database.");
            }
            catch (Exception exception)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, exception);
            }
        }
        
        /// <summary>Changes UserTask status by usertask id.</summary>
        /// <param name="userTaskId">Id of the userTask status to be changed.</param>
        /// /// <param name="newStatus">New userTask.</param>
        [HttpPut]
        [Route("api/task/usertask/status")] 
        public HttpResponseMessage PutNewUserTaskStatus(int userTaskId, string newStatus)
        {
            try
            {
                if (!Regex.IsMatch(newStatus,ValidationRules.USERTASK_STATE))
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "New Status not valid");
                bool success = taskService.UpdateUserTaskStatus(userTaskId, newStatus);
                if (success)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, "User task status succesfully updated.");
                }
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Incorrect new starus or usertask does not exist.");
            }
            catch (Exception exception)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, exception);
            }
        }

        /// <summary> Changes UserTask result by usertask id. </summary>
        /// <param name="userTaskId">Id of the userTask status to be changed</param>
        /// <param name="newResult">>New userTask result</param>
        /// <returns></returns>
        [HttpPut]
        [Route("api/task/usertask/result")] 
        public HttpResponseMessage PutNewUserTaskResult(int userTaskId, string newResult)
        {
            try
            {
                if (newResult.Length >= ValidationRules.MAX_USERTASK_RESULT_LENGTH)
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "New Result is too long");
                bool success = taskService.UpdateUserTaskResult(userTaskId, newResult);
                if (success)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, "User task result succesfully updated.");
                }
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Incorrect request syntax or usertask does not exist.");
            }
            catch (Exception exception)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, exception);
            }
        }

        /// <summary>Returns tasks states for array of id.</summary>
        /// <param name="user_id">Id of the user.</param>
        /// <param name="task_ids">Array of tasks id.</param>
        [HttpGet]
        [Route("api/task/state")] 
        public HttpResponseMessage GetAllTasksState(int user_id, int[] task_ids)
        {
            try
            {
                List<UserTaskStateDTO> userTaskStateList = taskService.GetTaskStatesForUser(task_ids, user_id);
                if (userTaskStateList == null || userTaskStateList.Count == 0)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NoContent, "There are no created usertasks.");
                }
                return Request.CreateResponse(HttpStatusCode.OK, userTaskStateList);
            }
            catch (Exception exception)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, exception);
            }
        }

        /// <summary>Returns tasks which name contains string key.</summary>
        /// <param name="key">Key for search.</param>
        [HttpGet]
        [Route("api/task/search")]
        public HttpResponseMessage Search(string key)
        {
            try
            {
                if (key == null)
                {
                    return GetAllTasks();
                }
                string[] lines = key.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                var taskList = taskService.Search(lines);
                return Request.CreateResponse(HttpStatusCode.OK, taskList);
            }
            catch (Exception exception)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, exception);
            }
        }

        /// <summary>
        /// Returns tasks in plan which names contain string key.
        /// </summary>
        /// <param name="key">Key for search.</param>
        /// <param name="planId">Id of the plan.</param>
        [HttpGet]
        [Route("api/task/searchinplan")]
        public HttpResponseMessage SearchInPlan(string key, int planId)
        {
            try
            {
                if (key == null)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Incorrect request syntax.");
                }
                string[] lines = key.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                var taskList = taskService.Search(lines, planId);
                if (taskList == null)
                    return Request.CreateErrorResponse(HttpStatusCode.NoContent, "This plan does not exist.");
                return Request.CreateResponse(HttpStatusCode.OK, taskList);
            }
            catch (Exception exception)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, exception);
            }
        }

        /// <summary>
        /// Creates new task
        /// </summary>
        /// <param name="newTask">Task object for creation.</param>
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
                    return Request.CreateResponse(HttpStatusCode.OK, "Task succesfully created");
                }
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Creation error.");
            }
            catch (Exception exception)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, exception);
            }

        }

        /// <summary>
        /// Updates task by Id
        /// </summary>
        /// <param name="taskId">Task Id for update.</param>
        /// <param name="task">Modified task object for update.</param>
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
                    return Request.CreateResponse(HttpStatusCode.OK, "Task succesfully updated.");
                }
                return Request.CreateErrorResponse(HttpStatusCode.NoContent, "Task does not exist.");
            }
            catch (Exception exception)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, exception);
            }
        }

        /// <summary>
        /// Deletes task by Id
        /// </summary>
        /// <param name="taskId">Task Id for delete.</param>
        [HttpDelete]
        [Route("api/task/{id}")]
        public HttpResponseMessage Delete(int taskId)
        {
            try
            {
                bool success = taskService.RemoveTaskById(taskId);
                if (success)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, "Task succesfully deleted.");
                }
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Task not exist or cannot be deleted because of dependency conflict.");
            }
            catch (Exception exception)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, exception);
            }
        }

        /// <summary>
        /// Releases memory
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            taskService.Dispose();
            messageService.Dispose();
            base.Dispose(disposing);
        }
    }
}
