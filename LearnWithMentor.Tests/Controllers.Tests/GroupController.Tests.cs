using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using NUnit.Framework;
using Moq;
using System.Data.Entity.Core;
using LearnWithMentor.Controllers;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Tracing;
using LearnWithMentorBLL.Interfaces;
using LearnWithMentorDTO;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace LearnWithMentor.Tests.Controllers.Tests
{
    [TestFixture]
    public class GroupControllerTests
    {
        private GroupController groupController;
        private Mock<IGroupService> groupServiceMock;
        private List<GroupDto> groups;
        private Mock<ITraceWriter> traceWriterMock;
        private Mock<IUserService> userServiceMock;
        private Mock<IUserIdentityService> userIdentityServiceMock;


        [SetUp]
        public void SetUp()
        {
            groups = new List<GroupDto>()
            {
                new GroupDto(1, "name1", 1, "mentorName1"),
                new GroupDto(2, "name2", 2, "mentorName2"),
                new GroupDto(3, "name3", 3, "mentorName3"),
                new GroupDto(4, "name4", 4, "mentorName4")
            };

            groupServiceMock = new Mock<IGroupService>();
            traceWriterMock = new Mock<ITraceWriter>();
            userServiceMock = new Mock<IUserService>();
            userIdentityServiceMock = new Mock<IUserIdentityService>();

            var userPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Role, "Admin")
            }));

            groupController = new GroupController(groupServiceMock.Object, userServiceMock.Object, userIdentityServiceMock.Object, traceWriterMock.Object);
            groupController.ControllerContext.RequestContext.Principal = userPrincipal;
            groupController.Request = new HttpRequestMessage();
            groupController.Configuration = new HttpConfiguration();
            groupController.ControllerContext.ControllerDescriptor = new HttpControllerDescriptor(
            groupController.Configuration, "GroupController", groupController.GetType());
        }

        public void ValidateViewModel<TModel, TController>(TController controller, TModel ModelToValidate)
        where TController : ApiController
        {
            var validationContext = new ValidationContext(ModelToValidate, null, null);
            var validationResults = new List<ValidationResult>();
            Validator.TryValidateObject(ModelToValidate, validationContext, validationResults, true);
            foreach (var validationResult in validationResults)
            {
                controller.ModelState.AddModelError(validationResult.MemberNames.FirstOrDefault() ?? string.Empty, validationResult.ErrorMessage);
            }
        }

        [TearDown]
        public void TearDown()
        {
            groupController.Dispose();
            groupServiceMock = null;
            userServiceMock = null;
            userIdentityServiceMock = null;
            traceWriterMock = null;
            groups = null;
        }
        #region GetGroupByIdTest
        [Test]
        public async Task GetGroupByIdTest_ShouldReturnTask()
        {
            groupServiceMock.Setup(u => u.GetGroupByIdAsync(It.IsAny<int>())).ReturnsAsync(
                 (int i) => groups.Single(x => x.Id == i));

            HttpResponseMessage response = await groupController.GetByIdAsync(3);
            var successfull =  response.TryGetContentValue<GroupDto>(out var groupDTO);
            var expected = await groupServiceMock.Object.GetGroupByIdAsync(3);
            var actual = groupDTO;

            Assert.IsTrue(successfull);
            Assert.AreEqual(expected.Id, actual.Id);
            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);
        }

        [Test]
        public async Task GetGroupByIdTest_ShouldReturnNotFoundResponse()
        {
            groupServiceMock.Setup(u => u.GetGroupByIdAsync(It.IsAny<int>())).Returns(Task.FromResult<GroupDto>(null));

            HttpResponseMessage response = await groupController.GetByIdAsync(1);
            var expected = HttpStatusCode.NotFound;
            var actual = response.StatusCode;

            Assert.AreEqual(expected, actual);
        }
        #endregion
        #region GetGroupByMentor
        [Test]
        public async Task GetGroupByMentor_ShouldReturnGroupsForMentor()
        {
            groupServiceMock.Setup(g => g.GetGroupsByMentorAsync(It.IsAny<int>())).ReturnsAsync(new List<GroupDto>());

            HttpResponseMessage response = await groupController.GetByMentorAsync(2);
            var successfull = response.TryGetContentValue<List<GroupDto>>(out var groupDTOs);
            var expected = await groupServiceMock.Object.GetGroupsByMentorAsync(2);
            var actual = groupDTOs;

            Assert.IsTrue(successfull);
            Assert.AreEqual(expected, actual);
            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);
        }

        [Test]
        public async Task GetGroupByMentor_ShouldReturnNotFoundResponse()
        {
            groupServiceMock.Setup(u => u.GetGroupsByMentorAsync(It.IsAny<int>())).Returns(Task.FromResult<IEnumerable<GroupDto>>(null));
            HttpResponseMessage response = await groupController.GetByMentorAsync(6);
            HttpStatusCode expected = HttpStatusCode.NotFound;
            HttpStatusCode actual = response.StatusCode;
            Assert.AreEqual(expected, actual);
        }
        #endregion
        #region GetAllPlansByGroupId
        [Test]
        public async Task GetAllPlansByGroupId_ShouldReturnOk()
        {
            groupServiceMock.Setup(p => p.GetPlansAsync(It.IsAny<int>())).ReturnsAsync(new List<PlanDto>());

            var response = await groupController.GetPlansAsync(4);
            var expected = HttpStatusCode.OK;
            var actual = response.StatusCode;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task GetAllPlansByGroupId_ShouldReturnNoContent()
        {
            groupServiceMock.Setup(p => p.GetPlansAsync(It.IsAny<int>())).ReturnsAsync(() => Enumerable.Empty<PlanDto>());

            var response = await groupController.GetPlansAsync(99);
            var expected = HttpStatusCode.NoContent;
            var actual = response.StatusCode;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task GetAllPlansByGroupId_ShouldReturInternalServerError()
        {
            groupServiceMock.Setup(p => p.GetPlansAsync(It.IsAny<int>())).Throws(new EntityException());

            var response = await groupController.GetPlansAsync(99);
            var expected = HttpStatusCode.InternalServerError;
            var actual = response.StatusCode;

            Assert.AreEqual(expected, actual);
        }
        #endregion
        #region GetUsersWithImage
        [Test]
        public async Task GetUsersWithImage_ShouldReturnOk()
        {
            groupServiceMock.Setup(p => p.GetUsersWithImageAsync(It.IsAny<int>())).ReturnsAsync(new List<UserWithImageDto>());

            HttpResponseMessage response = await groupController.GetUsersWithImageAsync(4);
            var expected = HttpStatusCode.OK;
            var actual = response.StatusCode;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task GetUsersWithImage_ShouldReturnNoContent()
        {
            groupServiceMock.Setup(p => p.GetUsersWithImageAsync(It.IsAny<int>())).ReturnsAsync(() => null);

            HttpResponseMessage response = await groupController.GetUsersWithImageAsync(4);
            var expected = HttpStatusCode.NoContent;
            var actual = response.StatusCode;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task GetUsersWithImage_ShouldReturnInternalServerError()
        {
            groupServiceMock.Setup(p => p.GetUsersWithImageAsync(It.IsAny<int>())).Throws(new EntityException());

            HttpResponseMessage response = await groupController.GetUsersWithImageAsync(99);
            var expected = HttpStatusCode.InternalServerError;
            var actual = response.StatusCode;

            Assert.AreEqual(expected, actual);
        }
        #endregion
        #region GetUsersNotInCurrentGroup
        [Test]
        public async Task GetUsersNotInCurrentGroupShouldReturnOk()
        {
            groupServiceMock.Setup(u => u.GetUsersNotInGroupAsync(It.IsAny<int>())).ReturnsAsync(new List<UserIdentityDto>());
            HttpResponseMessage response = await groupController.GetUsersNotInCurrentGroupAsync(4);
            var expected = HttpStatusCode.OK;
            var actual = response.StatusCode;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task GetUsersNotInCurrentGroupShouldReturnNotFound()
        {
            groupServiceMock.Setup(u => u.GetUsersNotInGroupAsync(It.IsAny<int>())).Returns(Task.FromResult<IEnumerable<UserIdentityDto>>(null));
            HttpResponseMessage response = await groupController.GetUsersNotInCurrentGroupAsync(4);
            var expected = HttpStatusCode.NotFound;
            var actual = response.StatusCode;

            Assert.AreEqual(expected, actual);
        }
        #endregion
        #region GetPlansNotUsedInCurrentGroup
        [Test]
        public async Task GetPlansNotUsedInCurrentGroupShouldReturnOk()
        {
            groupServiceMock.Setup(u => u.GetPlansNotUsedInGroupAsync(It.IsAny<int>())).ReturnsAsync(new List<PlanDto>());
            var response = await groupController.GetPlansNotUsedInCurrentGroupAsync(4);
            var expected = HttpStatusCode.OK;
            var actual = response.StatusCode;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task GetPlansNotUsedInCurrentGroupShouldReturnNoContent()
        {
            groupServiceMock.Setup(u => u.GetPlansNotUsedInGroupAsync(It.IsAny<int>())).ReturnsAsync(() => null);
            var response = await groupController.GetPlansNotUsedInCurrentGroupAsync(4);
            var expected = HttpStatusCode.NoContent;
            var actual = response.StatusCode;

            Assert.AreEqual(expected, actual);
        }
        #endregion
        #region AddNewGroup
        [Test]
        public async Task AddNewGroupShouldReturnOk()
        {
            groupServiceMock.Setup(u => u.AddGroupAsync(It.IsAny<GroupDto>())).ReturnsAsync(true);
            var response = await groupController.PostAsync(groups[0]);
            var expected = HttpStatusCode.OK;
            var actual = response.StatusCode;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task AddNewGroupShouldReturnBadRequestValid()
        {
            groupServiceMock.Setup(u => u.AddGroupAsync(It.IsAny<GroupDto>())).ReturnsAsync(false);
            var response = await groupController.PostAsync(groups[0]);
            var expected = HttpStatusCode.BadRequest;
            var actual = response.StatusCode;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task AddNewGroupShouldReturnBadRequestInValid()
        {
            groupServiceMock.Setup(u => u.AddGroupAsync(It.IsAny<GroupDto>())).ReturnsAsync(true);
            var newGroup = new GroupDto { };
            ValidateViewModel<GroupDto, GroupController>(groupController, newGroup);
            var response = await groupController.PostAsync(groups[0]);
            var expected = HttpStatusCode.BadRequest;
            var actual = response.StatusCode;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task AddNewGroupShouldReturnInternalServerError()
        {
            groupServiceMock.Setup(u => u.AddGroupAsync(It.IsAny<GroupDto>())).Throws(new EntityException());
            var response = await groupController.PostAsync(groups[0]);
            var expected = HttpStatusCode.InternalServerError;
            var actual = response.StatusCode;

            Assert.AreEqual(expected, actual);
        }
        #endregion
        #region PutUsersToGroup
        [Test]
        public async Task PutUsersToGroupShouldReturnOk()
        {
            groupServiceMock.Setup(u => u.AddUsersToGroupAsync(It.IsAny<int[]>(), It.IsAny<int>())).ReturnsAsync(true);
            userIdentityServiceMock.Setup(u => u.GetUserId()).Returns(new int());
            groupServiceMock.Setup(u => u.GetMentorIdByGroup(It.IsAny<int>())).ReturnsAsync(new int());
            HttpResponseMessage response = await groupController.PutUsersToGroupAsync(5, new int[] { 1, 2, 3 });
            var expected = HttpStatusCode.OK;
            var actual = response.StatusCode;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task PutUsersToGroupShouldReturnUnauthorized()
        {
            groupServiceMock.Setup(u => u.AddUsersToGroupAsync(It.IsAny<int[]>(), It.IsAny<int>())).ReturnsAsync(true);
            userIdentityServiceMock.Setup(u => u.GetUserId()).Returns(new int());
            groupServiceMock.Setup(u => u.GetMentorIdByGroup(It.IsAny<int>())).ReturnsAsync(() => null);
            HttpResponseMessage response = await groupController.PutUsersToGroupAsync(5, new int[] { 1, 2, 3 });
            var expected = HttpStatusCode.Unauthorized;
            var actual = response.StatusCode;

            Assert.AreEqual(expected, actual);
        }
        [Test]
        public async Task PutUsersToGroupShouldReturnBadRequest()
        {
            groupServiceMock.Setup(u => u.AddUsersToGroupAsync(It.IsAny<int[]>(), It.IsAny<int>())).ReturnsAsync(false);
            userIdentityServiceMock.Setup(u => u.GetUserId()).Returns(new int());
            groupServiceMock.Setup(u => u.GetMentorIdByGroup(It.IsAny<int>())).ReturnsAsync(new int());
            HttpResponseMessage response = await groupController.PutUsersToGroupAsync(5, new int[] { 1, 2, 3 });
            var expected = HttpStatusCode.BadRequest;
            var actual =  response.StatusCode;

            Assert.AreEqual(expected, actual);
        }
        [Test]
        public async Task PutUsersToGroupShouldReturnInternalServerError()
        {
            groupServiceMock.Setup(u => u.AddUsersToGroupAsync(It.IsAny<int[]>(), It.IsAny<int>())).Throws(new EntityException());
            userIdentityServiceMock.Setup(u => u.GetUserId()).Returns(new int());
            groupServiceMock.Setup(u => u.GetMentorIdByGroup(It.IsAny<int>())).ReturnsAsync(new int());
            HttpResponseMessage response = await groupController.PutUsersToGroupAsync(5, new int[] { 1, 2, 3 });
            var expected = HttpStatusCode.InternalServerError;
            var actual = response.StatusCode;

            Assert.AreEqual(expected, actual);
        }
        #endregion
        #region PutPlansToGroup
        [Test]
        public async Task PutPlansToGroupShouldReturnOk()
        {
            groupServiceMock.Setup(u => u.AddPlansToGroupAsync(It.IsAny<int[]>(), It.IsAny<int>())).ReturnsAsync(true);
            userIdentityServiceMock.Setup(u => u.GetUserId()).Returns(new int());
            groupServiceMock.Setup(u => u.GetMentorIdByGroup(It.IsAny<int>())).ReturnsAsync(new int());
            HttpResponseMessage response = await groupController.PutPlansToGroupAsync(5, new int[] { 1, 2, 3 });
            var expected = HttpStatusCode.OK;
            var actual = response.StatusCode;

            Assert.AreEqual(expected, actual);
        }
        [Test]
        public async Task PutPlansToGroupShouldReturnUnauthorized()
        {
            groupServiceMock.Setup(u => u.AddPlansToGroupAsync(It.IsAny<int[]>(), It.IsAny<int>())).ReturnsAsync(true);
            userIdentityServiceMock.Setup(u => u.GetUserId()).Returns(new int());
            groupServiceMock.Setup(u => u.GetMentorIdByGroup(It.IsAny<int>())).ReturnsAsync(() => null);
            HttpResponseMessage response = await groupController.PutPlansToGroupAsync(5, new int[] { 1, 2, 3 });
            var expected = HttpStatusCode.Unauthorized;
            var actual = response.StatusCode;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task PutPlansToGroupShouldReturnBadRequest()
        {
            groupServiceMock.Setup(u => u.AddPlansToGroupAsync(It.IsAny<int[]>(), It.IsAny<int>())).ReturnsAsync(false);
            userIdentityServiceMock.Setup(u => u.GetUserId()).Returns(new int());
            groupServiceMock.Setup(u => u.GetMentorIdByGroup(It.IsAny<int>())).ReturnsAsync(new int());
            HttpResponseMessage response = await groupController.PutPlansToGroupAsync(5, new int[] { 1, 2, 3 });
            var expected = HttpStatusCode.BadRequest;
            var actual = response.StatusCode;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task PutPlansToGroupShouldReturnInternalServerError()
        {
            groupServiceMock.Setup(u => u.AddPlansToGroupAsync(It.IsAny<int[]>(), It.IsAny<int>())).Throws(new EntityException());
            userIdentityServiceMock.Setup(u => u.GetUserId()).Returns(new int());
            groupServiceMock.Setup(u => u.GetMentorIdByGroup(It.IsAny<int>())).ReturnsAsync(new int());
            HttpResponseMessage response = await groupController.PutPlansToGroupAsync(5, new int[] { 1, 2, 3 });
            var expected = HttpStatusCode.InternalServerError;
            var actual = response.StatusCode;

            Assert.AreEqual(expected, actual);
        }
        #endregion
        #region SearchPlansNotUsedInCurrentGroup
        [Test]
        public async Task SearchPlansNotUsedInCurrentGroupShouldReturnOk()
        {
            groupServiceMock.Setup(u => u.SearchPlansNotUsedInGroup(It.IsAny<string[]>(), It.IsAny<int>())).ReturnsAsync(new List<PlanDto>());
            var response = await groupController.SearchPlansNotUsedInCurrentGroup("search", 2);
            var expected = HttpStatusCode.OK;
            var actual = response.StatusCode;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task SearchPlansNotUsedInCurrentGroupShouldReturnNoContent()
        {
            groupServiceMock.Setup(u => u.SearchPlansNotUsedInGroup(It.IsAny<string[]>(), It.IsAny<int>())).ReturnsAsync(() => null);
            var response = await groupController.SearchPlansNotUsedInCurrentGroup("search", 2);
            var expected = HttpStatusCode.NoContent;
            var actual = response.StatusCode;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task SearchPlansNotUsedInCurrentGroupShouldReturnInternalServerError()
        {
            groupServiceMock.Setup(u => u.SearchPlansNotUsedInGroup(It.IsAny<string[]>(), It.IsAny<int>())).Throws(new EntityException());
            var response = await groupController.SearchPlansNotUsedInCurrentGroup("search", 2);
            var expected = HttpStatusCode.InternalServerError;
            var actual = response.StatusCode;

            Assert.AreEqual(expected, actual);
        }
        #endregion
        #region SearchUsersNotUsedInCurrentGroup
        [Test]
        public async Task SearchUsersNotUsedInCurrentGroupOk()
        {
            groupServiceMock.Setup(u => u.SearchUserNotInGroup(It.IsAny<string[]>(), It.IsAny<int>())).ReturnsAsync(new List<UserIdentityDto>());
            var response = await groupController.SearchPlansNotUsedInCurrentGroup("search", 2);
            var expected = HttpStatusCode.OK;
            var actual = response.StatusCode;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task SearchUsersNotUsedInCurrentGroupNoContent()
        {
            groupServiceMock.Setup(u => u.SearchUserNotInGroup(It.IsAny<string[]>(), It.IsAny<int>())).Returns(() => null);
            HttpResponseMessage response = await groupController.SearchUsersNotUsedInCurrentGroup("search", 2);
            var expected = HttpStatusCode.NoContent;
            var actual = response.StatusCode;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task SearchUsersNotUsedInCurrentGroupInternalServerError()
        {
            groupServiceMock.Setup(u => u.SearchUserNotInGroup(It.IsAny<string[]>(), It.IsAny<int>())).Throws(new EntityException());
            HttpResponseMessage response = await groupController.SearchUsersNotUsedInCurrentGroup("search", 2);
            var expected = HttpStatusCode.InternalServerError;
            var actual = response.StatusCode;

            Assert.AreEqual(expected, actual);
        }
        #endregion
        #region RemoveUserFromCurrentGroup
        [Test]
        public async Task RemoveUserFromCurrentGroupOk()
        {
            groupServiceMock.Setup(u => u.RemoveUserFromGroupAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(true);
            userIdentityServiceMock.Setup(u => u.GetUserId()).Returns(new int());
            groupServiceMock.Setup(u => u.GetMentorIdByGroupAsync(It.IsAny<int>())).ReturnsAsync(new int());
            HttpResponseMessage response = await groupController.RemoveUserFromCurrentGroupAsync(1, 2);
            var expected = HttpStatusCode.OK;
            var actual = response.StatusCode;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task RemoveUserFromCurrentGroupUnauthorized()
        {
            groupServiceMock.Setup(u => u.RemoveUserFromGroupAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(true);
            userIdentityServiceMock.Setup(u => u.GetUserId()).Returns(new int());
            groupServiceMock.Setup(u => u.GetMentorIdByGroup(It.IsAny<int>())).ReturnsAsync(() => null);
            HttpResponseMessage response = await groupController.RemoveUserFromCurrentGroupAsync(1, 2);
            var expected = HttpStatusCode.Unauthorized;
            var actual = response.StatusCode;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task RemoveUserFromCurrentGroupNoContent()
        {
            groupServiceMock.Setup(u => u.RemoveUserFromGroupAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(false);
            userIdentityServiceMock.Setup(u => u.GetUserId()).Returns(new int());
            groupServiceMock.Setup(u => u.GetMentorIdByGroup(It.IsAny<int>())).ReturnsAsync(new int());
            HttpResponseMessage response = await groupController.RemoveUserFromCurrentGroupAsync(1, 2);
            var expected = HttpStatusCode.NoContent;
            var actual = response.StatusCode;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task RemoveUserFromCurrentGroupInternalServerError()
        {
            groupServiceMock.Setup(u => u.RemoveUserFromGroupAsync(It.IsAny<int>(), It.IsAny<int>())).Throws(new EntityException());
            userIdentityServiceMock.Setup(u => u.GetUserId()).Returns(new int());
            groupServiceMock.Setup(u => u.GetMentorIdByGroup(It.IsAny<int>())).ReturnsAsync(new int());
            HttpResponseMessage response = await groupController.RemoveUserFromCurrentGroupAsync(1, 2);
            var expected = HttpStatusCode.InternalServerError;
            var actual = response.StatusCode;

            Assert.AreEqual(expected, actual);
        }
        #endregion
        #region RemovePlanFromCurrentGroup
        [Test]
        public async Task RemovePlanFromCurrentGroupOk()
        {
            groupServiceMock.Setup(u => u.RemovePlanFromGroupAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(true);
            HttpResponseMessage response = await groupController.RemovePlanFromCurrentGroupAsync(1, 2);
            var expected = HttpStatusCode.OK;
            var actual = response.StatusCode;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task RemovePlanFromCurrentGroupBadRequest()
        {
            groupServiceMock.Setup(u => u.RemovePlanFromGroupAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(false);
            HttpResponseMessage response = await groupController.RemovePlanFromCurrentGroupAsync(1, 2);
            var expected = HttpStatusCode.BadRequest;
            var actual = response.StatusCode;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task RemovePlanFromCurrentGroupInternalServerError()
        {
            groupServiceMock.Setup(u => u.RemovePlanFromGroupAsync(It.IsAny<int>(), It.IsAny<int>())).Throws(new EntityException());
            HttpResponseMessage response = await groupController.RemovePlanFromCurrentGroupAsync(1, 2);
            var expected = HttpStatusCode.InternalServerError;
            var actual = response.StatusCode;

            Assert.AreEqual(expected, actual);
        }
        #endregion
        #region GetUserGroups
        [Test]
        public async Task GetUserGroupsOk()
        {
            userIdentityServiceMock.Setup(u => u.GetUserId()).Returns(new int());
            userServiceMock.Setup(u => u.ContainsId(It.IsAny<int>())).ReturnsAsync(true);
            groupServiceMock.Setup(u => u.GetUserGroupsAsync(It.IsAny<int>())).ReturnsAsync(new List<GroupDto>());
            groupServiceMock.Setup(u => u.GroupsCountAsync()).ReturnsAsync(1);
            HttpResponseMessage response = await groupController.GetUserGroupsAsync();
            var expected = HttpStatusCode.OK;
            var actual = response.StatusCode;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task GetUserGroupsNoUsers()
        {
            userIdentityServiceMock.Setup(u => u.GetUserId()).Returns(new int());
            userServiceMock.Setup(u => u.ContainsId(It.IsAny<int>())).ReturnsAsync(false);
            groupServiceMock.Setup(u => u.GetUserGroupsAsync(It.IsAny<int>())).ReturnsAsync(new List<GroupDto>());
            groupServiceMock.Setup(u => u.GroupsCountAsync()).ReturnsAsync(1);
            HttpResponseMessage response = await groupController.GetUserGroupsAsync();
            var expected = HttpStatusCode.NoContent;
            var actual = response.StatusCode;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task GetUserGroupsNoGroups()
        {
            userIdentityServiceMock.Setup(u => u.GetUserId()).Returns(new int());
            userServiceMock.Setup(u => u.ContainsId(It.IsAny<int>())).ReturnsAsync(true);
            groupServiceMock.Setup(u => u.GetUserGroupsAsync(It.IsAny<int>())).ReturnsAsync(new List<GroupDto>());
            groupServiceMock.Setup(u => u.GroupsCountAsync()).ReturnsAsync(0);
            HttpResponseMessage response = await groupController.GetUserGroupsAsync();
            var expected = HttpStatusCode.NoContent;
            var actual = response.StatusCode;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task GetUserGroupsBadRequest()
        {
            userIdentityServiceMock.Setup(u => u.GetUserId()).Returns(new int());
            userServiceMock.Setup(u => u.ContainsId(It.IsAny<int>())).ReturnsAsync(true);
            groupServiceMock.Setup(u => u.GetUserGroupsAsync(It.IsAny<int>())).ReturnsAsync(() => null);
            groupServiceMock.Setup(u => u.GroupsCountAsync()).ReturnsAsync(1);
            HttpResponseMessage response = await groupController.GetUserGroupsAsync();
            var expected = HttpStatusCode.BadRequest;
            var actual = response.StatusCode;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task GetUserGroupsInternalServerError()
        {
            userIdentityServiceMock.Setup(u => u.GetUserId()).Throws(new EntityException());
            userServiceMock.Setup(u => u.ContainsId(It.IsAny<int>())).ReturnsAsync(true);
            groupServiceMock.Setup(u => u.GetUserGroupsAsync(It.IsAny<int>())).ReturnsAsync(new List<GroupDto>());
            groupServiceMock.Setup(u => u.GroupsCountAsync()).ReturnsAsync(1);
            HttpResponseMessage response = await groupController.GetUserGroupsAsync();
            var expected = HttpStatusCode.InternalServerError;
            var actual = response.StatusCode;

            Assert.AreEqual(expected, actual);
        }
        #endregion
    }
}
