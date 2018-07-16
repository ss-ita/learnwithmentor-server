using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using LearnWithMentor.Filters;
using LearnWithMentorDTO;
using LearnWithMentorBLL.Interfaces;
using LearnWithMentorBLL.Services;
using System.Web.Http.Tracing;
using LearnWithMentor.Log;
using System.Data.Entity.Core;
using System.Web;
using System.IO;
using System.Drawing;

namespace LearnWithMentor.Controllers
{
    /// <summary>
    /// Controller for plans.
    /// </summary>
    [Authorize]
    [JwtAuthentication]
    public class PlanController : ApiController
    {
        private readonly IPlanService planService;
        private readonly ITaskService taskService;
        private readonly ITraceWriter tracer;

        /// <summary>
        /// Creates new instance of controller.
        /// </summary>
        public PlanController(IPlanService planService, ITaskService taskService, ITraceWriter tracer)
        {
            this.planService = planService;
            this.taskService = taskService;
            this.tracer = tracer;
        }

        /// <summary>
        /// Returns all plans in database.
        /// </summary>
        [HttpGet]
        [Route("api/plan")]
        public HttpResponseMessage Get()
        {
            List<PlanDTO> dtoList = planService.GetAll();
            if (dtoList == null || dtoList.Count == 0)
            {
                var errorMessage = "No plans in database.";
                return Request.CreateErrorResponse(HttpStatusCode.NoContent, errorMessage);
            }
            return Request.CreateResponse<IEnumerable<PlanDTO>>(HttpStatusCode.OK, dtoList);
        }

        /// <summary>
        /// Gets plan by id.
        /// </summary>
        /// <param name="id"> Id of the plan. </param>
        [HttpGet]
        [Route("api/plan/{id}")]
        public HttpResponseMessage Get(int id)
        {
            var plan = planService.Get(id);
            if (plan == null)
            {
                var message = "Plan does not exist in database.";
                return Request.CreateErrorResponse(HttpStatusCode.NoContent, message);
            }
            return Request.CreateResponse<PlanDTO>(HttpStatusCode.OK, plan);
        }

        /// <summary>
        /// Gets some number of plans on page. 
        /// </summary>
        /// <param name="prevAmount"> Previous amount to start with. </param>
        /// <param name="amount"> Amount of plans to be returned. </param>
        [AllowAnonymous]
        [HttpGet]
        [Route("api/plan/some")]
        public HttpResponseMessage GetSome(int prevAmount, int amount)
        {
            List<PlanDTO> dtoList = planService.GetSomeAmount(prevAmount, amount);
            if (dtoList == null || dtoList.Count == 0)
            {
                var errorMessage = "No plans in database.";
                return Request.CreateErrorResponse(HttpStatusCode.NoContent, errorMessage);
            }
            return Request.CreateResponse<IEnumerable<PlanDTO>>(HttpStatusCode.OK, dtoList);
        }

        /// <summary>
        /// Gets all tasks assigned to plan.
        /// </summary>
        /// <param name="plan_id"> Id of plan. </param>
        [HttpGet]
        [Route("api/plan/{plan_id}/tasks")]
        public HttpResponseMessage GetAllTasks(int plan_id)
        {
            List<TaskDTO> dtosList = planService.GetAllTasks(plan_id);
            if (dtosList == null || dtosList.Count == 0)
            {
                var message = "Plan does not contain any task.";
                return Request.CreateErrorResponse(HttpStatusCode.NoContent, message);
            }
            return Request.CreateResponse<IEnumerable<TaskDTO>>(HttpStatusCode.OK, dtosList);
        }

