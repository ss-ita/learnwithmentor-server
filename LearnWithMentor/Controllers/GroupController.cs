using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using LearnWithMentor.Filters;
using LearnWithMentorDTO;
using LearnWithMentorBLL.Interfaces;
using LearnWithMentorBLL.Infrastructure;
using LearnWithMentorBLL.Services;

namespace LearnWithMentor.Controllers
{
    [Authorize]
    [JwtAuthentication]
    public class GroupController : ApiController
    {
        private readonly IGroupService groupService;

        public GroupController()
        {
            groupService = new GroupService();
        }
        // GET api/<controller>
        /// <summary>
        /// Returns group by mentor Id "api/group/mentor/{id}"
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("api/group/mentor/{id}")]
        public HttpResponseMessage GetByMentor(int id)
        {
            var allGroups = groupService.GetGroupsByMentor(id);
            if (allGroups != null)
                return Request.CreateResponse(HttpStatusCode.OK, allGroups);
            else
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, $"No groups for the mentor in database. (mentorId = {id})");
        }
        /// <summary>
        /// Returns group by Id "api/group/{id}"
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/group/{id}")]
        public HttpResponseMessage GetById(int id)
        {
            var group = groupService.GetGroupById(id);
            if (group != null)
                return Request.CreateResponse(HttpStatusCode.OK, group);
            else
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, $"There isn't group with id = {id}");
        }
        /// <summary>
        /// Returns plans for specific group by group Id "api/group/{id}/plans"
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/group/{id}/plans")]
        public HttpResponseMessage GetPlans(int id)
        {
            var group = groupService.GetPlans(id);
            if (group != null)
                return Request.CreateResponse(HttpStatusCode.OK, group);
            else
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, $"There isn't plans for the group id = {id}");
        }
        /// <summary>
        /// Returns users that belong to group by group Id "api/group/{id}/users"
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/group/{id}/users")]
        public HttpResponseMessage GetUsers(int id)
        {
            var group = groupService.GetUsers(id);
            if (group != null)
                return Request.CreateResponse(HttpStatusCode.OK, group);
            else
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, $"There isn't users in the group id = {id}");
        }
        /// <summary>
        /// Create new group
        /// </summary>
        /// <param name="group"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/group")]
        public HttpResponseMessage Post([FromBody]GroupDTO group)
        {
            try
            {
                if (!ModelState.IsValid)
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
                bool success = groupService.AddGroup(group);
                if (success)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, $"Succesfully created group: {group.Name}.");
                }
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Creation error.");
            }
            catch (Exception exception)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, exception);
            }
        }
        /// <summary>
        /// Add users array to group by group id. You have to pass users Id as int[] in body "api/group/{id}/user"
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("api/group/{id}/user")]
        public HttpResponseMessage PutUsersToGroup(int id, [FromBody] int[] userId)
        {
            try
            {
                bool success = groupService.AddUsersToGroup(userId, id);
                if (success)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, $"Succesfully added users to group ({id}).");
                }
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Incorrect request syntax or user or group does not exist.");
            }
            catch (Exception exception)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, exception);
            }
        }
        /// <summary>
        /// Add plans array to group by groupId. You have to pass plans Id as int[] in body "api/group/{id}/plan"
        /// </summary>
        /// <param name="id"></param>
        /// <param name="planId"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("api/group/{id}/plan")]
        public HttpResponseMessage PutPlansToGroup(int id, [FromBody] int[] planId)
        {
            try
            {
                bool success = groupService.AddPlansToGroup(planId, id);
                if (success)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, $"Succesfully added plans to group ({id}).");
                }
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Incorrect request syntax or plan or group does not exist.");
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
            groupService.Dispose();
            base.Dispose(disposing);
        }
    }
}