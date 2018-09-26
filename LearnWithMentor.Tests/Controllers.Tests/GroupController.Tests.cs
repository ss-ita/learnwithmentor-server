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
            groupServiceMock.Setup(u =>  u.GetGroupById(It.IsAny<int>())).ReturnsAsync(
                 (int i) => groups.Single(x => x.Id == i));
            var response = await groupController.GetById(3);
            var successfull =  response.TryGetContentValue<GroupDto>(out var groupDTO);
            var expected = groupServiceMock.Object.GetGroupById(3);
            var actual = groupDTO;

            Assert.IsTrue(successfull);
            Assert.AreEqual(expected.Id, actual.Id);
            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);
        }

        [Test]
        public async  Task GetGroupByIdTest_ShouldReturnNotFoundResponse()
        {
            groupServiceMock.Setup(u => u.GetGroupById(It.IsAny<int>()));

            var response = await groupController.GetById(1);
            var expected = HttpStatusCode.NotFound;
            var actual = response.StatusCode;

            Assert.AreEqual(expected, actual);
        }
        #endregion
        #region GetGroupByMentor
        [Test]
        public void GetGroupByMentor_ShouldReturnGroupsForMentor()
        {
            groupServiceMock.Setup(g => g.GetGroupsByMentor(It.IsAny<int>())).Returns(new List<GroupDto>());

            var response = groupController.GetByMentor(2);
            var successfull = response.TryGetContentValue<List<GroupDto>>(out var groupDTOs);
            var expected = groupServiceMock.Object.GetGroupsByMentor(2);
            var actual = groupDTOs;

            Assert.IsTrue(successfull);
            Assert.AreEqual(expected, actual);
            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);
        }

        [Test]
        public void GetGroupByMentor_ShouldReturnNotFoundResponse()
        {
            groupServiceMock.Setup(u => u.GetGroupsByMentor(It.IsAny<int>()));

            var response = groupController.GetByMentor(6);
            var expected = HttpStatusCode.NotFound;
            var actual = response.StatusCode;
            Assert.AreEqual(expected, actual);
        }
        #endregion
        #region GetAllPlansByGroupId
        [Test]
        public async Task GetAllPlansByGroupId_ShouldReturnOk()
        {
            groupServiceMock.Setup(p => p.GetPlans(It.IsAny<int>())).ReturnsAsync(new List<PlanDto>());

            var response = await groupController.GetPlans(4);
            var expected = HttpStatusCode.OK;
            var actual = response.StatusCode;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task GetAllPlansByGroupId_ShouldReturnNoContent()
        {
            groupServiceMock.Setup(p => p.GetPlans(It.IsAny<int>())).ReturnsAsync(() => Enumerable.Empty<PlanDto>());

            var response = await groupController.GetPlans(99);
            var expected = HttpStatusCode.NoContent;
            var actual = response.StatusCode;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task GetAllPlansByGroupId_ShouldReturInternalServerError()
        {
            groupServiceMock.Setup(p => p.GetPlans(It.IsAny<int>())).Throws(new EntityException());

            var response = await groupController.GetPlans(99);
            var expected = HttpStatusCode.InternalServerError;
            var actual = response.StatusCode;

            Assert.AreEqual(expected, actual);
        }
        #endregion
        #region GetUsersWithImage
        [Test]
        public async Task GetUsersWithImage_ShouldReturnOk()
        {
            groupServiceMock.Setup(p => p.GetUsersWithImage(It.IsAny<int>())).ReturnsAsync(new List<UserWithImageDto>());

            HttpResponseMessage response = await groupController.GetUsersWithImage(4);
            var expected = HttpStatusCode.OK;
            var actual = response.StatusCode;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task GetUsersWithImage_ShouldReturnNoContent()
        {
            groupServiceMock.Setup(p => p.GetUsersWithImage(It.IsAny<int>())).ReturnsAsync(() => null);

            HttpResponseMessage response = await groupController.GetUsersWithImage(4);
            var expected = HttpStatusCode.NoContent;
            var actual = response.StatusCode;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task GetUsersWithImage_ShouldReturnInternalServerError()
        {
            groupServiceMock.Setup(p => p.GetUsersWithImage(It.IsAny<int>())).Throws(new EntityException());

            HttpResponseMessage response = await groupController.GetUsersWithImage(99);
            var expected = HttpStatusCode.InternalServerError;
            var actual = response.StatusCode;

            Assert.AreEqual(expected, actual);
        }
        #endregion
        #region GetUsersNotInCurrentGroup
        [Test]
        public void GetUsersNotInCurrentGroupShouldReturnOk()
        {
            groupServiceMock.Setup(u => u.GetUsersNotInGroup(It.IsAny<int>())).Returns(new List<UserIdentityDto>());
            var response = groupController.GetUsersNotInCurrentGroup(4);
            var expected = HttpStatusCode.OK;
            var actual = response.StatusCode;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void GetUsersNotInCurrentGroupShouldReturnNotFound()
        {
            groupServiceMock.Setup(u => u.GetUsersNotInGroup(It.IsAny<int>())).Returns(() => null);
            var response = groupController.GetUsersNotInCurrentGroup(4);
            var expected = HttpStatusCode.NotFound;
            var actual = response.StatusCode;

            Assert.AreEqual(expected, actual);
        }
        #endregion
        #region GetPlansNotUsedInCurrentGroup
        [Test]
        public async Task GetPlansNotUsedInCurrentGroupShouldReturnOk()
        {
            groupServiceMock.Setup(u => u.GetPlansNotUsedInGroup(It.IsAny<int>())).ReturnsAsync(new List<PlanDto>());
            var response = await groupController.GetPlansNotUsedInCurrentGroup(4);
            var expected = HttpStatusCode.OK;
            var actual = response.StatusCode;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task GetPlansNotUsedInCurrentGroupShouldReturnNoContent()
        {
            groupServiceMock.Setup(u => u.GetPlansNotUsedInGroup(It.IsAny<int>())).ReturnsAsync(() => null);
            var response = await groupController.GetPlansNotUsedInCurrentGroup(4);
            var expected = HttpStatusCode.NoContent;
            var actual = response.StatusCode;

            Assert.AreEqual(expected, actual);
        }
        #endregion
        #region AddNewGroup
        [Test]
        public void AddNewGroupShouldReturnOk()
        {
            groupServiceMock.Setup(u => u.AddGroup(It.IsAny<GroupDto>())).Returns(true);
            var response = groupController.Post(groups[0]);
            var expected = HttpStatusCode.OK;
            var actual = response.StatusCode;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void AddNewGroupShouldReturnBadRequestValid()
        {
            groupServiceMock.Setup(u => u.AddGroup(It.IsAny<GroupDto>())).Returns(false);
            var response = groupController.Post(groups[0]);
            var expected = HttpStatusCode.BadRequest;
            var actual = response.StatusCode;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void AddNewGroupShouldReturnBadRequestInValid()
        {
            groupServiceMock.Setup(u => u.AddGroup(It.IsAny<GroupDto>())).Returns(true);
            var newGroup = new GroupDto { };
            ValidateViewModel<GroupDto, GroupController>(groupController, newGroup);
            var response = groupController.Post(groups[0]);
            var expected = HttpStatusCode.BadRequest;
            var actual = response.StatusCode;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void AddNewGroupShouldReturnInternalServerError()
        {
            groupServiceMock.Setup(u => u.AddGroup(It.IsAny<GroupDto>())).Throws(new EntityException());
            var response = groupController.Post(groups[0]);
            var expected = HttpStatusCode.InternalServerError;
            var actual = response.StatusCode;

            Assert.AreEqual(expected, actual);
        }
        #endregion
        #region PutUsersToGroup
        [Test]
        public async Task PutUsersToGroupShouldReturnOk()
        {
            groupServiceMock.Setup(u => u.AddUsersToGroup(It.IsAny<int[]>(), It.IsAny<int>())).ReturnsAsync(true);
            userIdentityServiceMock.Setup(u => u.GetUserId()).Returns(new int());
            groupServiceMock.Setup(u => u.GetMentorIdByGroup(It.IsAny<int>())).Returns(new int());
            HttpResponseMessage response = await groupController.PutUsersToGroup(5, new int[] { 1, 2, 3 });
            var expected = HttpStatusCode.OK;
            var actual = response.StatusCode;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task PutUsersToGroupShouldReturnUnauthorized()
        {
            groupServiceMock.Setup(u => u.AddUsersToGroup(It.IsAny<int[]>(), It.IsAny<int>())).ReturnsAsync(true);
            userIdentityServiceMock.Setup(u => u.GetUserId()).Returns(new int());
            groupServiceMock.Setup(u => u.GetMentorIdByGroup(It.IsAny<int>())).Returns(() => null);
            HttpResponseMessage response = await groupController.PutUsersToGroup(5, new int[] { 1, 2, 3 });
            var expected = HttpStatusCode.Unauthorized;
            var actual = response.StatusCode;

            Assert.AreEqual(expected, actual);
        }
        [Test]
        public async Task PutUsersToGroupShouldReturnBadRequest()
        {
            groupServiceMock.Setup(u => u.AddUsersToGroup(It.IsAny<int[]>(), It.IsAny<int>())).ReturnsAsync(false);
            userIdentityServiceMock.Setup(u => u.GetUserId()).Returns(new int());
            groupServiceMock.Setup(u => u.GetMentorIdByGroup(It.IsAny<int>())).Returns(new int());
            HttpResponseMessage response = await groupController.PutUsersToGroup(5, new int[] { 1, 2, 3 });
            var expected = HttpStatusCode.BadRequest;
            var actual =  response.StatusCode;

            Assert.AreEqual(expected, actual);
        }
        [Test]
        public async Task PutUsersToGroupShouldReturnInternalServerError()
        {
            groupServiceMock.Setup(u => u.AddUsersToGroup(It.IsAny<int[]>(), It.IsAny<int>())).Throws(new EntityException());
            userIdentityServiceMock.Setup(u => u.GetUserId()).Returns(new int());
            groupServiceMock.Setup(u => u.GetMentorIdByGroup(It.IsAny<int>())).Returns(new int());
            HttpResponseMessage response = await groupController.PutUsersToGroup(5, new int[] { 1, 2, 3 });
            var expected = HttpStatusCode.InternalServerError;
            var actual = response.StatusCode;

            Assert.AreEqual(expected, actual);
        }
        #endregion
        #region PutPlansToGroup
        [Test]
        public async Task PutPlansToGroupShouldReturnOk()
        {
            groupServiceMock.Setup(u => u.AddPlansToGroup(It.IsAny<int[]>(), It.IsAny<int>())).ReturnsAsync(true);
            userIdentityServiceMock.Setup(u => u.GetUserId()).Returns(new int());
            groupServiceMock.Setup(u => u.GetMentorIdByGroup(It.IsAny<int>())).Returns(new int());
            var response = await groupController.PutPlansToGroup(5, new int[] { 1, 2, 3 });
            var expected = HttpStatusCode.OK;
            var actual = response.StatusCode;

            Assert.AreEqual(expected, actual);
        }
        [Test]
        public async Task PutPlansToGroupShouldReturnUnauthorized()
        {
            groupServiceMock.Setup(u => u.AddPlansToGroup(It.IsAny<int[]>(), It.IsAny<int>())).ReturnsAsync(true);
            userIdentityServiceMock.Setup(u => u.GetUserId()).Returns(new int());
            groupServiceMock.Setup(u => u.GetMentorIdByGroup(It.IsAny<int>())).Returns(() => null);
            var response = await groupController.PutPlansToGroup(5, new int[] { 1, 2, 3 });
            var expected = HttpStatusCode.Unauthorized;
            var actual = response.StatusCode;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task PutPlansToGroupShouldReturnBadRequest()
        {
            groupServiceMock.Setup(u => u.AddPlansToGroup(It.IsAny<int[]>(), It.IsAny<int>())).ReturnsAsync(false);
            userIdentityServiceMock.Setup(u => u.GetUserId()).Returns(new int());
            groupServiceMock.Setup(u => u.GetMentorIdByGroup(It.IsAny<int>())).Returns(new int());
            var response = await groupController.PutPlansToGroup(5, new int[] { 1, 2, 3 });
            var expected = HttpStatusCode.BadRequest;
            var actual = response.StatusCode;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task PutPlansToGroupShouldReturnInternalServerError()
        {
            groupServiceMock.Setup(u => u.AddPlansToGroup(It.IsAny<int[]>(), It.IsAny<int>())).Throws(new EntityException());
            userIdentityServiceMock.Setup(u => u.GetUserId()).Returns(new int());
            groupServiceMock.Setup(u => u.GetMentorIdByGroup(It.IsAny<int>())).Returns(new int());
            var response = await groupController.PutPlansToGroup(5, new int[] { 1, 2, 3 });
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
            groupServiceMock.Setup(u => u.SearchUserNotInGroup(It.IsAny<string[]>(), It.IsAny<int>())).Returns(new List<UserIdentityDto>());
            var response = await groupController.SearchPlansNotUsedInCurrentGroup("search", 2);
            var expected = HttpStatusCode.OK;
            var actual = response.StatusCode;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void SearchUsersNotUsedInCurrentGroupNoContent()
        {
            groupServiceMock.Setup(u => u.SearchUserNotInGroup(It.IsAny<string[]>(), It.IsAny<int>())).Returns(() => null);
            var response = groupController.SearchUsersNotUsedInCurrentGroup("search", 2);
            var expected = HttpStatusCode.NoContent;
            var actual = response.StatusCode;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void SearchUsersNotUsedInCurrentGroupInternalServerError()
        {
            groupServiceMock.Setup(u => u.SearchUserNotInGroup(It.IsAny<string[]>(), It.IsAny<int>())).Throws(new EntityException());
            var response = groupController.SearchUsersNotUsedInCurrentGroup("search", 2);
            var expected = HttpStatusCode.InternalServerError;
            var actual = response.StatusCode;

            Assert.AreEqual(expected, actual);
        }
        #endregion
        #region RemoveUserFromCurrentGroup
        [Test]
        public async Task RemoveUserFromCurrentGroupOk()
        {
            groupServiceMock.Setup(u => u.RemoveUserFromGroup(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(true);
            userIdentityServiceMock.Setup(u => u.GetUserId()).Returns(new int());
            groupServiceMock.Setup(u => u.GetMentorIdByGroup(It.IsAny<int>())).Returns(new int());
            HttpResponseMessage response = await groupController.RemoveUserFromCurrentGroup(1, 2);
            var expected = HttpStatusCode.OK;
            var actual = response.StatusCode;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task RemoveUserFromCurrentGroupUnauthorized()
        {
            groupServiceMock.Setup(u => u.RemoveUserFromGroup(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(true);
            userIdentityServiceMock.Setup(u => u.GetUserId()).Returns(new int());
            groupServiceMock.Setup(u => u.GetMentorIdByGroup(It.IsAny<int>())).Returns(() => null);
            HttpResponseMessage response = await groupController.RemoveUserFromCurrentGroup(1, 2);
            var expected = HttpStatusCode.Unauthorized;
            var actual = response.StatusCode;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task RemoveUserFromCurrentGroupNoContent()
        {
            groupServiceMock.Setup(u => u.RemoveUserFromGroup(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(false);
            userIdentityServiceMock.Setup(u => u.GetUserId()).Returns(new int());
            groupServiceMock.Setup(u => u.GetMentorIdByGroup(It.IsAny<int>())).Returns(new int());
            HttpResponseMessage response = await groupController.RemoveUserFromCurrentGroup(1, 2);
            var expected = HttpStatusCode.NoContent;
            var actual = response.StatusCode;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task RemoveUserFromCurrentGroupInternalServerError()
        {
            groupServiceMock.Setup(u => u.RemoveUserFromGroup(It.IsAny<int>(), It.IsAny<int>())).Throws(new EntityException());
            userIdentityServiceMock.Setup(u => u.GetUserId()).Returns(new int());
            groupServiceMock.Setup(u => u.GetMentorIdByGroup(It.IsAny<int>())).Returns(new int());
            HttpResponseMessage response = await groupController.RemoveUserFromCurrentGroup(1, 2);
            var expected = HttpStatusCode.InternalServerError;
            var actual = response.StatusCode;

            Assert.AreEqual(expected, actual);
        }
        #endregion
        #region RemovePlanFromCurrentGroup
        [Test]
        public async Task RemovePlanFromCurrentGroupOk()
        {
            groupServiceMock.Setup(u => u.RemovePlanFromGroup(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(true);
            var response = await groupController.RemovePlanFromCurrentGroup(1, 2);
            var expected = HttpStatusCode.OK;
            var actual = response.StatusCode;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task RemovePlanFromCurrentGroupBadRequest()
        {
            groupServiceMock.Setup(u => u.RemovePlanFromGroup(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(false);
            var response = await groupController.RemovePlanFromCurrentGroup(1, 2);
            var expected = HttpStatusCode.BadRequest;
            var actual = response.StatusCode;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task RemovePlanFromCurrentGroupInternalServerError()
        {
            groupServiceMock.Setup(u => u.RemovePlanFromGroup(It.IsAny<int>(), It.IsAny<int>())).Throws(new EntityException());
            var response = await groupController.RemovePlanFromCurrentGroup(1, 2);
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
            userServiceMock.Setup(u => u.ContainsId(It.IsAny<int>())).Returns(true);
            groupServiceMock.Setup(u => u.GetUserGroups(It.IsAny<int>())).ReturnsAsync(new List<GroupDto>());
            groupServiceMock.Setup(u => u.GroupsCount()).Returns(1);
            HttpResponseMessage response = await groupController.GetUserGroups();
            var expected = HttpStatusCode.OK;
            var actual = response.StatusCode;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task GetUserGroupsNoUsers()
        {
            userIdentityServiceMock.Setup(u => u.GetUserId()).Returns(new int());
            userServiceMock.Setup(u => u.ContainsId(It.IsAny<int>())).Returns(false);
            groupServiceMock.Setup(u => u.GetUserGroups(It.IsAny<int>())).ReturnsAsync(new List<GroupDto>());
            groupServiceMock.Setup(u => u.GroupsCount()).Returns(1);
            HttpResponseMessage response = await groupController.GetUserGroups();
            var expected = HttpStatusCode.NoContent;
            var actual = response.StatusCode;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task GetUserGroupsNoGroups()
        {
            userIdentityServiceMock.Setup(u => u.GetUserId()).Returns(new int());
            userServiceMock.Setup(u => u.ContainsId(It.IsAny<int>())).Returns(true);
            groupServiceMock.Setup(u => u.GetUserGroups(It.IsAny<int>())).ReturnsAsync(new List<GroupDto>());
            groupServiceMock.Setup(u => u.GroupsCount()).Returns(0);
            HttpResponseMessage response = await groupController.GetUserGroups();
            var expected = HttpStatusCode.NoContent;
            var actual = response.StatusCode;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task GetUserGroupsBadRequest()
        {
            userIdentityServiceMock.Setup(u => u.GetUserId()).Returns(new int());
            userServiceMock.Setup(u => u.ContainsId(It.IsAny<int>())).Returns(true);
            groupServiceMock.Setup(u => u.GetUserGroups(It.IsAny<int>())).ReturnsAsync(() => null);
            groupServiceMock.Setup(u => u.GroupsCount()).Returns(1);
            HttpResponseMessage response = await groupController.GetUserGroups();
            var expected = HttpStatusCode.BadRequest;
            var actual = response.StatusCode;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task GetUserGroupsInternalServerError()
        {
            userIdentityServiceMock.Setup(u => u.GetUserId()).Throws(new EntityException());
            userServiceMock.Setup(u => u.ContainsId(It.IsAny<int>())).Returns(true);
            groupServiceMock.Setup(u => u.GetUserGroups(It.IsAny<int>())).ReturnsAsync(new List<GroupDto>());
            groupServiceMock.Setup(u => u.GroupsCount()).Returns(1);
            HttpResponseMessage response = await groupController.GetUserGroups();
            var expected = HttpStatusCode.InternalServerError;
            var actual = response.StatusCode;

            Assert.AreEqual(expected, actual);
        }
        #endregion
    }
}
