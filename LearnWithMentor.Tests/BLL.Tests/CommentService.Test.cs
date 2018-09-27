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
    public class CommentServiceTests
    {
        private CommentService commentService;
        private Mock<LearnWithMentorContext> dbContextMock;
        private Mock<UnitOfWork> uowMock;
        private Mock<CommentRepository> commentRepositoryMock;
        private Mock<UserRepository> userRepositoryMock;
        private Mock<PlanTaskRepository> planTaskRepositoryMock;

        [SetUp]
        public void SetUp()
        {
            dbContextMock = new Mock<LearnWithMentorContext>();
            commentRepositoryMock = new Mock<CommentRepository>(dbContextMock.Object);
            userRepositoryMock = new Mock<UserRepository>(dbContextMock.Object);
            planTaskRepositoryMock = new Mock<PlanTaskRepository>(dbContextMock.Object);
            uowMock = new Mock<UnitOfWork>(dbContextMock.Object);
            commentService = new CommentService(uowMock.Object);
        }

        [TearDown]
        public void TearDown()
        {
            commentService.Dispose();
            dbContextMock = null;
        }

        [Test]
        public async ThreadTask.Task GetCommentById_ShouldReturnNullValue()
        {
            uowMock.SetupGet(c => c.Comments).Returns(commentRepositoryMock.Object);
            uowMock.SetupGet(u => u.Users).Returns(userRepositoryMock.Object);

            uowMock.Setup(u => u.Users.Get(It.IsAny<int>())).ReturnsAsync(new User() { Comments = new HashSet<Comment>() { new Comment() { Id = 3 } } });
            uowMock.Setup(c => c.Comments.Get(It.IsAny<int>())).ReturnsAsync((Comment)null);

            //arrange
            int commentId = 3;

            //act
            var result = await commentService.GetCommentAsync(commentId);

            //assert
            Assert.IsNull(result);
        }

        [Test]
        public async ThreadTask.Task RemoveById_NotRemoveBecauseNotExist()
        {
            uowMock.SetupGet(c => c.Comments).Returns(commentRepositoryMock.Object);
            uowMock.SetupGet(u => u.Users).Returns(userRepositoryMock.Object);

            uowMock.Setup(u => u.Users.Get(It.IsAny<int>())).ReturnsAsync(new User() { Comments = new HashSet<Comment>() { new Comment() { Id = 3 } } });
            uowMock.Setup(c => c.Comments.Get(It.IsAny<int>())).ReturnsAsync( (Comment)null);

            //arrange
            int commentId = 3;

            //act
            var result = await commentService.RemoveByIdAsync(commentId);

            //assert
            Assert.IsFalse(result);
        }

        [Test]
        public async ThreadTask.Task UpdateCommentIdText_ShouldReturnUpdatedText()
        {
            uowMock.SetupGet(c => c.Comments).Returns(commentRepositoryMock.Object);
            uowMock.SetupGet(u => u.Users).Returns(userRepositoryMock.Object);
            uowMock.Setup(u => u.Users.Get(It.IsAny<int>())).ReturnsAsync(new User() { Comments = new HashSet<Comment>() { new Comment() { Id = 3 } } });
            uowMock.Setup(c => c.Comments.Get(It.IsAny<int>())).ReturnsAsync(new Comment() { Id = 3, Creator = new User() { Comments = new HashSet<Comment>() { new Comment() { Id = 3 } } } });


            //arrange
            string newText = "Hello";
            int commentId = 3;

            //act
            var result = await commentService.UpdateCommentIdTextAsync(commentId,newText);

            //assert
            Assert.IsTrue(result);
        }

        [Test]
        public async ThreadTask.Task UpdateCommentIdText_ShouldReturnNullBecauseEmptyNewString()
        {
            uowMock.SetupGet(c => c.Comments).Returns(commentRepositoryMock.Object);
            uowMock.SetupGet(u => u.Users).Returns(userRepositoryMock.Object);

            uowMock.Setup(u => u.Users.Get(It.IsAny<int>())).ReturnsAsync(new User() { Comments = new HashSet<Comment>() { new Comment() { Id = 3 } } });
            uowMock.Setup(c => c.Comments.Get(It.IsAny<int>())).ReturnsAsync(new Comment() { Id = 3, Creator = new User() { Comments = new HashSet<Comment>() { new Comment() { Id = 3 } } } });


            //arrange
            string newText="";
            int commentId = 3;

            //act
            var result = await commentService.UpdateCommentIdTextAsync(commentId, newText);

            //assert
            Assert.IsFalse(result);
        }

        [Test]
        public async ThreadTask.Task UpdateCommentIdText_ShouldReturnNull()
        {
            uowMock.SetupGet(c => c.Comments).Returns(commentRepositoryMock.Object);
            uowMock.SetupGet(u => u.Users).Returns(userRepositoryMock.Object);

            uowMock.Setup(u => u.Users.Get(It.IsAny<int>())).ReturnsAsync(new User() { Comments = new HashSet<Comment>() { new Comment() { Id = 3 } } });
            uowMock.Setup(c => c.Comments.Get(It.IsAny<int>())).ReturnsAsync((Comment)null);

            //arrange
            string newText = "Hello";
            int commentId = 3;

            //act
            var result = await commentService.UpdateCommentIdTextAsync(commentId, newText);

            //assert
            Assert.IsFalse(result);
        }
    }
}
