using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web;
using NUnit.Framework;
using Moq;
using LearnWithMentor.Controllers;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Tracing;
using LearnWithMentorBLL.Interfaces;
using LearnWithMentorDTO;
using NUnit.Framework.Internal;

namespace LearnWithMentor.Tests.Controllers.Tests
{
    [TestFixture]
    public class UserControllerTests
    {
        private UserController userController;
        private Mock<IUserService> userServiceMock;
        private Mock<IRoleService> roleServiceMock;
        private Mock<ITraceWriter> traceWriterMock;

        [OneTimeSetUp]
        public void SetUp()
        {
            var users = new List<UserDTO>()
            {
                new UserDTO(1, "test1", "test1", "Student", false),
                new UserDTO(2, "test2", "test2", "Student", false),
                new UserDTO(3, "test3", "test3", "Mentor", false),
                new UserDTO(4, "test4", "test4", "Admin", false)
            };
            var roles = new List<RoleDTO>()
            {
                new RoleDTO(1, "Student"),
                new RoleDTO(2, "Mentor"),
                new RoleDTO(3, "Admin")
            };

            userServiceMock = new Mock<IUserService>();
            roleServiceMock = new Mock<IRoleService>();
            traceWriterMock = new Mock<ITraceWriter>();

            userServiceMock.Setup(u => u.GetAllUsers()).Returns(users);
            userServiceMock.Setup(u => u.BlockById(It.IsInRange(1, 4, Range.Inclusive))).Returns(true);
            userServiceMock.Setup(u => u.GetUsersByRole(It.IsInRange(1,2, Range.Inclusive)))
                .Returns(users.Where(u => u.Role == "Student").ToList());
            userServiceMock.Setup(u => u.GetUsersByRole(3)).Returns(new List<UserDTO>());
            userServiceMock.Setup(u => u.UpdatePassword(It.IsAny<int>(), It.IsAny<string>())).Returns(true);
            userServiceMock.Setup(u => u.GetUsersByState(false)).Returns(users);
            userServiceMock.Setup(u => u.GetUsersByState(true)).Returns(new List<UserDTO>());

            roleServiceMock.Setup(r => r.GetAllRoles()).Returns(roles);
            roleServiceMock.Setup(r => r.Get(It.IsInRange(1, 3, Range.Inclusive))).Returns(roles.First());
            roleServiceMock.Setup(r => r.Get(4));


            var userPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Role, "Admin"),
                new Claim("Id", "4") 
            }));
            
            userController = new UserController(userServiceMock.Object, roleServiceMock.Object, null, traceWriterMock.Object);
            userController.ControllerContext.RequestContext.Principal = userPrincipal;

            userController.ControllerContext.ControllerDescriptor = new HttpControllerDescriptor(new HttpConfiguration(), "UserController", userController.GetType());
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
        }
        [Test]
        public void GetAllUsersTest()
        {
            var response = userController.Get();
            response.TryGetContentValue<IEnumerable<UserDTO>>(out var userDTOs);
            var expected = userServiceMock.Object.GetAllUsers().Count();
            var actual = userDTOs.Count();

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void GetUsersByRoleTest()
        {
            var response = userController.GetUsersbyRole(1);
            response.TryGetContentValue<IEnumerable<UserDTO>>(out var userDTOs);
            var expected = userServiceMock.Object.GetUsersByRole(1).Count();
            var actual = userDTOs.Count();

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void GetUsersByStateTest()
        {
            var response = userController.GetUsersbyState(false);
            response.TryGetContentValue<IEnumerable<UserDTO>>(out var userDTOs);
            var expected = userServiceMock.Object.GetUsersByState(false).Count();
            var actual = userDTOs.Count();

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void GetRolesTest()
        {
            var response = userController.GetRoles();
            response.TryGetContentValue<IEnumerable<RoleDTO>>(out var roleDTOs);
            var expected = roleServiceMock.Object.GetAllRoles().Count();
            var actual = roleDTOs.Count();

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void NoUsersInDatabaseTest()
        {
            roleServiceMock.Setup(r => r.GetAllRoles()).Returns(new List<RoleDTO>());
            var response = userController.Get();
            var expected = HttpStatusCode.NoContent;
            var actual = response.StatusCode;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void NoRolesInDatabaseTest()
        {
            userServiceMock.Setup(u => u.GetAllUsers()).Returns(new List<UserDTO>());
            var response = userController.Get();
            var expected = HttpStatusCode.NoContent;
            var actual = response.StatusCode;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void NotExistingRoleInGetUsersByRoleTest()
        {
            var response = userController.GetUsersbyRole(4);
            var expected = HttpStatusCode.NoContent;
            var actual = response.StatusCode;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void NoUsersInGetUsersByRoleTest()
        {
            var response = userController.GetUsersbyRole(3);
            var expected = HttpStatusCode.NoContent;
            var actual = response.StatusCode;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void NoUsersInGetUsersByStateTest()
        {
            var response = userController.GetUsersbyState(true);
            var expected = HttpStatusCode.NoContent;
            var actual = response.StatusCode;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void BlockUserTest()
        {
            var response = userController.Delete(1);
            var expected = HttpStatusCode.OK;
            var actual = response.StatusCode;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void UpdatePasswordTest()
        {
            var response = userController.UpdatePassword("newPass");
            var expected = HttpStatusCode.OK;
            var actual = response.StatusCode;

            Assert.AreEqual(expected, actual);
        }
    }
}
