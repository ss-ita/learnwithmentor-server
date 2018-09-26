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
using Task = System.Threading.Tasks.Task;

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
        public async Task GetCommentById_ShouldReturnCommentById()
        {
            uowMock.SetupGet(c => c.Comments).Returns(commentRepositoryMock.Object);
            uowMock.SetupGet(u => u.Users).Returns(userRepositoryMock.Object);
<<<<<<< Updated upstream
            uowMock.Setup(u => u.Users.Get(It.IsAny<int>())).ReturnsAsync(new User() { Comments = new HashSet<Comment>() { new Comment() { Id = 3 } } });
            uowMock.Setup(c => c.Comments.Get(It.IsAny<int>())).Returns(new Comment() { Id = 3, Creator = new User() { Comments = new HashSet<Comment>() { new Comment() { Id = 3 } } } });
=======
            uowMock.Setup(u =>  u.Users.Get(It.IsAny<int>())).Returns(new User() { Comments = new HashSet<Comment>() { new Comment() { Id = 3 } } });
            uowMock.Setup(c => c.Comments.Get(It.IsAny<int>())).ReturnsAsync(new Comment() { Id = 3, Creator = new User() { Comments = new HashSet<Comment>() { new Comment() { Id = 3 } } } });
>>>>>>> Stashed changes

            //arrange
            int commentId = 3;

            //act
            var result = commentService.GetComment(commentId);

            //assert
            Assert.IsNotNull(result);
            Assert.AreEqual(commentId, result.Id);
            Assert.AreSame(result.GetType(), typeof(CommentDto));
        }

        [Test]
        public void GetCommentById_ShouldReturnNullValue()
        {
            uowMock.SetupGet(c => c.Comments).Returns(commentRepositoryMock.Object);
            uowMock.SetupGet(u => u.Users).Returns(userRepositoryMock.Object);
<<<<<<< Updated upstream
            uowMock.Setup(u => u.Users.Get(It.IsAny<int>())).ReturnsAsync(new User() { Comments = new HashSet<Comment>() { new Comment() { Id = 3 } } });
            uowMock.Setup(c => c.Comments.Get(It.IsAny<int>())).Returns((Comment)null);
=======
            uowMock.Setup(u => u.Users.Get(It.IsAny<int>())).Returns(new User() { Comments = new HashSet<Comment>() { new Comment() { Id = 3 } } });
            uowMock.Setup(c => c.Comments.Get(It.IsAny<int>())).ReturnsAsync((Comment)null);
>>>>>>> Stashed changes

            //arrange
            int commentId = 3;

            //act
            var result = commentService.GetComment(commentId);

            //assert
            Assert.IsNull(result);
        }

        [Test]
        public async Task RemoveById_NotRemoveBecauseNotExist()
        {
            uowMock.SetupGet(c => c.Comments).Returns(commentRepositoryMock.Object);
            uowMock.SetupGet(u => u.Users).Returns(userRepositoryMock.Object);
<<<<<<< Updated upstream
            uowMock.Setup(u => u.Users.Get(It.IsAny<int>())).ReturnsAsync(new User() { Comments = new HashSet<Comment>() { new Comment() { Id = 3 } } });
            uowMock.Setup(c => c.Comments.Get(It.IsAny<int>())).Returns( (Comment)null);
=======
            uowMock.Setup(u => u.Users.Get(It.IsAny<int>())).Returns(new User() { Comments = new HashSet<Comment>() { new Comment() { Id = 3 } } });
            uowMock.Setup(c => c.Comments.Get(It.IsAny<int>())).ReturnsAsync((Comment)null);
>>>>>>> Stashed changes

            //arrange
            int commentId = 3;

            //act
            var result = await commentService.RemoveById(commentId);

            //assert
            Assert.IsFalse(result);
        }

        [Test]
        public void UpdateCommentIdText_ShouldReturnUpdatedText()
        {
            uowMock.SetupGet(c => c.Comments).Returns(commentRepositoryMock.Object);
            uowMock.SetupGet(u => u.Users).Returns(userRepositoryMock.Object);
<<<<<<< Updated upstream
            uowMock.Setup(u => u.Users.Get(It.IsAny<int>())).ReturnsAsync(new User() { Comments = new HashSet<Comment>() { new Comment() { Id = 3 } } });
            uowMock.Setup(c => c.Comments.Get(It.IsAny<int>())).Returns(new Comment() { Id = 3, Creator = new User() { Comments = new HashSet<Comment>() { new Comment() { Id = 3 } } } });
=======
            uowMock.Setup(u => u.Users.Get(It.IsAny<int>())).Returns(new User() { Comments = new HashSet<Comment>() { new Comment() { Id = 3 } } });
            uowMock.Setup(c => c.Comments.Get(It.IsAny<int>())).ReturnsAsync(new Comment() { Id = 3, Creator = new User() { Comments = new HashSet<Comment>() { new Comment() { Id = 3 } } } });
>>>>>>> Stashed changes

            //arrange
            string newText = "Hello";
            int commentId = 3;

            //act
            var result = commentService.UpdateCommentIdText(commentId,newText);

            //assert
            Assert.IsTrue(result);
        }

        [Test]
        public void UpdateCommentIdText_ShouldReturnNullBecauseEmptyNewString()
        {
            uowMock.SetupGet(c => c.Comments).Returns(commentRepositoryMock.Object);
            uowMock.SetupGet(u => u.Users).Returns(userRepositoryMock.Object);
<<<<<<< Updated upstream
            uowMock.Setup(u => u.Users.Get(It.IsAny<int>())).ReturnsAsync(new User() { Comments = new HashSet<Comment>() { new Comment() { Id = 3 } } });
            uowMock.Setup(c => c.Comments.Get(It.IsAny<int>())).Returns(new Comment() { Id = 3, Creator = new User() { Comments = new HashSet<Comment>() { new Comment() { Id = 3 } } } });
=======
            uowMock.Setup(u => u.Users.Get(It.IsAny<int>())).Returns(new User() { Comments = new HashSet<Comment>() { new Comment() { Id = 3 } } });
            uowMock.Setup(c => c.Comments.Get(It.IsAny<int>())).ReturnsAsync(new Comment() { Id = 3, Creator = new User() { Comments = new HashSet<Comment>() { new Comment() { Id = 3 } } } });
>>>>>>> Stashed changes

            //arrange
            string newText="";
            int commentId = 3;

            //act
            var result = commentService.UpdateCommentIdText(commentId, newText);

            //assert
            Assert.IsFalse(result);
        }

        [Test]
        public void UpdateCommentIdText_ShouldReturnNull()
        {
            uowMock.SetupGet(c => c.Comments).Returns(commentRepositoryMock.Object);
            uowMock.SetupGet(u => u.Users).Returns(userRepositoryMock.Object);
<<<<<<< Updated upstream
            uowMock.Setup(u => u.Users.Get(It.IsAny<int>())).ReturnsAsync(new User() { Comments = new HashSet<Comment>() { new Comment() { Id = 3 } } });
            uowMock.Setup(c => c.Comments.Get(It.IsAny<int>())).Returns((Comment)null);
=======
            uowMock.Setup(u => u.Users.Get(It.IsAny<int>())).Returns(new User() { Comments = new HashSet<Comment>() { new Comment() { Id = 3 } } });
            uowMock.Setup(c => c.Comments.Get(It.IsAny<int>())).ReturnsAsync((Comment)null);
>>>>>>> Stashed changes

            //arrange
            string newText = "Hello";
            int commentId = 3;

            //act
            var result = commentService.UpdateCommentIdText(commentId, newText);

            //assert
            Assert.IsFalse(result);
        }
    }
}
