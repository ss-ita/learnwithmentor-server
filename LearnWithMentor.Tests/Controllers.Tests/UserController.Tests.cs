using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using NUnit.Framework;
using Moq;
using LearnWithMentor.Controllers;
using System.Web.Http;
using LearnWithMentorBLL.Interfaces;
using LearnWithMentorDTO;

namespace LearnWithMentor.Tests.Controllers.Tests
{
    [TestFixture]
    public class UserControllerTests
    {
        private UserController userController;

        [OneTimeSetUp]
        public void SetUp()
        {
            var result = new List<UserDTO>()
            {
                new UserDTO(1, "test1", "test1", "Student", false),
                new UserDTO(2, "test2", "test2", "Student", false),
                new UserDTO(3, "test3", "test3", "Mentor", false),
                new UserDTO(4, "test4", "test4", "Admin", false)
            };
            var userServiceMock = new Mock<IUserService>();
            userServiceMock.Setup(u => u.GetAllUsers()).Returns(result);

            var userPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Role, "Admin")
            }));

            userController = new UserController(userServiceMock.Object, null, null, null);
            userController.ControllerContext.RequestContext.Principal = userPrincipal;
            userController.Request = new HttpRequestMessage();
            userController.Configuration = new HttpConfiguration();
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            userController.Dispose();
        }
        [Test]
        public void ShouldReturnAllUsers()
        {
            var response = userController.Get();
            response.TryGetContentValue<IEnumerable<UserDTO>>(out var userDTOS);
            var expected = 4;
            var actual = userDTOS.Count();

            Assert.AreEqual(expected, actual);
        }
    }
}
