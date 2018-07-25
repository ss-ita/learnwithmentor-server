using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Security.Claims;
using System.Net.Http;
using System.Web.Http.Tracing;
using System.Data.Entity.Core;
using NUnit.Framework;
using Moq;
using LearnWithMentor.Controllers;
using LearnWithMentorBLL.Interfaces;
using LearnWithMentorDTO;

namespace LearnWithMentor.Tests.Controllers.Tests
{
    [TestFixture]
    public class TaskControllerTests
    {
        private TaskController taskController;
        private Mock<ITaskService> taskServiceMock;
        private Mock<IMessageService> messageServiceMock;
        private Mock<ITraceWriter> traceWriterMock;

        [OneTimeSetUp]
        public void SetUp()
        {
            var tasks = GetTestTasks();

            taskServiceMock = new Mock<ITaskService>();
            messageServiceMock = new Mock<IMessageService>();
            traceWriterMock = new Mock<ITraceWriter>();
            
            var userPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Role, "Admin")
            }));

            taskController = new TaskController(taskServiceMock.Object, messageServiceMock.Object, traceWriterMock.Object);
            taskController.ControllerContext.RequestContext.Principal = userPrincipal;
            taskController.Request = new HttpRequestMessage();
            taskController.Configuration = new HttpConfiguration();
            taskController.ControllerContext.ControllerDescriptor = new HttpControllerDescriptor(
                taskController.Configuration, "TaskController", taskController.GetType());
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            taskController.Dispose();
            taskServiceMock = null;
            messageServiceMock = null;
            traceWriterMock = null;
        }

        private List<TaskDTO> GetTestTasks()
        {
            List<TaskDTO> testTasks = new List<TaskDTO>
            {
                new TaskDTO(1, "Task #1", "Task #1", false, 1, "Task #1 creator", 1,
                            "Task #1 creator", DateTime.Now, DateTime.Now, 1, 1, 1),
                new TaskDTO(2, "Task #2", "Task #2", false, 1, "Task #2 creator", 1,
                            "Task #2 creator", DateTime.Now, DateTime.Now, 1, 1, 1),
                new TaskDTO(3, "Task #3", "Task #3", false, 1, "Task #3 creator", 1,
                            "Task #3 creator", DateTime.Now, DateTime.Now, 1, 1, 1),
            };

            return testTasks;
        }
        #region GetAllTasks
        [Test]
        public void GetAllTasksTest_ShouldReturnAllTasks()
        {
            taskServiceMock.Setup(mtc => mtc.GetAllTasks()).Returns(GetTestTasks());

            var response = taskController.GetAllTasks();
            var successfull = response.TryGetContentValue<IEnumerable<TaskDTO>>(out var taskDTOs);
            var expected = taskServiceMock.Object.GetAllTasks().Count();
            var actual = taskDTOs.Count();

            Assert.IsTrue(successfull);
            Assert.AreEqual(expected, actual);
            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);
        }

        [Test]
        public void GetAllTasksTest_ShouldReturnNoContentResponse()
        {
            taskServiceMock.Setup(ts => ts.GetAllTasks());

            var response = taskController.GetAllTasks();

            Assert.AreEqual(response.StatusCode, HttpStatusCode.NoContent);
        }

        [Test]
        public void GetAllTasksTest_ShouldCatchEntityException()
        {
            taskServiceMock.Setup(ts => ts.GetAllTasks()).Throws(new EntityException());

            var response = taskController.GetAllTasks();

            Assert.AreEqual(response.StatusCode, HttpStatusCode.InternalServerError);
        }
        #endregion
        #region GetTaskById
        [Test]
        public void GetTaskByIdTest_ShouldReturnTask()
        {
            taskServiceMock.Setup(mtc => mtc.GetTaskById(It.IsAny<int>())).Returns(
                (int i) => GetTestTasks().Where(x => x.Id == i).Single());

            var task = GetTestTasks()[0];
            var response = taskController.GetTaskById(task.Id);
            var successfull = response.TryGetContentValue<TaskDTO>(out var taskDTO);
            var expected = taskServiceMock.Object.GetTaskById(task.Id);
            var actual = taskDTO;

            Assert.IsTrue(successfull);
            Assert.AreEqual(expected.Id, actual.Id);
            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);
        }

        [Test]
        public void GetTaskByIdTest_ShouldReturnNoContentResponse()
        {
            taskServiceMock.Setup(mtc => mtc.GetTaskById(It.IsAny<int>()));

            var task = GetTestTasks()[0];
            var response = taskController.GetTaskById(task.Id);
         
            Assert.AreEqual(response.StatusCode, HttpStatusCode.NoContent);
        }

        [Test]
        public void GetTaskByIdTest_ShouldCatchEntityException()
        {
            taskServiceMock.Setup(mtc => mtc.GetTaskById(It.IsAny<int>())).Throws(new EntityException());

            var task = GetTestTasks()[0];
            var response = taskController.GetTaskById(task.Id);

            Assert.AreEqual(response.StatusCode, HttpStatusCode.InternalServerError);
        }
        #endregion
    }
}
