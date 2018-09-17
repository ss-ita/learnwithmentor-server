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
    class CommentServiceTests
    {
        private CommentService commentService;
        private Mock<LearnWithMentor_DBEntities> dbContextMock;
        private Mock<UnitOfWork> uowMock;
        private Mock<CommentRepository> commentRepositoryMock;
        private Mock<UserRepository> userRepositoryMock;
        private User creator;

        [SetUp]
        public void SetUp()
        {
            dbContextMock = new Mock<LearnWithMentor_DBEntities>();
            commentRepositoryMock = new Mock<CommentRepository>(dbContextMock.Object);
            userRepositoryMock = new Mock<UserRepository>(dbContextMock.Object);
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
        public void GetCommentById_ShouldReturnCommentById()
        {
            uowMock.SetupGet(c => c.Comments).Returns(commentRepositoryMock.Object);
            uowMock.SetupGet(u => u.Users).Returns(userRepositoryMock.Object);
            uowMock.Setup(u => u.Users.Get(It.IsAny<int>())).Returns(new User() { Comments = new HashSet<Comment>(){ new Comment() { Id = 3 } } });
            uowMock.Setup(c => c.Comments.Get(It.IsAny<int>())).Returns(new Comment() { Id = 3, Creator= new User() { Comments = new HashSet<Comment>() { new Comment() { Id = 3 } } } });

            //arrange
            int commentId = 3;

            //act
            var result = commentService.GetComment(commentId);

            //assert
            Assert.IsNotNull(result);
            Assert.AreEqual(commentId, result.Id);
            Assert.AreSame(result.GetType(), typeof(CommentDTO));
        }
    }
}