        /// <summary>
        /// Creates new plan.
        /// </summary>
        /// <param name="value"> New plan to be created. </param>
        [Authorize(Roles = "Mentor")]
        [HttpPost]
        [Route("api/plan")]
        public HttpResponseMessage Post([FromBody]PlanDTO value)
        {
            try
            {
                var success = planService.Add(value);
                if (success)
                {
                    var log = $"Succesfully created plan {value.Name} with id = {value.Id} by user with id = {value.CreatorId}";
                    tracer.Info(Request, ControllerContext.ControllerDescriptor.ControllerType.FullName, log);
                    var okMessage = $"Succesfully created plan: {value.Name}";
                    return Request.CreateResponse(HttpStatusCode.OK, okMessage);
                }
            }
            catch (EntityException e)
            {
                tracer.Error(Request, ControllerContext.ControllerDescriptor.ControllerType.FullName, e);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }
            tracer.Warn(Request, ControllerContext.ControllerDescriptor.ControllerType.FullName, "Error occured on creating plan");
            var message = "Incorrect request syntax.";
            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, message);
        }

        /// <summary>
        /// Creates new plan and returns id of the created plan.
        /// </summary>
        /// <param name="value"> New plan to be created. </param>
        [Authorize(Roles = "Mentor")]
        [HttpPost]
        [Route("api/plan/return")]
        public HttpResponseMessage PostAndReturnId([FromBody]PlanDTO value)
        {
            try
            {
                var result = planService.AddAndGetId(value);
                if (result != null)
                {
                    var log = $"Succesfully created plan {value.Name} with id = {result} by user with id = {value.CreatorId}";
                    tracer.Info(Request, ControllerContext.ControllerDescriptor.ControllerType.FullName, log);
                    var okMessage = $"Succesfully created plan: {value.Name}";
                    return Request.CreateResponse(HttpStatusCode.OK, result);
                }
            }
            catch (EntityException e)
            {
                tracer.Error(Request, ControllerContext.ControllerDescriptor.ControllerType.FullName, e);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }
            tracer.Warn(Request, ControllerContext.ControllerDescriptor.ControllerType.FullName, "Error occured on creating plan");
            var message = "Incorrect request syntax.";
            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, message);
        }

        /// <summary>
        /// Updates existing plan.
        /// </summary>
        /// <param name="id"> Id of plan to be updated. </param>
        /// <param name="value"> Plan info to be updated. </param>
        [Authorize(Roles = "Mentor")]
        [HttpPut]
        [Route("api/plan")]
        public HttpResponseMessage Put(int id, [FromBody]PlanDTO value)
        {
            try
            {
                var success = planService.UpdateById(value, id);
                if (success)
                {
                    var log = $"Succesfully updated plan {value.Name} with id = {value.Id} by user with id = {value.Modid}";
                    tracer.Info(Request, ControllerContext.ControllerDescriptor.ControllerType.FullName, log);
                    var okMessage = $"Succesfully updated plan.";
                    return Request.CreateResponse(HttpStatusCode.OK, okMessage);
                }
            }
            catch (EntityException e)
            {
                tracer.Error(Request, ControllerContext.ControllerDescriptor.ControllerType.FullName, e);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }
            tracer.Warn(Request, ControllerContext.ControllerDescriptor.ControllerType.FullName, "Error occured on updating plan");
            var message = "Incorrect request syntax or plan does not exist.";
            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, message);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="taskId"></param>
        /// <param name="sectionId"></param>
        /// <param name="priority"></param>
        /// <returns></returns>            
        [Authorize(Roles = "Mentor")]
        [HttpPut]
        [Route("api/plan/{id}/task/{taskId}")]
        public HttpResponseMessage PutTaskToPlan(int id,  int taskId,string sectionId, string priority)
        {
            try
            {
                int? section;
                int? priorityNew;
                if (string.IsNullOrEmpty(sectionId))
                {
                    section = null;
                }
                else
                {
                    section = int.Parse(sectionId);
                }
                if (string.IsNullOrEmpty(priority))
                {
                    priorityNew = null;
                }
                else
                {
                    priorityNew = int.Parse(priority);
                }
                bool success = planService.AddTaskToPlan(id, taskId, section, priorityNew);
                if (success)
                {
                    var log = $"Succesfully add task with id {taskId} to plan with id = {id}";
                    tracer.Info(Request, ControllerContext.ControllerDescriptor.ControllerType.FullName, log);
                    return Request.CreateResponse(HttpStatusCode.OK, $"Succesfully added task to plan ({id}).");
                }
                tracer.Warn(Request, ControllerContext.ControllerDescriptor.ControllerType.FullName, "Error occured on adding task to plan");
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Incorrect request syntax or task or plan does not exist.");
            }
            catch (EntityException e)
            {
                tracer.Error(Request, ControllerContext.ControllerDescriptor.ControllerType.FullName, e);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }
        }

        /// <summary>
        /// Sets image for plan by plan id.
        /// </summary>
        /// <param name="id"> Id of the plan. </param>
        [Authorize(Roles = "Mentor")]
        [HttpPost]
        [Route("api/plan/{id}/image")]
        public HttpResponseMessage PostImage(int id)
        {
            if (!planService.ContainsId(id))
            {
                var errorMessage = "No plan with this id in database.";
                return Request.CreateResponse(HttpStatusCode.NoContent, errorMessage);
            }

            if (HttpContext.Current.Request.Files.Count != 1)
            {
                var errorMessage = "Only one image can be sent.";
                return Request.CreateResponse(HttpStatusCode.BadRequest, errorMessage);
            }

            try
            {
                var postedFile = HttpContext.Current.Request.Files[0];
                if (postedFile != null && postedFile.ContentLength > 0)
                {
                    List<string> allowedFileExtensions = new List<string> { ".jpeg", ".jpg", ".png" };

                    var extension = postedFile.FileName.Substring(postedFile.FileName.LastIndexOf('.')).ToLower();
                    if (!allowedFileExtensions.Contains(extension))
                    {
                        string errorMessage = "Types allowed only .jpeg .jpg .png";
                        return Request.CreateResponse(HttpStatusCode.BadRequest, errorMessage);
                    }

                    int maxContentLength = 1024 * 1024 * 1; //Size = 1 MB  
                    if (postedFile.ContentLength > maxContentLength)
                    {
                        string errorMessage = "Please Upload a file upto 1 mb.";
                        return Request.CreateResponse(HttpStatusCode.BadRequest, errorMessage);
                    }

                    byte[] imageData = null;
                    using (var binaryReader = new BinaryReader(postedFile.InputStream))
                    {
                        imageData = binaryReader.ReadBytes(postedFile.ContentLength);
                    }

                    planService.SetImage(id, imageData, postedFile.FileName);
                    var okMessage = "Successfully created image.";
                    return Request.CreateResponse(HttpStatusCode.OK, okMessage);
                }
                string emptyImageMessage = "Empty image.";
                return Request.CreateErrorResponse(HttpStatusCode.NotModified, emptyImageMessage);
            }
            catch (EntityException e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, e);
            }
        }

        /// <summary>
        /// Returns image of concrete plan form database.
        /// </summary>
        /// <param name="id"> Id of the plan. </param>
        [AllowAnonymous]
        [HttpGet]
        [Route("api/plan/{id}/image")]
        public HttpResponseMessage GetImage(int id)
        {
            try
            {
                if (!planService.ContainsId(id))
                {
                    var errorMessage = "No plan with this id in database.";
                    return Request.CreateResponse(HttpStatusCode.NoContent, errorMessage);
                }
                ImageDTO dto = planService.GetImage(id);
                if (dto == null)
                {
                    var message = "No image for this plan in database.";
                    return Request.CreateResponse(HttpStatusCode.NoContent, message);
                }
                return Request.CreateResponse(HttpStatusCode.OK, dto);
            }
            catch (EntityException e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, e);
            }
        }

        /// <summary>
        /// Searches plans that match q string
        /// </summary>
        /// <param name="q">Match string</param>
        [HttpGet]
        [Route("api/plan/search")]
        public HttpResponseMessage Search(string q)
        {
            if (string.IsNullOrEmpty(q))
            {
                return Get();
            }
            string[] lines = q.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            var dto = planService.Search(lines);
            if (dto == null || dto.Count == 0)
            {
                var message = "No plans found.";
                return Request.CreateErrorResponse(HttpStatusCode.NoContent, message);
            }
            return Request.CreateResponse<IEnumerable<PlanDTO>>(HttpStatusCode.OK, dto);
        }

        /// <summary>
        /// Releases memory
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            planService.Dispose();
            taskService.Dispose();
            base.Dispose(disposing);
        }
    }
}
