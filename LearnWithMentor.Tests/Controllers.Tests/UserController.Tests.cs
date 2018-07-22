using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using NUnit.Framework;
using Moq;
using LearnWithMentor.Controllers;
using System.Web.Http;
using System.Web.Http.Tracing;
using LearnWithMentorBLL.Interfaces;
using LearnWithMentorDTO;

namespace LearnWithMentor.Tests.Controllers.Tests
{
    [TestFixture]
    public class UserControllerTests
    {
        private UserController userController;
        private Mock<IUserService> userServiceMock;
        private Mock<IRoleService> roleServiceMock;

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

            userServiceMock.Setup(u => u.GetAllUsers()).Returns(users);
            userServiceMock.Setup(u => u.BlockById(It.IsInRange(1, 4, Range.Exclusive))).Returns(true);

            roleServiceMock.Setup(r => r.GetAllRoles()).Returns(roles);

            var userPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Role, "Admin")
            }));

            userController = new UserController(userServiceMock.Object, roleServiceMock.Object, null, null);
            userController.ControllerContext.RequestContext.Principal = userPrincipal;
            userController.Request = new HttpRequestMessage();
            userController.Configuration = new HttpConfiguration();
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            userController.Dispose();
            userServiceMock = null;
            roleServiceMock = null;
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
            userServiceMock.Setup(u => u.GetAllUsers()).Returns(new List<UserDTO>());
            var response = userController.Get();
            response.TryGetContentValue<IEnumerable<UserDTO>>(out var userDTOs);
            var expected = HttpStatusCode.NoContent;
            var actual = response.StatusCode;

            Assert.AreEqual(expected, actual);
        }
    }
}
