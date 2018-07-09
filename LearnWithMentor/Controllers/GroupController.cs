﻿using System;
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

namespace LearnWithMentor.Controllers
{
    /// <summary>
    /// Controller for groups.
    /// </summary>
    [Authorize]
    [JwtAuthentication]
    public class GroupController : ApiController
    {
        private readonly IGroupService groupService;
        private readonly IUserService userService;
        private readonly ITraceWriter tracer;

        /// <summary>
        /// Creates new instance of controller.
        /// </summary>
        public GroupController(IGroupService groupService, IUserService userService, ITraceWriter tracer)
        {
            this.userService = userService;
            this.groupService = groupService;
            this.tracer = tracer;
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
        /// Returns users that is not belong to group by group Id "api/group/{id}/users"
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/group/{groupId}/users/notingroup")]
        public HttpResponseMessage GetUsersNotInCurrentGroup(int groupId)
        {
            var group = groupService.GetUsersNotInGroup(groupId);
            if (group != null)
                return Request.CreateResponse(HttpStatusCode.OK, group);
            else
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, $"There isn't users outside of the group id = {groupId}");
        }

        /// <summary>
        /// Returns all plans not used in current group.
        /// </summary>
        [HttpGet]
        [Route("api/plan/notingroup/{groupId}")]
        public HttpResponseMessage GetPlansNotUsedInCurrentGroup(int groupId)
        {
            var notUsedPlans = groupService.GetPlansNotUsedInGroup(groupId);
            if (notUsedPlans == null)
            {
                var errorMessage = "No plans in database.";
                return Request.CreateErrorResponse(HttpStatusCode.NoContent, errorMessage);
            }
            return Request.CreateResponse(HttpStatusCode.OK, notUsedPlans);
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
                    var log = $"Succesfully created group {group.Name} with id = {group.Id} with mentor id = {group.MentorId}";
                    tracer.Info(Request, ControllerContext.ControllerDescriptor.ControllerType.FullName, log);
                    return Request.CreateResponse(HttpStatusCode.OK, $"Succesfully created group: {group.Name}.");
                }
                tracer.Warn(Request, ControllerContext.ControllerDescriptor.ControllerType.FullName, "Error occured on creating group");
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Creation error.");
            }
            catch (EntityException e)
            {
                tracer.Error(Request, ControllerContext.ControllerDescriptor.ControllerType.FullName, e);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
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
                    var log = $"Succesfully add user with id {userId} to group with id = {id}";
                    tracer.Info(Request, ControllerContext.ControllerDescriptor.ControllerType.FullName, log);
                    return Request.CreateResponse(HttpStatusCode.OK, $"Succesfully added users to group ({id}).");
                }
                tracer.Warn(Request, ControllerContext.ControllerDescriptor.ControllerType.FullName, "Error occured on adding user to group");
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Incorrect request syntax or user or group does not exist.");
            }
            catch (EntityException e)
            {
                tracer.Error(Request, ControllerContext.ControllerDescriptor.ControllerType.FullName, e);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
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
                    var log = $"Succesfully add plan with id = {planId} to group with id = {id}";
                    tracer.Info(Request, ControllerContext.ControllerDescriptor.ControllerType.FullName, log);
                    return Request.CreateResponse(HttpStatusCode.OK, $"Succesfully added plans to group ({id}).");
                }
                tracer.Warn(Request, ControllerContext.ControllerDescriptor.ControllerType.FullName, "Error occured on adding plan to group");
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Incorrect request syntax or plan or group does not exist.");
            }
            catch (EntityException e)
            {
                tracer.Error(Request, ControllerContext.ControllerDescriptor.ControllerType.FullName, e);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
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

        /// <summary>
        /// Returns plans that is no used in current group and which names contain string key.
        /// </summary>
        /// <param name="searchKey">Key for search.</param>
        /// <param name="groupId">Id of the plan.</param>
        [HttpGet]
        [Route("api/group/searchinNotUsedPlan")]
        public HttpResponseMessage SearchPlansNotUsedInCurrentGroup(string searchKey, int groupId)
        {
            try
            {
                if (searchKey == null)
                {
                    return GetPlansNotUsedInCurrentGroup(groupId);
                }
                string[] lines = searchKey.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                var plansList = groupService.SearchPlansNotUsedInGroup(lines,groupId);
                if (plansList == null)
                    return Request.CreateErrorResponse(HttpStatusCode.NoContent, "This plan does not exist.");
                return Request.CreateResponse(HttpStatusCode.OK, plansList);
            }
            catch (EntityException e)
            {
                tracer.Error(Request, ControllerContext.ControllerDescriptor.ControllerType.FullName, e);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }
        }

        /// <summary>
        /// Returns users that is not involved in current group and which names contain string key.
        /// </summary>
        /// <param name="searchKey">Key for search.</param>
        /// <param name="groupId">Id of the plan.</param>
        [HttpGet]
        [Route("api/group/searchinNotInvolvedUsers")]
        public HttpResponseMessage SearchUsersNotUsedInCurrentGroup(string searchKey, int groupId)
        {
            try
            {
                if (searchKey == null)
                {
                    return GetUsersNotInCurrentGroup(groupId);
                }
                string[] lines = searchKey.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                var usersList = groupService.SearchUserNotInGroup(lines, groupId);
                if (usersList == null)
                    return Request.CreateErrorResponse(HttpStatusCode.NoContent, "This user does not exist.");
                return Request.CreateResponse(HttpStatusCode.OK, usersList);
            }
            catch (EntityException e)
            {
                tracer.Error(Request, ControllerContext.ControllerDescriptor.ControllerType.FullName, e);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }
        }

        /// <summary>
        /// Removes user from current group.
        /// </summary>
        /// <param name="groupId">Group ID where user should be removed.</param>
        /// <param name="userId">Id of the user to remove.</param>
        [HttpDelete]
        [Route("api/group/removeUserFromGroup")]
        public HttpResponseMessage RemoveUserFromCurrentGroup(int groupId, int userToRemoveId)
        {
            try
            {
                bool successfullyRemoved = groupService.RemoveUserFromGroup(groupId, userToRemoveId);
                if (successfullyRemoved)
                {
                    var log = $"Succesfully removed user with id = {userToRemoveId} to group with id = {groupId}";
                    tracer.Info(Request, ControllerContext.ControllerDescriptor.ControllerType.FullName, log);
                    return Request.CreateResponse(HttpStatusCode.OK, $"Succesfully removed user from group ({groupId}).");
                }
                tracer.Warn(Request, ControllerContext.ControllerDescriptor.ControllerType.FullName, "Error occured on removing user from the group");
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Incorrect request syntax or user or group does not exist.");
            }
            catch (EntityException e)
            {
                tracer.Error(Request, ControllerContext.ControllerDescriptor.ControllerType.FullName, e);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }
            
        }

        /// <summary>
        /// Removes plan from current group.
        /// </summary>
        /// <param name="groupId">Group ID where user should be removed.</param>
        /// <param name="planId">Id of the plan to remove.</param>
        [HttpDelete]
        [Route("api/group/removePlanFromGroup")]
        public HttpResponseMessage RemovePlanFromCurrentGroup(int groupId, int planToRemoveId)
        {
            try
            {
                bool successfullyRemoved = groupService.RemovePlanFromGroup(groupId, planToRemoveId);
                if (successfullyRemoved)
                {
                    var log = $"Succesfully removed plan with id = {planToRemoveId} to group with id = {groupId}";
                    tracer.Info(Request, ControllerContext.ControllerDescriptor.ControllerType.FullName, log);
                    return Request.CreateResponse(HttpStatusCode.OK, $"Succesfully removed plan from group ({groupId}).");
                }
                tracer.Warn(Request, ControllerContext.ControllerDescriptor.ControllerType.FullName, "Error occured on removing user from the group");
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Incorrect request syntax or plan or group does not exist.");
            }
            catch (EntityException e)
            {
                tracer.Error(Request, ControllerContext.ControllerDescriptor.ControllerType.FullName, e);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }

        }
        /// <summary>
        /// If user: strudent - returns its learning groups, if mentor - returns mentored groups, if admin - returns all groups."
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/group/user/{userId}/groups")]
        public HttpResponseMessage GetUserGroups(int userId)
        {
            if(!userService.ContainsId(userId))
                return Request.CreateErrorResponse(HttpStatusCode.NoContent, $"There are no users with id = {userId}");
            if (groupService.GroupsCount()==0)
                return Request.CreateErrorResponse(HttpStatusCode.NoContent, $"There are no groups in database.");
            var groups = groupService.GetUserGroups(userId);
            if (groups != null)
                return Request.CreateResponse(HttpStatusCode.OK, groups);
            else
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, $"There are no groups for this user");
        }
    }
}
