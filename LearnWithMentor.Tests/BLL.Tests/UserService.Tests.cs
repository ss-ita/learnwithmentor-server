using LearnWithMentorBLL.Services;
using LearnWithMentorDAL.Entities;
using LearnWithMentorDAL.Repositories;
using LearnWithMentorDAL.UnitOfWork;
using LearnWithMentorDTO;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ThreadTask = System.Threading.Tasks;

namespace LearnWithMentor.Tests.BLL.Tests
{
    [TestFixture]
    public class UserServiceTests
    {
        private UserService userService;
        private Mock<LearnWithMentorContext> dbContextMock;
        private Mock<UnitOfWork> uowMock;
        private Mock<UserRepository> userRepositoryMock;

        [SetUp]
        public void SetUp()
        {
            dbContextMock = new Mock<LearnWithMentorContext>();
            userRepositoryMock = new Mock<UserRepository>(dbContextMock.Object);
            uowMock = new Mock<UnitOfWork>(dbContextMock.Object);
            userService = new UserService(uowMock.Object);
        }

        [TearDown]
        public void TearDown()
        {
            userService.Dispose();
            dbContextMock = null;
        }

        [Test]
        public async ThreadTask.Task GetUserById_ShouldReturnUserById()
        {
            uowMock.SetupGet(u => u.Users).Returns(userRepositoryMock.Object);
            uowMock.Setup(u => u.Users.Get(It.IsAny<int>())).ReturnsAsync(new User() { Id = 3, Role = new Role() });

            //arrange
            int userId = 3;

            //act
           var result = await userService.Get(userId);

            //assert
            Assert.IsNotNull(result);
            Assert.AreEqual(userId, result.Id);
            Assert.AreSame(result.GetType(), typeof(UserDto));
        }

        [Test]
        public async ThreadTask.Task GetUserById_ShouldReturnNull()
        {
            uowMock.SetupGet(u => u.Users).Returns(userRepositoryMock.Object);
            uowMock.Setup(u => u.Users.Get(It.IsAny<int>())).ReturnsAsync((User)null);

            //arrange
            int userId = 3;

            //act
            UserDto result = await userService.Get(userId);

            //assert
            Assert.IsNull(result);
        }

        [Test]
        public async ThreadTask.Task GetUserByEmail_ShouldReturnUserByEmail()
        {
            //arrange
            string userEmail = "qwerty@gmail.com";

            uowMock.SetupGet(u => u.Users).Returns(userRepositoryMock.Object);
            uowMock.Setup(u => u.Users.GetByEmail(It.IsAny<string>())).ReturnsAsync(new User() { Email = userEmail, Role = new Role() });

            //act
            UserIdentityDto result = await userService.GetByEmail(userEmail);

            //assert
            Assert.IsNotNull(result);
            Assert.AreEqual(userEmail, result.Email);
            Assert.AreSame(result.GetType(), typeof(UserIdentityDto));
        }

        [Test]
        public async ThreadTask.Task GetUserByEmail_ShouldReturnNull()
        {
            uowMock.SetupGet(u => u.Users).Returns(userRepositoryMock.Object);
            uowMock.Setup(u => u.Users.GetByEmail(It.IsAny<string>())).ReturnsAsync((User)null);

            //arrange
            string userEmail = "qwerty@gmail.com";

            //act
            UserIdentityDto result = await userService.GetByEmail(userEmail);

            //assert
            Assert.IsNull(result);
        }

        [Test]
        public async ThreadTask.Task GetAllUsers_ShouldReturnAllUsers()
        {
            //arrange
            var users = new List<User>
            {
                new User() {Id=1, Role=new Role() },
                new User() {Id=2, Role=new Role() },
                new User() {Id=3, Role=new Role() }
            };

            uowMock.SetupGet(u => u.Users).Returns(userRepositoryMock.Object);
            uowMock.Setup(u => u.Users.GetAll()).Returns(users);

            //act
            List<UserDto> result = await userService.GetAllUsers();

            //assert
            Assert.IsNotNull(result);
            Assert.AreEqual(users.Count, result.Count);

            for (int i = 0; i < users.Count; i++)
            {
                Assert.AreEqual(users[i].Id, result[i].Id);
            }

            Assert.AreSame(result.GetType(), typeof(List<UserDto>));
        }

        [Test]
        public async ThreadTask.Task GetAllUsers_ShouldReturnNull()
        {
            //arrange
            uowMock.SetupGet(u => u.Users).Returns(userRepositoryMock.Object);
            uowMock.Setup(u => u.Users.GetAll()).Returns((List<User>)null);

            //act
          var result = await userService.GetAllUsers();

            //assert
            Assert.IsNull(result);
        }
    }
}
