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


namespace LearnWithMentor.Tests.Controllers.Tests
{
    [TestFixture]
    public class GroupControllerTests
    {
        private GroupController groupController;
        private Mock<IGroupService> groupServiceMock;
        private List<GroupDTO> groups;
        private Mock<ITraceWriter> traceWriterMock;
        private Mock<IUserService> userServiceMock;
        private Mock<IUserIdentityService> userIdentityServiceMock;


        [SetUp]
        public void SetUp()
        {
            groups = new List<GroupDTO>()
            {
                new GroupDTO(1, "name1", 1, "mentorName1"),
                new GroupDTO(2, "name2", 2, "mentorName2"),
                new GroupDTO(3, "name3", 3, "mentorName3"),
                new GroupDTO(4, "name4", 4, "mentorName4")
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
        public void GetGroupByIdTest_ShouldReturnTask()
        {
            groupServiceMock.Setup(u => u.GetGroupById(It.IsAny<int>())).Returns(
                 (int i) => groups.Where(x => x.Id == i).Single());

            var response = groupController.GetById(3);
            var successfull = response.TryGetContentValue<GroupDTO>(out var groupDTO);
            var expected = groupServiceMock.Object.GetGroupById(3);
            var actual = groupDTO;

            Assert.IsTrue(successfull);
            Assert.AreEqual(expected.Id, actual.Id);
            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);
        }

        [Test]
        public void GetGroupByIdTest_ShouldReturnNotFoundResponse()
        {
            groupServiceMock.Setup(u => u.GetGroupById(It.IsAny<int>()));

            var response = groupController.GetById(1);
            var expected = HttpStatusCode.NotFound;
            var actual = response.StatusCode;

            Assert.AreEqual(expected, actual);
        }
        #endregion
        #region GetGroupByMentor
        [Test]
        public void GetGroupByMentor_ShouldReturnGroupsForMentor()
        {
            groupServiceMock.Setup(g => g.GetGroupsByMentor(It.IsAny<int>())).Returns(new List<GroupDTO>());

            var response = groupController.GetByMentor(2);
            var successfull = response.TryGetContentValue<List<GroupDTO>>(out var groupDTOs);
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
        public void GetAllPlansByGroupId_ShouldReturnOk()
        {
            groupServiceMock.Setup(p => p.GetPlans(It.IsAny<int>())).Returns(new List<PlanDTO>());

            var response = groupController.GetPlans(4);
            var expected = HttpStatusCode.OK;
            var actual = response.StatusCode;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void GetAllPlansByGroupId_ShouldReturnNoContent()
        {
            groupServiceMock.Setup(p => p.GetPlans(It.IsAny<int>())).Returns(() => null);

            var response = groupController.GetPlans(99);
            var expected = HttpStatusCode.NoContent;
            var actual = response.StatusCode;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void GetAllPlansByGroupId_ShouldReturInternalServerError()
        {
            groupServiceMock.Setup(p => p.GetPlans(It.IsAny<int>())).Throws(new EntityException());

            var response = groupController.GetPlans(99);
            var expected = HttpStatusCode.InternalServerError;
            var actual = response.StatusCode;

            Assert.AreEqual(expected, actual);
        }
        #endregion
        #region GetUsersWithImage
        [Test]
        public void GetUsersWithImage_ShouldReturnOk()
        {
            groupServiceMock.Setup(p => p.GetUsersWithImage(It.IsAny<int>())).Returns(new List<UserWithImageDTO>());

            var response = groupController.GetUsersWithImage(4);
            var expected = HttpStatusCode.OK;
            var actual = response.StatusCode;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void GetUsersWithImage_ShouldReturnNoContent()
        {
            groupServiceMock.Setup(p => p.GetUsersWithImage(It.IsAny<int>())).Returns(() => null);

            var response = groupController.GetUsersWithImage(4);
            var expected = HttpStatusCode.NoContent;
            var actual = response.StatusCode;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void GetUsersWithImage_ShouldReturnInternalServerError()
        {
            groupServiceMock.Setup(p => p.GetUsersWithImage(It.IsAny<int>())).Throws(new EntityException());

            var response = groupController.GetUsersWithImage(99);
            var expected = HttpStatusCode.InternalServerError;
            var actual = response.StatusCode;

            Assert.AreEqual(expected, actual);
        }
        #endregion
        #region GetUsersNotInCurrentGroup
        [Test]
        public void GetUsersNotInCurrentGroupShouldReturnOk()
        {
            groupServiceMock.Setup(u => u.GetUsersNotInGroup(It.IsAny<int>())).Returns(new List<UserIdentityDTO>());
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
        public void GetPlansNotUsedInCurrentGroupShouldReturnOk()
        {
            groupServiceMock.Setup(u => u.GetPlansNotUsedInGroup(It.IsAny<int>())).Returns(new List<PlanDTO>());
            var response = groupController.GetPlansNotUsedInCurrentGroup(4);
            var expected = HttpStatusCode.OK;
            var actual = response.StatusCode;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void GetPlansNotUsedInCurrentGroupShouldReturnNoContent()
        {
            groupServiceMock.Setup(u => u.GetPlansNotUsedInGroup(It.IsAny<int>())).Returns(() => null);
            var response = groupController.GetPlansNotUsedInCurrentGroup(4);
            var expected = HttpStatusCode.NoContent;
            var actual = response.StatusCode;

            Assert.AreEqual(expected, actual);
        }
        #endregion
        #region AddNewGroup
        [Test]
        public void AddNewGroupShouldReturnOk()
        {
            groupServiceMock.Setup(u => u.AddGroup(It.IsAny<GroupDTO>())).Returns(true);
            var response = groupController.Post(groups[0]);
            var expected = HttpStatusCode.OK;
            var actual = response.StatusCode;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void AddNewGroupShouldReturnBadRequestValid()
        {
            groupServiceMock.Setup(u => u.AddGroup(It.IsAny<GroupDTO>())).Returns(false);
            var response = groupController.Post(groups[0]);
            var expected = HttpStatusCode.BadRequest;
            var actual = response.StatusCode;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void AddNewGroupShouldReturnBadRequestInValid()
        {
            groupServiceMock.Setup(u => u.AddGroup(It.IsAny<GroupDTO>())).Returns(true);
            var newGroup = new GroupDTO { };
            ValidateViewModel<GroupDTO, GroupController>(groupController, newGroup);
            var response = groupController.Post(groups[0]);
            var expected = HttpStatusCode.BadRequest;
            var actual = response.StatusCode;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void AddNewGroupShouldReturnInternalServerError()
        {
            groupServiceMock.Setup(u => u.AddGroup(It.IsAny<GroupDTO>())).Throws(new EntityException());
            var response = groupController.Post(groups[0]);
            var expected = HttpStatusCode.InternalServerError;
            var actual = response.StatusCode;

            Assert.AreEqual(expected, actual);
        }
        #endregion
        #region PutUsersToGroup
        [Test]
        public void PutUsersToGroupShouldReturnOk()
        {
            groupServiceMock.Setup(u => u.AddUsersToGroup(It.IsAny<int[]>(), It.IsAny<int>())).Returns(true);
            userIdentityServiceMock.Setup(u => u.GetUserId()).Returns(new int());
            groupServiceMock.Setup(u => u.GetMentorIdByGroup(It.IsAny<int>())).Returns(new int());
            var response = groupController.PutUsersToGroup(5, new int[] { 1, 2, 3 });
            var expected = HttpStatusCode.OK;
            var actual = response.StatusCode;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void PutUsersToGroupShouldReturnUnauthorized()
        {
            groupServiceMock.Setup(u => u.AddUsersToGroup(It.IsAny<int[]>(), It.IsAny<int>())).Returns(true);
            userIdentityServiceMock.Setup(u => u.GetUserId()).Returns(new int());
            groupServiceMock.Setup(u => u.GetMentorIdByGroup(It.IsAny<int>())).Returns(() => null);
            var response = groupController.PutUsersToGroup(5, new int[] { 1, 2, 3 });
            var expected = HttpStatusCode.Unauthorized;
            var actual = response.StatusCode;

            Assert.AreEqual(expected, actual);
        }
        [Test]
        public void PutUsersToGroupShouldReturnBadRequest()
        {
            groupServiceMock.Setup(u => u.AddUsersToGroup(It.IsAny<int[]>(), It.IsAny<int>())).Returns(false);
            userIdentityServiceMock.Setup(u => u.GetUserId()).Returns(new int());
            groupServiceMock.Setup(u => u.GetMentorIdByGroup(It.IsAny<int>())).Returns(new int());
            var response = groupController.PutUsersToGroup(5, new int[] { 1, 2, 3 });
            var expected = HttpStatusCode.BadRequest;
            var actual = response.StatusCode;

            Assert.AreEqual(expected, actual);
        }
        [Test]
        public void PutUsersToGroupShouldReturnInternalServerError()
        {
            groupServiceMock.Setup(u => u.AddUsersToGroup(It.IsAny<int[]>(), It.IsAny<int>())).Throws(new EntityException());
            userIdentityServiceMock.Setup(u => u.GetUserId()).Returns(new int());
            groupServiceMock.Setup(u => u.GetMentorIdByGroup(It.IsAny<int>())).Returns(new int());
            var response = groupController.PutUsersToGroup(5, new int[] { 1, 2, 3 });
            var expected = HttpStatusCode.InternalServerError;
            var actual = response.StatusCode;

            Assert.AreEqual(expected, actual);
        }
        #endregion
        #region PutPlansToGroup
        [Test]
        public void PutPlansToGroupShouldReturnOk()
        {
            groupServiceMock.Setup(u => u.AddPlansToGroup(It.IsAny<int[]>(), It.IsAny<int>())).Returns(true);
            userIdentityServiceMock.Setup(u => u.GetUserId()).Returns(new int());
            groupServiceMock.Setup(u => u.GetMentorIdByGroup(It.IsAny<int>())).Returns(new int());
            var response = groupController.PutPlansToGroup(5, new int[] { 1, 2, 3 });
            var expected = HttpStatusCode.OK;
            var actual = response.StatusCode;

            Assert.AreEqual(expected, actual);
        }
        [Test]
        public void PutPlansToGroupShouldReturnUnauthorized()
        {
            groupServiceMock.Setup(u => u.AddPlansToGroup(It.IsAny<int[]>(), It.IsAny<int>())).Returns(true);
            userIdentityServiceMock.Setup(u => u.GetUserId()).Returns(new int());
            groupServiceMock.Setup(u => u.GetMentorIdByGroup(It.IsAny<int>())).Returns(() => null);
            var response = groupController.PutPlansToGroup(5, new int[] { 1, 2, 3 });
            var expected = HttpStatusCode.Unauthorized;
            var actual = response.StatusCode;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void PutPlansToGroupShouldReturnBadRequest()
        {
            groupServiceMock.Setup(u => u.AddPlansToGroup(It.IsAny<int[]>(), It.IsAny<int>())).Returns(false);
            userIdentityServiceMock.Setup(u => u.GetUserId()).Returns(new int());
            groupServiceMock.Setup(u => u.GetMentorIdByGroup(It.IsAny<int>())).Returns(new int());
            var response = groupController.PutPlansToGroup(5, new int[] { 1, 2, 3 });
            var expected = HttpStatusCode.BadRequest;
            var actual = response.StatusCode;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void PutPlansToGroupShouldReturnInternalServerError()
        {
            groupServiceMock.Setup(u => u.AddPlansToGroup(It.IsAny<int[]>(), It.IsAny<int>())).Throws(new EntityException());
            userIdentityServiceMock.Setup(u => u.GetUserId()).Returns(new int());
            groupServiceMock.Setup(u => u.GetMentorIdByGroup(It.IsAny<int>())).Returns(new int());
            var response = groupController.PutPlansToGroup(5, new int[] { 1, 2, 3 });
            var expected = HttpStatusCode.InternalServerError;
            var actual = response.StatusCode;

            Assert.AreEqual(expected, actual);
        }
        #endregion
        #region SearchPlansNotUsedInCurrentGroup
        [Test]
        public void SearchPlansNotUsedInCurrentGroupShouldReturnOk()
        {
            groupServiceMock.Setup(u => u.SearchPlansNotUsedInGroup(It.IsAny<string[]>(), It.IsAny<int>())).Returns(new List<PlanDTO>());
            var response = groupController.SearchPlansNotUsedInCurrentGroup("search", 2);
            var expected = HttpStatusCode.OK;
            var actual = response.StatusCode;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void SearchPlansNotUsedInCurrentGroupShouldReturnNoContent()
        {
            groupServiceMock.Setup(u => u.SearchPlansNotUsedInGroup(It.IsAny<string[]>(), It.IsAny<int>())).Returns(() => null);
            var response = groupController.SearchPlansNotUsedInCurrentGroup("search", 2);
            var expected = HttpStatusCode.NoContent;
            var actual = response.StatusCode;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void SearchPlansNotUsedInCurrentGroupShouldReturnInternalServerError()
        {
            groupServiceMock.Setup(u => u.SearchPlansNotUsedInGroup(It.IsAny<string[]>(), It.IsAny<int>())).Throws(new EntityException());
            var response = groupController.SearchPlansNotUsedInCurrentGroup("search", 2);
            var expected = HttpStatusCode.InternalServerError;
            var actual = response.StatusCode;

            Assert.AreEqual(expected, actual);
        }
        #endregion
        #region SearchUsersNotUsedInCurrentGroup
        [Test]
        public void SearchUsersNotUsedInCurrentGroupOk()
        {
            groupServiceMock.Setup(u => u.SearchUserNotInGroup(It.IsAny<string[]>(), It.IsAny<int>())).Returns(new List<UserIdentityDTO>());
            var response = groupController.SearchPlansNotUsedInCurrentGroup("search", 2);
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
        public void RemoveUserFromCurrentGroupOk()
        {
            groupServiceMock.Setup(u => u.RemoveUserFromGroup(It.IsAny<int>(), It.IsAny<int>())).Returns(true);
            userIdentityServiceMock.Setup(u => u.GetUserId()).Returns(new int());
            groupServiceMock.Setup(u => u.GetMentorIdByGroup(It.IsAny<int>())).Returns(new int());
            var response = groupController.RemoveUserFromCurrentGroup(1, 2);
            var expected = HttpStatusCode.OK;
            var actual = response.StatusCode;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void RemoveUserFromCurrentGroupUnauthorized()
        {
            groupServiceMock.Setup(u => u.RemoveUserFromGroup(It.IsAny<int>(), It.IsAny<int>())).Returns(true);
            userIdentityServiceMock.Setup(u => u.GetUserId()).Returns(new int());
            groupServiceMock.Setup(u => u.GetMentorIdByGroup(It.IsAny<int>())).Returns(() => null);
            var response = groupController.RemoveUserFromCurrentGroup(1, 2);
            var expected = HttpStatusCode.Unauthorized;
            var actual = response.StatusCode;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void RemoveUserFromCurrentGroupNoContent()
        {
            groupServiceMock.Setup(u => u.RemoveUserFromGroup(It.IsAny<int>(), It.IsAny<int>())).Returns(false);
            userIdentityServiceMock.Setup(u => u.GetUserId()).Returns(new int());
            groupServiceMock.Setup(u => u.GetMentorIdByGroup(It.IsAny<int>())).Returns(new int());
            var response = groupController.RemoveUserFromCurrentGroup(1, 2);
            var expected = HttpStatusCode.NoContent;
            var actual = response.StatusCode;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void RemoveUserFromCurrentGroupInternalServerError()
        {
            groupServiceMock.Setup(u => u.RemoveUserFromGroup(It.IsAny<int>(), It.IsAny<int>())).Throws(new EntityException());
            userIdentityServiceMock.Setup(u => u.GetUserId()).Returns(new int());
            groupServiceMock.Setup(u => u.GetMentorIdByGroup(It.IsAny<int>())).Returns(new int());
            var response = groupController.RemoveUserFromCurrentGroup(1, 2);
            var expected = HttpStatusCode.InternalServerError;
            var actual = response.StatusCode;

            Assert.AreEqual(expected, actual);
        }
        #endregion
        #region RemovePlanFromCurrentGroup
        [Test]
        public void RemovePlanFromCurrentGroupOk()
        {
            groupServiceMock.Setup(u => u.RemovePlanFromGroup(It.IsAny<int>(), It.IsAny<int>())).Returns(true);
            var response = groupController.RemovePlanFromCurrentGroup(1, 2);
            var expected = HttpStatusCode.OK;
            var actual = response.StatusCode;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void RemovePlanFromCurrentGroupBadRequest()
        {
            groupServiceMock.Setup(u => u.RemovePlanFromGroup(It.IsAny<int>(), It.IsAny<int>())).Returns(false);
            var response = groupController.RemovePlanFromCurrentGroup(1, 2);
            var expected = HttpStatusCode.BadRequest;
            var actual = response.StatusCode;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void RemovePlanFromCurrentGroupInternalServerError()
        {
            groupServiceMock.Setup(u => u.RemovePlanFromGroup(It.IsAny<int>(), It.IsAny<int>())).Throws(new EntityException());
            var response = groupController.RemovePlanFromCurrentGroup(1, 2);
            var expected = HttpStatusCode.InternalServerError;
            var actual = response.StatusCode;

            Assert.AreEqual(expected, actual);
        }
        #endregion
        #region GetUserGroups
        [Test]
        public void GetUserGroupsOk()
        {
            userIdentityServiceMock.Setup(u => u.GetUserId()).Returns(new int());
            userServiceMock.Setup(u => u.ContainsId(It.IsAny<int>())).Returns(true);
            groupServiceMock.Setup(u => u.GetUserGroups(It.IsAny<int>())).Returns(new List<GroupDTO>());
            groupServiceMock.Setup(u => u.GroupsCount()).Returns(1);
            var response = groupController.GetUserGroups();
            var expected = HttpStatusCode.OK;
            var actual = response.StatusCode;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void GetUserGroupsNoUsers()
        {
            userIdentityServiceMock.Setup(u => u.GetUserId()).Returns(new int());
            userServiceMock.Setup(u => u.ContainsId(It.IsAny<int>())).Returns(false);
            groupServiceMock.Setup(u => u.GetUserGroups(It.IsAny<int>())).Returns(new List<GroupDTO>());
            groupServiceMock.Setup(u => u.GroupsCount()).Returns(1);
            var response = groupController.GetUserGroups();
            var expected = HttpStatusCode.NoContent;
            var actual = response.StatusCode;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void GetUserGroupsNoGroups()
        {
            userIdentityServiceMock.Setup(u => u.GetUserId()).Returns(new int());
            userServiceMock.Setup(u => u.ContainsId(It.IsAny<int>())).Returns(true);
            groupServiceMock.Setup(u => u.GetUserGroups(It.IsAny<int>())).Returns(new List<GroupDTO>());
            groupServiceMock.Setup(u => u.GroupsCount()).Returns(0);
            var response = groupController.GetUserGroups();
            var expected = HttpStatusCode.NoContent;
            var actual = response.StatusCode;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void GetUserGroupsBadRequest()
        {
            userIdentityServiceMock.Setup(u => u.GetUserId()).Returns(new int());
            userServiceMock.Setup(u => u.ContainsId(It.IsAny<int>())).Returns(true);
            groupServiceMock.Setup(u => u.GetUserGroups(It.IsAny<int>())).Returns(() => null);
            groupServiceMock.Setup(u => u.GroupsCount()).Returns(1);
            var response = groupController.GetUserGroups();
            var expected = HttpStatusCode.BadRequest;
            var actual = response.StatusCode;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void GetUserGroupsInternalServerError()
        {
            userIdentityServiceMock.Setup(u => u.GetUserId()).Throws(new EntityException());
            userServiceMock.Setup(u => u.ContainsId(It.IsAny<int>())).Returns(true);
            groupServiceMock.Setup(u => u.GetUserGroups(It.IsAny<int>())).Returns(new List<GroupDTO>());
            groupServiceMock.Setup(u => u.GroupsCount()).Returns(1);
            var response = groupController.GetUserGroups();
            var expected = HttpStatusCode.InternalServerError;
            var actual = response.StatusCode;

            Assert.AreEqual(expected, actual);
        }
        #endregion
    }
}
