﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using LearnWithMentor.Filters;
using LearnWithMentorDTO;
using LearnWithMentorBLL.Interfaces;
using System.Web.Http.Tracing;
using System.Data.Entity.Core;
using System.Web;
using System.IO;
using System.Threading.Tasks;

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
        [AllowAnonymous]
        [HttpGet]
        [Route("api/plan")]
        public HttpResponseMessage Get()
        {
            var dtoList = planService.GetAll();
            if (dtoList == null || dtoList.Count == 0)
            {
                const string errorMessage = "No plans in database.";
                return Request.CreateErrorResponse(HttpStatusCode.NoContent, errorMessage);
            }
            return Request.CreateResponse<IEnumerable<PlanDto>>(HttpStatusCode.OK, dtoList);
        }

        /// <summary>
        /// Gets plan by id.
        /// </summary>
        /// <param name="id"> Id of the plan. </param>
        [AllowAnonymous]
        [HttpGet]
        [Route("api/plan/{id}")]
        public async Task<HttpResponseMessage> GetAsync(int id)
        {
            var plan = await planService.GetAsync(id);
            if (plan == null)
            {
                const string message = "Plan does not exist in database.";
                return Request.CreateErrorResponse(HttpStatusCode.NoContent, message);
            }
            return Request.CreateResponse(HttpStatusCode.OK, plan);
        }

        /// <summary>
        /// Gets plan name and group name  by groupid and planid.
        /// </summary>
        /// <param name="groupid"> Id of the group. </param>
        ///  <param name="planid"> Id of the plan. </param>
        [HttpGet]
        [Route("api/plan/{planid}/group/{groupid}")]
        public async Task<HttpResponseMessage> GetInfoAsync(int groupid, int planid)
        {

            var info = await planService.GetInfoAsync(groupid, planid);
            if (info == null)
            {
                const string message = "Plan or Group does not exist in database.";
                return Request.CreateErrorResponse(HttpStatusCode.NoContent, message);
            }
            return Request.CreateResponse(HttpStatusCode.OK, info);
        }

        /// <summary>
        /// Returns tasks for concrete plan grouped by sections.
        /// </summary>
        /// <param name="id"> Id of the plan. </param>
        [HttpGet]
        [Route("api/plan/{id}/sections")]
        public async Task<HttpResponseMessage> GetTasksForPlanAsync(int id)
        {
            List<SectionDto> sections = await planService.GetTasksForPlanAsync(id);
            if (sections == null)
            {
                const string message = "Plan does not exist in database.";
                return Request.CreateErrorResponse(HttpStatusCode.NoContent, message);
            }
            return Request.CreateResponse<IEnumerable<SectionDto>>(HttpStatusCode.OK, sections);
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
            var dtoList = planService.GetSomeAmount(prevAmount, amount);
            if (dtoList == null || dtoList.Count == 0)
            {
                const string errorMessage = "No plans in database.";
                return Request.CreateErrorResponse(HttpStatusCode.NoContent, errorMessage);
            }
            return Request.CreateResponse<IEnumerable<PlanDto>>(HttpStatusCode.OK, dtoList);
        }

        /// <summary>
        /// Gets all tasks assigned to plan.
        /// </summary>
        /// <param name="planId"> Id of plan. </param>
        [AllowAnonymous]
        [HttpGet]
        [Route("api/plan/{planId}/tasks")]
        public async Task<HttpResponseMessage> GetAllTasksAsync(int planId)
        {
            List<TaskDto> dtosList = await planService.GetAllTasksAsync(planId);
            if (dtosList == null || dtosList.Count == 0)
            {
                const string message = "Plan does not contain any task.";
                return Request.CreateErrorResponse(HttpStatusCode.NoContent, message);
            }
            return Request.CreateResponse<IEnumerable<TaskDto>>(HttpStatusCode.OK, dtosList);
        }

        /// <summary>
        /// Gets all Plantask ids of concrete plan.
        /// </summary>
        /// <param name="planId"> Id of plan. </param>
        [HttpGet]
        [Route("api/plan/{planId}/plantaskids")]
        public async Task<HttpResponseMessage> GetAllPlanTaskIdsAsync(int planId)
        {
            var idsList = await planService.GetAllPlanTaskidsAsync(planId);
            if (idsList == null || idsList.Count == 0)
            {
                const string message = "Plan does not contain any plantask.";
                return Request.CreateErrorResponse(HttpStatusCode.NoContent, message);
            }
            return Request.CreateResponse<IEnumerable<int>>(HttpStatusCode.OK, idsList);
        }

        /// <summary>
        /// Creates new plan.
        /// </summary>
        /// <param name="value"> New plan to be created. </param>
        [Authorize(Roles = "Mentor")]
        [HttpPost]
        [Route("api/plan")]
        public async Task<HttpResponseMessage> PostAsync([FromBody]PlanDto value)
        {
            try
            {
                var success = await planService.AddAsync(value);
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
            const string message = "Incorrect request syntax.";
            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, message);
        }

        /// <summary>
        /// Creates new plan and returns id of the created plan.
        /// </summary>
        /// <param name="value"> New plan to be created. </param>
        [Authorize(Roles = "Mentor")]
        [HttpPost]
        [Route("api/plan/return")]
        public async Task<HttpResponseMessage> PostAndReturnIdAsync([FromBody]PlanDto value)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
                }
                int? result = await planService.AddAndGetIdAsync(value);
                if (result != null)
                {
                    var log = $"Succesfully created plan {value.Name} with id = {result} by user with id = {value.CreatorId}";
                    tracer.Info(Request, ControllerContext.ControllerDescriptor.ControllerType.FullName, log);
                    return Request.CreateResponse(HttpStatusCode.OK, result);
                }
            }
            catch (EntityException e)
            {
                tracer.Error(Request, ControllerContext.ControllerDescriptor.ControllerType.FullName, e);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }
            tracer.Warn(Request, ControllerContext.ControllerDescriptor.ControllerType.FullName, "Error occured on creating plan");
            const string message = "Incorrect request syntax.";
            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, message);
        }

        /// <summary>
        /// Updates existing plan.
        /// </summary>
        /// <param name="id"> Id of plan to be updated. </param>
        /// <param name="value"> Plan info to be updated. </param>
        [Authorize(Roles = "Mentor, Admin")]
        [HttpPut]
        [Route("api/plan/{id}")]
        public async Task<HttpResponseMessage> PutAsync(int id, [FromBody]PlanDto value)
        {
            try
            {
                var success = await planService.UpdateByIdAsync(value, id);
                if (success)
                {
                    var log = $"Succesfully updated plan {value.Name} with id = {value.Id} by user with id = {value.Modid}";
                    tracer.Info(Request, ControllerContext.ControllerDescriptor.ControllerType.FullName, log);
                    const string okMessage = "Succesfully updated plan.";
                    return Request.CreateResponse(HttpStatusCode.OK, okMessage);
                }
            }
            catch (EntityException e)
            {
                tracer.Error(Request, ControllerContext.ControllerDescriptor.ControllerType.FullName, e);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }
            tracer.Warn(Request, ControllerContext.ControllerDescriptor.ControllerType.FullName, "Error occured on updating plan");
            const string message = "Incorrect request syntax or plan does not exist.";
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
        [Authorize(Roles = "Mentor, Admin")]
        [HttpPut]
        [Route("api/plan/{id}/task/{taskId}")]
        public async Task<HttpResponseMessage> PutTaskToPlanAsync(int id, int taskId,string sectionId, string priority)
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
                bool success = await planService.AddTaskToPlanAsync(id, taskId, section, priorityNew);
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
        public async Task<HttpResponseMessage> PostImageAsync(int id)
        {
            if (!await planService.ContainsId(id))
            {
                const string errorMessage = "No plan with this id in database.";
                return Request.CreateResponse(HttpStatusCode.NoContent, errorMessage);
            }
            if (HttpContext.Current.Request.Files.Count != 1)
            {
                const string errorMessage = "Only one image can be sent.";
                return Request.CreateResponse(HttpStatusCode.BadRequest, errorMessage);
            }
            try
            {
                var postedFile = HttpContext.Current.Request.Files[0];
                if (postedFile.ContentLength > 0)
                {
                    var allowedFileExtensions = new List<string>(Constants.ImageRestrictions.Extensions);
                    var extension = postedFile.FileName.Substring(postedFile.FileName.LastIndexOf('.')).ToLower();
                    if (!allowedFileExtensions.Contains(extension))
                    {
                        const string errorMessage = "Types allowed only .jpeg .jpg .png";
                        return Request.CreateResponse(HttpStatusCode.BadRequest, errorMessage);
                    }
                    const int maxContentLength = Constants.ImageRestrictions.MaxSize;
                    if (postedFile.ContentLength > maxContentLength)
                    {
                        const string errorMessage = "Please Upload a file upto 1 mb.";
                        return Request.CreateResponse(HttpStatusCode.BadRequest, errorMessage);
                    }
                    byte[] imageData;
                    using (var binaryReader = new BinaryReader(postedFile.InputStream))
                    {
                        imageData = binaryReader.ReadBytes(postedFile.ContentLength);
                    }
                    await planService.SetImageAsync(id, imageData, postedFile.FileName);
                    const string okMessage = "Successfully created image.";
                    return Request.CreateResponse(HttpStatusCode.OK, okMessage);
                }
                const string emptyImageMessage = "Empty image.";
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
        public async Task<HttpResponseMessage> GetImageAsync(int id)
        {
            try
            {
                if (!await planService.ContainsId(id))
                {
                    const string errorMessage = "No plan with this id in database.";
                    return Request.CreateResponse(HttpStatusCode.NoContent, errorMessage);
                }
                var dto = await planService.GetImageAsync(id);
                if (dto == null)
                {
                    const string message = "No image for this plan in database.";
                    return Request.CreateResponse(HttpStatusCode.NoContent, message);
                }
                return Request.CreateResponse(HttpStatusCode.OK, dto);
            }
            catch (EntityException e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
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
            var lines = q.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            var dto = planService.Search(lines);
            if (dto == null || dto.Count == 0)
            {
                const string message = "No plans found.";
                return Request.CreateErrorResponse(HttpStatusCode.NoContent, message);
            }
            return Request.CreateResponse<IEnumerable<PlanDto>>(HttpStatusCode.OK, dto);
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
