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
using System.Threading.Tasks;

namespace LearnWithMentor.Tests.BLL.Tests
{
    [TestFixture]
    public class UserServiceTests
    {
        private UserService userService;
        private Mock<LearnWithMentor_DBEntities> dbContextMock;
        private Mock<UnitOfWork> uowMock;
        private Mock<UserRepository> userRepositoryMock;

        [SetUp]
        public void SetUp()
        {
            dbContextMock = new Mock<LearnWithMentor_DBEntities>();
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
        public void GetUserById_ShouldReturnUserById()
        {
            uowMock.SetupGet(u => u.Users).Returns(userRepositoryMock.Object);
            uowMock.Setup(u => u.Users.Get(It.IsAny<int>())).Returns(new User() { Id = 3, Roles = new Role() });

            //arrange
            int userId = 3;

            //act
            var result = userService.Get(userId);

            //assert
            Assert.IsNotNull(result);
            Assert.AreEqual(userId, result.Id);
            Assert.AreSame(result.GetType(), typeof(UserDTO));
        }

        [Test]
        public void GetUserById_ShouldReturnNull()
        {
            uowMock.SetupGet(u => u.Users).Returns(userRepositoryMock.Object);
            uowMock.Setup(u => u.Users.Get(It.IsAny<int>())).Returns((User)null);

            //arrange
            int userId = 3;

            //act
            var result = userService.Get(userId);

            //assert
            Assert.IsNull(result);
        }

        [Test]
        public void GetUserByEmail_ShouldReturnUserByEmail()
        {
            //arrange
            string userEmail = "qwerty@gmail.com";

            uowMock.SetupGet(u => u.Users).Returns(userRepositoryMock.Object);
            uowMock.Setup(u => u.Users.GetByEmail(It.IsAny<string>())).Returns(new User() { Email = userEmail, Roles = new Role() });

            //act
            var result = userService.GetByEmail(userEmail);

            //assert
            Assert.IsNotNull(result);
            Assert.AreEqual(userEmail, result.Email);
            Assert.AreSame(result.GetType(), typeof(UserIdentityDTO));
        }

        [Test]
        public void GetUserByEmail_ShouldReturnNull()
        {
            uowMock.SetupGet(u => u.Users).Returns(userRepositoryMock.Object);
            uowMock.Setup(u => u.Users.GetByEmail(It.IsAny<string>())).Returns((User)null);

            //arrange
            string userEmail = "qwerty@gmail.com";

            //act
            var result = userService.GetByEmail(userEmail);

            //assert
            Assert.IsNull(result);
        }

        [Test]
        public void GetAllUsers_ShouldReturnAllUsers()
        {
            //arrange
            var users = new List<User>
            {
                new User() {Id=1, Roles=new Role() },
                new User() {Id=2, Roles=new Role() },
                new User() {Id=3, Roles=new Role() }
            };

            uowMock.SetupGet(u => u.Users).Returns(userRepositoryMock.Object);
            uowMock.Setup(u => u.Users.GetAll()).Returns(users);
            
            //act
            var result = userService.GetAllUsers();

            //assert
            Assert.IsNotNull(result);
            Assert.AreEqual(users.Count, result.Count);

            for (int i = 0; i < users.Count; i++)
            {
                Assert.AreEqual(users[i].Id, result[i].Id);
            }

            Assert.AreSame(result.GetType(), typeof(List<UserDTO>));
        }

        [Test]
        public void GetAllUsers_ShouldReturnNull()
        {
            //arrange
            uowMock.SetupGet(u => u.Users).Returns(userRepositoryMock.Object);
            uowMock.Setup(u => u.Users.GetAll()).Returns((List<User>)null);

            //act
            var result = userService.GetAllUsers();

            //assert
            Assert.IsNull(result);
        }
    }
}
