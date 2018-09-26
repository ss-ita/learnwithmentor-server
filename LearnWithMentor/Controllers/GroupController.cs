using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using LearnWithMentor.Filters;
using LearnWithMentorDTO;
using LearnWithMentorBLL.Interfaces;
using System.Web.Http.Tracing;
using System.Data.Entity.Core;
using LearnWithMentorDAL.Entities;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

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
        private readonly IUserIdentityService userIdentityService;

        /// <summary>
        /// Creates new instance of controller.
        /// </summary>
        public GroupController(IGroupService groupService, IUserService userService, IUserIdentityService userIdentityService, ITraceWriter tracer)
        {
            this.userService = userService;
            this.groupService = groupService;
            this.userIdentityService = userIdentityService;
            this.tracer = tracer;
        }

        // GET api/<controller>
        /// <summary>
        /// Returns group by mentor Id "api/group/mentor/{id}"
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("api/group/mentor/{id}")]
        public async Task<HttpResponseMessage> GetByMentor(int id)
        {
            IEnumerable<GroupDto> allGroups = await groupService.GetGroupsByMentor(id);
            if (allGroups != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, allGroups);
            }
            return Request.CreateErrorResponse(HttpStatusCode.NotFound, $"No groups for the mentor in database. (mentorId = {id})");
        }

        /// <summary>
        /// Returns group by Id "api/group/{id}"
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/group/{id}")]
        public async Task<HttpResponseMessage> GetById(int id)
        {
            GroupDto group = await groupService.GetGroupById(id);
            if (group != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, group);
            }
            return Request.CreateErrorResponse(HttpStatusCode.NotFound, $"There isn't group with id = {id}");
        }

        /// <summary>
        /// Returns plans for specific group by group Id "api/group/{id}/plans"
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/group/{id}/plans")]
        public async Task<HttpResponseMessage> GetPlans(int id)
        {
            try
            {
                IEnumerable<PlanDto> group = await groupService.GetPlans(id);
                if (group != Enumerable.Empty<PlanDto>())
                {
                    return Request.CreateResponse(HttpStatusCode.OK, group);
                }
                return Request.CreateErrorResponse(HttpStatusCode.NoContent, "There no plans in this group.");
            }
            catch (EntityException e)
            {
                tracer.Error(Request, ControllerContext.ControllerDescriptor.ControllerType.FullName, e);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }
        }

        /// <summary>
        /// Returns users that belong to group by group Id "api/group/{id}/users"
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/group/{id}/users")]
        public async Task<HttpResponseMessage> GetUsers(int id)
        {
            try
            {
                IEnumerable<UserIdentityDto> group = await groupService.GetUsers(id);
                if (group != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, group);
                }
                return Request.CreateErrorResponse(HttpStatusCode.NoContent, "There are no users in the group.");
            }
            catch (EntityException e)
            {
                tracer.Error(Request, ControllerContext.ControllerDescriptor.ControllerType.FullName, e);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }
        }

        /// <summary>
        /// Returns users that belong to group by group Id "api/group/{id}/users"
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/group/{id}/userimages")]
        public async Task<HttpResponseMessage> GetUsersWithImage(int id)
        {
            try
            {
                var group = await groupService.GetUsersWithImage(id);
                if (group != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, group);
                }
                return Request.CreateErrorResponse(HttpStatusCode.NoContent, "There are no users with image in the group.");
            }
            catch (EntityException e)
            {
                tracer.Error(Request, ControllerContext.ControllerDescriptor.ControllerType.FullName, e);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }
        }

        /// <summary>
        /// Returns users that is not belong to group by group Id "api/group/{id}/users"
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        [Authorize(Roles = "Mentor")]
        [HttpGet]
        [Route("api/group/{groupId}/users/notingroup")]
        public async Task<HttpResponseMessage> GetUsersNotInCurrentGroup(int groupId)
        {
            IEnumerable<UserIdentityDto> group = await groupService.GetUsersNotInGroup(groupId);
            if (group != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, group);
            }
            return Request.CreateErrorResponse(HttpStatusCode.NotFound, $"There isn't users outside of the group id = {groupId}");
        }
              
        /// <summary>
        /// Returns all plans not used in current group.
        /// </summary>
        [Authorize(Roles = "Mentor")]
        [HttpGet]
        [Route("api/plan/notingroup/{groupId}")]
        public async Task<HttpResponseMessage> GetPlansNotUsedInCurrentGroup(int groupId)
        {
            var notUsedPlans = await groupService.GetPlansNotUsedInGroup(groupId);
            if (notUsedPlans != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, notUsedPlans);
            }
            const string errorMessage = "No plans in database.";
            return Request.CreateErrorResponse(HttpStatusCode.NoContent, errorMessage);
        }


        /// <summary>
        /// Create new group
        /// </summary>
        /// <param name="group"></param>
        /// <returns></returns>
        [Authorize(Roles = "Mentor")]
        [HttpPost]
        [Route("api/group")]
        public async Task<HttpResponseMessage> Post([FromBody]GroupDto group)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
                }
                var success = groupService.AddGroup(group);
                if (await success)
                {
                    const string log = "Group succesfully created.";
                    tracer.Info(Request, ControllerContext.ControllerDescriptor.ControllerType.FullName, log);
                    return Request.CreateResponse(HttpStatusCode.OK, $"Succesfully created group: {group.Name}.");
                }
                tracer.Warn(Request, ControllerContext.ControllerDescriptor.ControllerType.FullName, "Error occured on creating group");
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Group with this name already exists");
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
        [Authorize(Roles = "Mentor")]
        [HttpPut]
        [Route("api/group/{id}/user")]
        public async Task<HttpResponseMessage> PutUsersToGroup(int id, [FromBody] int[] userId)
        {
            try
            {
                var currentUserId = userIdentityService.GetUserId();
                int? mentorId = await groupService.GetMentorIdByGroup(id);
                if (mentorId != currentUserId)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Authorization denied.");
                }
                bool success = await groupService.AddUsersToGroup(userId, id);
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
        [Authorize(Roles = "Mentor")]
        [HttpPut]
        [Route("api/group/{id}/plan")]
        public async Task<HttpResponseMessage> PutPlansToGroup(int id, [FromBody] int[] planId)
        {
            try
            {
                var userId = userIdentityService.GetUserId();
                int? mentorId = await groupService.GetMentorIdByGroup(id);
                if (mentorId != userId)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Authorization denied.");
                }
                bool success = await groupService.AddPlansToGroup(planId, id);
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
        /// Returns plans that is no used in current group and which names contain string key.
        /// </summary>
        /// <param name="searchKey">Key for search.</param>
        /// <param name="groupId">Id of the plan.</param>
        [HttpGet]
        [Route("api/group/searchinNotUsedPlan")]
        public async Task<HttpResponseMessage> SearchPlansNotUsedInCurrentGroup(string searchKey, int groupId)
        {
            try
            {
                if (searchKey == null)
                {
                    return await GetPlansNotUsedInCurrentGroup(groupId);
                }
                var lines = searchKey.Split(new [] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                var plansList = await groupService.SearchPlansNotUsedInGroup(lines, groupId);
                if (plansList == null)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NoContent, "This plan does not exist.");
                }
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
        public async Task<HttpResponseMessage> SearchUsersNotUsedInCurrentGroup(string searchKey, int groupId)
        {
            try
            {
                if (searchKey == null)
                {
                    return await GetUsersNotInCurrentGroup(groupId);
                }
                var lines = searchKey.Split(new [] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                var usersList = groupService.SearchUserNotInGroup(lines, groupId);
                if (usersList == null)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NoContent, "This user does not exist.");
                }
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
        /// <param name="userToRemoveId">Id of the user to remove.</param>
        [Authorize(Roles = "Mentor")]
        [HttpDelete]
        [Route("api/group/removeUserFromGroup")]
        public async Task<HttpResponseMessage> RemoveUserFromCurrentGroup(int groupId, int userToRemoveId)
        {
            try
            {
                var id = userIdentityService.GetUserId();
                int? mentorId = await groupService.GetMentorIdByGroup(groupId);
                if (mentorId != id)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Authorization denied.");
                }
                bool successfullyRemoved = await groupService.RemoveUserFromGroup(groupId, userToRemoveId);
                if (successfullyRemoved)
                {
                    var log = $"Succesfully removed user with id = {userToRemoveId} from group with id = {groupId}";
                    tracer.Info(Request, ControllerContext.ControllerDescriptor.ControllerType.FullName, log);
                    return Request.CreateResponse(HttpStatusCode.OK, "User succesfully removed.");
                }
                tracer.Warn(Request, ControllerContext.ControllerDescriptor.ControllerType.FullName, "Error occured on removing user from the group");
                return Request.CreateErrorResponse(HttpStatusCode.NoContent, "Incorrect request syntax: user or group does not exist.");
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
        /// <param name="planToRemoveId">Id of the plan to remove.</param>
        [Authorize(Roles = "Mentor")]
        [HttpDelete]
        [Route("api/group/removePlanFromGroup")]
        public async Task<HttpResponseMessage> RemovePlanFromCurrentGroup(int groupId, int planToRemoveId)
        {
            try
            {
                bool successfullyRemoved = await groupService.RemovePlanFromGroup(groupId, planToRemoveId);
                if (successfullyRemoved)
                {
                    var log = $"Succesfully removed plan with id = {planToRemoveId} from group with id = {groupId}";
                    tracer.Info(Request, ControllerContext.ControllerDescriptor.ControllerType.FullName, log);
                    return Request.CreateResponse(HttpStatusCode.OK, "Plan succesfully removed from group.");
                }
                tracer.Warn(Request, ControllerContext.ControllerDescriptor.ControllerType.FullName, "Error occured on removing plan from the group");
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Incorrect request syntax: plan or group does not exist.");
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
        /// <returns></returns>
        [HttpGet]
        [Route("api/group/mygroups")]
        public async Task<HttpResponseMessage> GetUserGroups()
        {
            try
            {
                var userId = userIdentityService.GetUserId();
                if (!(await userService.ContainsId(userId)))
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NoContent, $"There are no users with id = {userId}");
                }
                if (await groupService.GroupsCount() == 0)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NoContent, "There are no groups in database.");
                }
                var groups = await groupService.GetUserGroups(userId);
                if (groups == null)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "There are no groups for this user");
                }
                return Request.CreateResponse(HttpStatusCode.OK, groups);
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
    }
}
