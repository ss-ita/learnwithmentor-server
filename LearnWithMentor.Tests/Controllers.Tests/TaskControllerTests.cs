using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Net;
using System.Net.Http;
using System.Web.Http.Tracing;
using NUnit.Framework;
using Moq;
using LearnWithMentor.Controllers;
using LearnWithMentorBLL.Services;
using LearnWithMentorBLL.Interfaces;
using LearnWithMentorDTO;

namespace LearnWithMentor.Tests.Controllers.Tests
{
    [TestFixture]
    public class TaskControllerTests
    {
        public readonly IList<TaskDTO> Tasks;
        public readonly ITaskService MockTaskService;
        public readonly IMessageService MockMessageService;
        public readonly ITraceWriter MockTraceWriter;
        public TaskControllerTests()
        {
            IList<TaskDTO> tasks = new List<TaskDTO>
            {
                new TaskDTO(1, "Task #1", "Task #1", false, 1, "Task #1 creator", 1,
                            "Task #1 creator", DateTime.Now, DateTime.Now, 1, 1, 1),
                new TaskDTO(2, "Task #2", "Task #2", false, 1, "Task #2 creator", 1,
                            "Task #2 creator", DateTime.Now, DateTime.Now, 1, 1, 1),
                new TaskDTO(3, "Task #3", "Task #3", false, 1, "Task #3 creator", 1,
                            "Task #3 creator", DateTime.Now, DateTime.Now, 1, 1, 1),
            };
            this.Tasks = tasks;
            Mock<ITaskService> mockTaskService = new Mock<ITaskService>();
            Mock<IMessageService> mockMessageService = new Mock<IMessageService>();
            Mock<ITraceWriter> mockTraceWriter = new Mock<ITraceWriter>();

            mockTaskService.Setup(mtc => mtc.GetAllTasks()).Returns(Tasks);
            mockTaskService.Setup(mtc => mtc.GetTaskById(
               It.IsAny<int>())).Returns((int i) => tasks.Where(
               x => x.Id == i).Single());

            this.MockTaskService = mockTaskService.Object;
            this.MockMessageService = mockMessageService.Object;
            this.MockTraceWriter = mockTraceWriter.Object;
        }

        [Test]
        public void GetAllTasksTest()
        {
            // Arrange
            var controller = new TaskController(MockTaskService, MockMessageService, MockTraceWriter);
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            // Act
            var response = controller.GetAllTasks();

            // Assert
            IList<TaskDTO> resultTasks;
            Assert.IsTrue(response.TryGetContentValue(out resultTasks));
            Assert.AreEqual(Tasks, resultTasks);
            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);
        }

        [Test]
        public void GetTaskByIdTest()
        {
            // Arrange
            var task = Tasks[0];
            var controller = new TaskController(MockTaskService, MockMessageService, MockTraceWriter);
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            // Act
            var response = controller.GetTaskById(task.Id);

            // Assert
            TaskDTO resultTask;
            Assert.IsTrue(response.TryGetContentValue(out resultTask));
            Assert.AreEqual(task, resultTask);
            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);
        }
    }
}
