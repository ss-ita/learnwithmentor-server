﻿using System;
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
        private readonly ITraceWriter _tracer;

        /// <summary>
        /// Creates new instance of controller.
        /// </summary>
        public PlanController()
        {
            planService = new PlanService();
            taskService = new TaskService();
            _tracer = new NLogger();
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
                    _tracer.Info(Request, ControllerContext.ControllerDescriptor.ControllerType.FullName, log);
                    var okMessage = $"Succesfully created plan: {value.Name}";
                    return Request.CreateResponse(HttpStatusCode.OK, okMessage);
                }
            }
            catch (Exception e)
            {
                _tracer.Error(Request, ControllerContext.ControllerDescriptor.ControllerType.FullName, e);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }
            _tracer.Warn(Request, ControllerContext.ControllerDescriptor.ControllerType.FullName, "Error occured on creating plan");
            var message = "Incorrect request syntax.";
            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, message);
        }

        /// <summary>
        /// Updates existing plan.
        /// </summary>
        /// <param name="id"> Id of plan to be updated. </param>
        /// <param name="value"> Plan info to be updated. </param>
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
                    _tracer.Info(Request, ControllerContext.ControllerDescriptor.ControllerType.FullName, log);
                    var okMessage = $"Succesfully updated plan.";
                    return Request.CreateResponse(HttpStatusCode.OK, okMessage);
                }
            }
            catch (Exception e)
            {
                _tracer.Error(Request, ControllerContext.ControllerDescriptor.ControllerType.FullName, e);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }
            _tracer.Warn(Request, ControllerContext.ControllerDescriptor.ControllerType.FullName, "Error occured on updating plan");
            var message = "Incorrect request syntax or plan does not exist.";
            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, message);
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
