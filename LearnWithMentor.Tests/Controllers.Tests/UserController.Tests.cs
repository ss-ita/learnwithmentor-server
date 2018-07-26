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

namespace LearnWithMentor.Tests.Controllers.Tests
{
    [TestFixture]
    public class UserControllerTests
    {
        private UserController userController;
        private Mock<IUserService> userServiceMock;
        private Mock<IRoleService> roleServiceMock;
        private Mock<ITraceWriter> traceWriterMock;
        private List<UserDTO> users;
        private List<RoleDTO> roles;

        [OneTimeSetUp]
        public void SetUp()
        {
            users = new List<UserDTO>()
            {
                new UserDTO(1, "test1", "test1", "Student", false),
                new UserDTO(2, "test2", "test2", "Student", false),
                new UserDTO(3, "test3", "test3", "Mentor", false),
                new UserDTO(4, "test4", "test4", "Admin", false)
            };
            roles = new List<RoleDTO>()
            {
                new RoleDTO(1, "Student"),
                new RoleDTO(2, "Mentor"),
                new RoleDTO(3, "Admin")
            };

            userServiceMock = new Mock<IUserService>();
            roleServiceMock = new Mock<IRoleService>();
            traceWriterMock = new Mock<ITraceWriter>();

            var userPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Role, "Admin"),
                new Claim("Id", "4") 
            }));
            
            userController = new UserController(userServiceMock.Object, roleServiceMock.Object, null, traceWriterMock.Object);
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
        public void GetAllUsersTest()
        {
            userServiceMock.Setup(u => u.GetAllUsers()).Returns(users);

            var response = userController.Get();
            response.TryGetContentValue<IEnumerable<UserDTO>>(out var userDTOs);
            var expected = userServiceMock.Object.GetAllUsers().Count;
            var actual = userDTOs.Count();

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void SearchUsersTest()
        {
            roleServiceMock.Setup(r => r.Get(It.IsInRange(1, 3, Range.Inclusive))).Returns(roles.First());
            userServiceMock.Setup(u => u.Search(It.IsAny<string[]>(), It.IsAny<int?>())).Returns(users);

            var response = userController.Search("test", "test");
            response.TryGetContentValue<IEnumerable<UserDTO>>(out var userDTOs);
            var expected = userServiceMock.Object.Search(new []{"test"}, 1).Count;
            var actual = userDTOs.Count();

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void GetUsersByRoleTest()
        {
            roleServiceMock.Setup(r => r.Get(It.IsInRange(1, 3, Range.Inclusive))).Returns(roles.First());
            userServiceMock.Setup(u => u.GetUsersByRole(It.IsInRange(1, 2, Range.Inclusive)))
                .Returns(users.Where(u => u.Role == "Student").ToList());

            var response = userController.GetUsersbyRole(1);
            response.TryGetContentValue<IEnumerable<UserDTO>>(out var userDTOs);
            var expected = userServiceMock.Object.GetUsersByRole(1).Count;
            var actual = userDTOs.Count();

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void GetUsersByStateTest()
        {
            userServiceMock.Setup(u => u.GetUsersByState(false)).Returns(users);

            var response = userController.GetUsersbyState(false);
            response.TryGetContentValue<IEnumerable<UserDTO>>(out var userDTOs);
            var expected = userServiceMock.Object.GetUsersByState(false).Count;
            var actual = userDTOs.Count();

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void GetRolesTest()
        {
            roleServiceMock.Setup(r => r.GetAllRoles()).Returns(roles);

            var response = userController.GetRoles();
            response.TryGetContentValue<IEnumerable<RoleDTO>>(out var roleDTOs);
            var expected = roleServiceMock.Object.GetAllRoles().Count;
            var actual = roleDTOs.Count();

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void GetImageTest()
        {
            userServiceMock.Setup(u => u.ContainsId(It.IsInRange(1, 8, Range.Inclusive))).Returns(true);
            userServiceMock.Setup(u => u.GetImage(It.IsInRange(1, 3, Range.Inclusive))).Returns(new ImageDTO()
            {
                Base64Data = "test",
                Name = "test"
            });

            var response = userController.GetImage(1);
            response.TryGetContentValue<ImageDTO>(out var imageDTO);
            var expected = "test";
            var actual = imageDTO.Name;

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
            roleServiceMock.Setup(r => r.Get(4));

            var response = userController.GetUsersbyRole(4);
            var expected = HttpStatusCode.NoContent;
            var actual = response.StatusCode;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void NoUsersInGetUsersByRoleTest()
        {
            roleServiceMock.Setup(r => r.GetByName(It.IsAny<string>())).Returns(roles.First());
            userServiceMock.Setup(u => u.GetUsersByRole(3)).Returns(new List<UserDTO>());

            var response = userController.GetUsersbyRole(3);
            var expected = HttpStatusCode.NoContent;
            var actual = response.StatusCode;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void NoUsersInGetUsersByStateTest()
        {
            userServiceMock.Setup(u => u.GetUsersByState(true)).Returns(new List<UserDTO>());

            var response = userController.GetUsersbyState(true);
            var expected = HttpStatusCode.NoContent;
            var actual = response.StatusCode;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void NoImageInGetImageTest()
        {
            userServiceMock.Setup(u => u.GetImage(4));

            var response = userController.GetImage(4);
            var expected = HttpStatusCode.NoContent;
            var actual = response.StatusCode;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void NoUserInGetImageTest()
        {
            var response = userController.GetImage(5);
            var expected = HttpStatusCode.NoContent;
            var actual = response.StatusCode;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ExceptionInGetImageTest()
        {
            userServiceMock.Setup(u => u.ContainsId(It.IsInRange(1, 8, Range.Inclusive))).Returns(true);
            userServiceMock.Setup(u => u.GetImage(6)).Throws(new EntityException());

            Assert.Throws(typeof(EntityException), () => userController.GetImage(6));
        }

        [Test]
        public void BlockUserTest()
        {
            userServiceMock.Setup(u => u.BlockById(It.IsInRange(1, 3, Range.Inclusive))).Returns(true);

            var response = userController.Delete(1);
            var expected = HttpStatusCode.OK;
            var actual = response.StatusCode;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void NoUserInBlockUserTest()
        {
            userServiceMock.Setup(u => u.BlockById(4)).Returns(false);

            var response = userController.Delete(4);
            var expected = HttpStatusCode.NoContent;
            var actual = response.StatusCode;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void CreateUserTest()
        {
            userServiceMock.Setup(u => u.Add(It.IsAny<UserRegistrationDTO>())).Returns(true);

            UserRegistrationDTO requestValue = new UserRegistrationDTO("test@test.test", "Test", "Test", "123");
            var response = userController.Post(requestValue);
            var expected = HttpStatusCode.OK;
            var actual = response.StatusCode;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void InvalidSyntaxCreateUserTest()
        {
            userServiceMock.Setup(u => u.Add(It.IsAny<UserRegistrationDTO>())).Returns(false);

            var requestValue = new UserRegistrationDTO("test", "Test", "Test", "123");
            var response = userController.Post(requestValue);
            var expected = HttpStatusCode.BadRequest;
            var actual = response.StatusCode;

            Assert.AreEqual(expected, actual);
        }

        //[Test]
        //public void UpdatePasswordTest()
        //{
        //    userServiceMock.Setup(u => u.UpdatePassword(It.IsAny<int>(), It.IsAny<string>())).Returns(true);

        //    var response = userController.UpdatePassword("newPass");
        //    var expected = HttpStatusCode.OK;
        //    var actual = response.StatusCode;

        //    Assert.AreEqual(expected, actual);
        //}
    }
}
