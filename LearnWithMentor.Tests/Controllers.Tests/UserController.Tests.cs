using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using NUnit.Framework;
using Moq;
using LearnWithMentor.Controllers;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Tracing;
using LearnWithMentorBLL.Interfaces;
using LearnWithMentorDTO;
using System.Data.Entity.Core;
using System.Threading.Tasks;
using System.Data.Entity.Core;

namespace LearnWithMentor.Tests.Controllers.Tests
{
    [TestFixture]
    public class UserControllerTests
    {
        private UserController userController;
        private Mock<IUserService> userServiceMock;
        private Mock<IRoleService> roleServiceMock;
        private Mock<ITraceWriter> traceWriterMock;
        private Mock<ITaskService> taskServiceMock;
        private Mock<IUserIdentityService> userIdentityServiceMock;
        private List<UserDto> users;
        private List<RoleDto> roles;

        [OneTimeSetUp]
        public void SetUp()
        {
            users = new List<UserDto>()
            {
                new UserDto(1, "test1", "test1","test1", "Student", false, true),
                new UserDto(2, "test2", "test2","test2", "Student", false, true),
                new UserDto(3, "test3", "test3","test3", "Mentor", false, true),
                new UserDto(4, "test4", "test4","test4", "Admin", false, true)
            };
            roles = new List<RoleDto>()
            {
                new RoleDto(1, "Student"),
                new RoleDto(2, "Mentor"),
                new RoleDto(3, "Admin")
            };

            userServiceMock = new Mock<IUserService>();
            roleServiceMock = new Mock<IRoleService>();
            traceWriterMock = new Mock<ITraceWriter>();
            taskServiceMock = new Mock<ITaskService>();
            userIdentityServiceMock = new Mock<IUserIdentityService>();

            var userPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Role, "Admin"),
                new Claim("Id", "4")
            }));

            userController = new UserController(userServiceMock.Object, roleServiceMock.Object, taskServiceMock.Object, userIdentityServiceMock.Object, traceWriterMock.Object);
            userController.ControllerContext.RequestContext.Principal = userPrincipal;

            userController.ControllerContext.ControllerDescriptor = new HttpControllerDescriptor(new HttpConfiguration(),
                "UserController", userController.GetType());
            userController.Request = new HttpRequestMessage();
            userController.Configuration = new HttpConfiguration();
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            userController.Dispose();
            userServiceMock = null;
            roleServiceMock = null;
            traceWriterMock = null;
            users = null;
            roles = null;
        }

        [Test]
        public async Task GetAllUsersTest()
        {
            userServiceMock.Setup(u => u.GetAllUsersAsync()).ReturnsAsync(users);

            HttpResponseMessage response = await userController.GetAsync();
            response.TryGetContentValue<IEnumerable<UserDto>>(out var userDTOs);
            var expected =( await userServiceMock.Object.GetAllUsersAsync()).Count;
            var actual = userDTOs.Count();

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task SearchUsersTest()
        {
            roleServiceMock.Setup(r => r.GetAsync(It.IsInRange(1, 3, Range.Inclusive))).ReturnsAsync(roles.First());
            userServiceMock.Setup(u => u.SearchAsync(It.IsAny<string[]>(), It.IsAny<int?>())).ReturnsAsync(users);

            HttpResponseMessage response = await userController.SearchAsync("test", "test");
            response.TryGetContentValue<IEnumerable<UserDto>>(out var userDTOs);
            var expected = (await userServiceMock.Object.SearchAsync(new[] { "test" }, 1)).Count;
            var actual = userDTOs.Count();

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task GetUsersByRoleTest()
        {
            roleServiceMock.Setup(r => r.GetAsync(It.IsInRange(1, 3, Range.Inclusive))).ReturnsAsync(roles.First());
            userServiceMock.Setup(u => u.GetUsersByRoleAsync(It.IsInRange(1, 2, Range.Inclusive)))
                .ReturnsAsync(users.Where(u => u.Role == "Student").ToList());

            HttpResponseMessage response = await userController.GetUsersbyRoleAsync(1);
            response.TryGetContentValue<IEnumerable<UserDto>>(out var userDTOs);
            var getUsers = await userServiceMock.Object.GetUsersByRoleAsync(1);
            var expected = getUsers.Count;
            var actual = userDTOs.Count();

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task GetUsersByStateTest()
        {
            userServiceMock.Setup(u => u.GetUsersByStateAsync(false)).ReturnsAsync(users);

            HttpResponseMessage response = await userController.GetUsersbyStateAsync(false);
            response.TryGetContentValue<IEnumerable<UserDto>>(out var userDTOs);
            var getGroup = await userServiceMock.Object.GetUsersByStateAsync(false);
            var expected = getGroup.Count;

            var actual = userDTOs.Count();

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task GetRolesTest()
        {
            roleServiceMock.Setup(r => r.GetAllRoles()).ReturnsAsync(roles);

            var response = await userController.GetRoles();
            response.TryGetContentValue<IEnumerable<RoleDto>>(out var roleDTOs);
            //var expected = await roleServiceMock.Object.GetAllRoles().Count;
            var expect = await roleServiceMock.Object.GetAllRoles();
            var expected = expect.Count;
            var actual = roleDTOs.Count();

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task GetSingleTest()
        {
            userIdentityServiceMock.Setup(u => u.GetUserId()).Returns(1);
            userServiceMock.Setup(u => u.GetAsync(1)).ReturnsAsync(users.First());

            HttpResponseMessage response = await userController.GetSingleAsync();
            response.TryGetContentValue<UserDto>(out var userDTO);
            var expectedUserId = users.First().Id;
            var actualUserId = userDTO.Id;
            var expectedStatusCode = HttpStatusCode.OK;
            var actualStatusCode = response.StatusCode;

            Assert.AreEqual(expectedUserId, actualUserId);
            Assert.AreEqual(expectedStatusCode, actualStatusCode);
        }

        [Test]
        public async Task GetImageTest()
        {
            userServiceMock.Setup(u => u.ContainsIdAsync(It.IsInRange(1, 8, Range.Inclusive))).ReturnsAsync(true);
            userServiceMock.Setup(u => u.GetImageAsync(It.IsInRange(1, 3, Range.Inclusive))).ReturnsAsync(new ImageDto()
            {
                Base64Data = "test",
                Name = "test"
            });

            HttpResponseMessage response = await userController.GetImageAsync(1);
            response.TryGetContentValue<ImageDto>(out var imageDTO);
            var expected = "test";
            var actual = imageDTO.Name;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task GetStatisticsTest()
        {
            var statsDTO = new StatisticsDto()
            {
                ApprovedNumber = 1,
                DoneNumber = 1,
                RejectedNumber = 1,
                InProgressNumber = 1
            };
            userIdentityServiceMock.Setup(u => u.GetUserId()).Returns(1);
            taskServiceMock.Setup(t => t.GetUserStatisticsAsync(1)).ReturnsAsync(statsDTO);

            HttpResponseMessage response = await userController.GetStatisticsAsync();
            response.TryGetContentValue<StatisticsDto>(out var resultDTO);
            var expectedNumber = statsDTO.ApprovedNumber;
            var actualNumber = resultDTO.ApprovedNumber;
            var expectedStatusCode = HttpStatusCode.OK;
            var actualStatusCode = response.StatusCode;

            Assert.AreEqual(expectedNumber, actualNumber);
            Assert.AreEqual(expectedStatusCode, actualStatusCode);
        }

        [Test]
        public async Task NoUsersInDatabaseTest()
        {
            roleServiceMock.Setup(r => r.GetAllRoles()).ReturnsAsync(new List<RoleDto>());

            HttpResponseMessage response = await userController.GetAsync();
            var expected = HttpStatusCode.NoContent;
            var actual = response.StatusCode;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task NoRolesInDatabaseTest()
        {
            userServiceMock.Setup(u => u.GetAllUsersAsync()).ReturnsAsync(new List<UserDto>());

            HttpResponseMessage response = await userController.GetAsync();
            var expected = HttpStatusCode.NoContent;
            var actual = response.StatusCode;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task NotExistingRoleInGetUsersByRoleTest()
        {
            roleServiceMock.Setup(r => r.GetAsync(4));

            HttpResponseMessage response = await userController.GetUsersbyRoleAsync(4);
            var expected = HttpStatusCode.NoContent;
            var actual = response.StatusCode;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task NoUsersInGetUsersByRoleTest()
        {
            roleServiceMock.Setup(r => r.GetByNameAsync(It.IsAny<string>())).ReturnsAsync(roles.First());
            userServiceMock.Setup(u => u.GetUsersByRoleAsync(3)).ReturnsAsync(new List<UserDto>());

            HttpResponseMessage response = await userController.GetUsersbyRoleAsync(3);
            var expected = HttpStatusCode.NoContent;
            var actual = response.StatusCode;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task NoUsersInGetUsersByStateTest()
        {
            userServiceMock.Setup(u => u.GetUsersByStateAsync(true)).ReturnsAsync(new List<UserDto>());

            HttpResponseMessage response = await userController.GetUsersbyStateAsync(true);
            var expected = HttpStatusCode.NoContent;
            var actual = response.StatusCode;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task NoImageInGetImageTest()
        {
            userServiceMock.Setup(u => u.GetImageAsync(4)).Returns(Task.FromResult<ImageDto>(null));

            HttpResponseMessage response = await userController.GetImageAsync(4);
            var expected = HttpStatusCode.NoContent;
            var actual = response.StatusCode;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task NoUserInGetImageTest()
        {
            HttpResponseMessage response = await userController.GetImageAsync(5);
            var expected = HttpStatusCode.NoContent;
            var actual = response.StatusCode;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task NoUserInGetStatisticsTest()
        {
            userIdentityServiceMock.Setup(u => u.GetUserId()).Returns(1);
            taskServiceMock.Setup(t => t.GetUserStatisticsAsync(1)).Returns(Task.FromResult<StatisticsDto>(null));

            HttpResponseMessage response = await userController.GetStatisticsAsync();
            var expectedStatusCode =  HttpStatusCode.NoContent;
            var actualStatusCode = response.StatusCode;

            Assert.AreEqual(expectedStatusCode, actualStatusCode);
        }

        /*[Test]
        public void ExceptionInGetImageTest()
        {
            userServiceMock.Setup(u => u.ContainsIdAsync(It.IsInRange(1, 8, Range.Inclusive))).ReturnsAsync(true);
            userServiceMock.Setup(u => u.GetImageAsync(6)).Throws(new EntityException());
            Assert.Throws(typeof(EntityException), () => userController.GetImageAsync(6).GetAwaiter().GetResult());
        }*/

        [Test]
        public async Task BlockUserTest()
        {
            userServiceMock.Setup(u => u.BlockByIdAsync(It.IsInRange(1, 3, Range.Inclusive))).ReturnsAsync(true);

            HttpResponseMessage response = await userController.DeleteAsync(1);
            var expected = HttpStatusCode.OK;
            var actual = response.StatusCode;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task NoUserInBlockUserTest()
        {
            userServiceMock.Setup(u => u.BlockByIdAsync(4)).ReturnsAsync(false);

            HttpResponseMessage response = await userController.DeleteAsync(4);
            var expected = HttpStatusCode.NoContent;
            var actual = response.StatusCode;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task NoUserGetSingleTest()
        {
            userIdentityServiceMock.Setup(u => u.GetUserId()).Returns(1);
            userServiceMock.Setup(u => u.GetAsync(1)).Returns(Task.FromResult<UserDto>(null));

            HttpResponseMessage response = await userController.GetSingleAsync();
            var expectedStatusCode = HttpStatusCode.NoContent;
            var actualStatusCode = response.StatusCode;

            Assert.AreEqual(expectedStatusCode, actualStatusCode);
        }

        [Test]
        public async Task CreateUserTest()
        {
            userServiceMock.Setup(u => u.AddAsync(It.IsAny<UserRegistrationDto>())).ReturnsAsync(true);

            UserRegistrationDto requestValue = new UserRegistrationDto("test@test.test", "Test", "Test", "123");
            var response = await userController.PostAsync(requestValue);
            var expected = HttpStatusCode.OK;
            var actual = response.StatusCode;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task UpdateUserTest()
        {
            userServiceMock.Setup(u => u.UpdateByIdAsync(It.IsAny<int>(), It.IsAny<UserDto>())).ReturnsAsync(true);

            UserDto reqestValue = new UserDto(1, "test", "test", "test", "test", false, true);
            HttpResponseMessage response = await userController.PutAsync(1, reqestValue);
            var expectedStatusCode = HttpStatusCode.OK;
            var actualStatusCode = response.StatusCode;

            Assert.AreEqual(expectedStatusCode, actualStatusCode);
        }

        [Test]
        public async Task NoUserInUpdateUserTest()
        {
            userServiceMock.Setup(u => u.UpdateByIdAsync(It.IsAny<int>(), It.IsAny<UserDto>())).ReturnsAsync(false);

            UserDto reqestValue = new UserDto(1, "test", "test", "test", "test",false, true);
            HttpResponseMessage response = await userController.PutAsync(1, reqestValue);
            var expectedStatusCode = HttpStatusCode.BadRequest;
            var actualStatusCode = response.StatusCode;

            Assert.AreEqual(expectedStatusCode, actualStatusCode);
        }

        [Test]
        public async Task InvalidSyntaxCreateUserTest()
        {
            userServiceMock.Setup(u => u.AddAsync(It.IsAny<UserRegistrationDto>())).ReturnsAsync(false);

            var requestValue = new UserRegistrationDto("test", "Test", "Test", "123");
            var response = await userController.PostAsync(requestValue);
            var expected = HttpStatusCode.BadRequest;
            var actual = response.StatusCode;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task UpdatePasswordTest()
        {
            userServiceMock.Setup(u => u.UpdatePasswordAsync(It.IsAny<int>(), It.IsAny<string>())).ReturnsAsync(true);
            userIdentityServiceMock.Setup(u => u.GetUserId()).Returns(1);

            HttpResponseMessage response = await userController.UpdatePasswordAsync("newPass");
            var expected = HttpStatusCode.OK;
            var actual = response.StatusCode;

            Assert.AreEqual(expected, actual);
        }
    }
}
