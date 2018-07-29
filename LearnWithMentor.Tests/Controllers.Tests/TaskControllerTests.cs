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
using System.ComponentModel.DataAnnotations;
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
        private Mock<IUserIdentityService> userIdentityServiceMock;

        [SetUp]
        public void SetUp()
        {
            var tasks = GetTestTasks();

            taskServiceMock = new Mock<ITaskService>();
            messageServiceMock = new Mock<IMessageService>();
            traceWriterMock = new Mock<ITraceWriter>();
            userIdentityServiceMock = new Mock<IUserIdentityService>();
            
            var userPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Role, "Admin")
            }));

            taskController = new TaskController(taskServiceMock.Object, messageServiceMock.Object, userIdentityServiceMock.Object, traceWriterMock.Object);
            taskController.ControllerContext.RequestContext.Principal = userPrincipal;
            taskController.Request = new HttpRequestMessage();
            taskController.Configuration = new HttpConfiguration();
            taskController.ControllerContext.ControllerDescriptor = new HttpControllerDescriptor(
                taskController.Configuration, "TaskController", taskController.GetType());
        }

        [TearDown]
        public void TearDown()
        {
            taskController.Dispose();
            taskServiceMock = null;
            messageServiceMock = null;
            traceWriterMock = null;
        }

        private List<TaskDTO> GetTestTasks()
        {
            var testTasks = new List<TaskDTO>
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
        private List<TaskDTO> GetTestTasksSearch(string[] lines)
        {
            var testTasks = GetTestTasks();
            var result = new List<TaskDTO>();
            foreach (var line in lines)
            {
                result.AddRange(testTasks.Where(t => t.Name.Contains(line)));
            }
            return result.Distinct().ToList();
        }
        public void ValidateViewModel<TModel, TController>(TController controller, TModel ModelToValidate)
        where TController : ApiController
        {
            var validationContext = new ValidationContext(ModelToValidate, null, null);
            var validationResults = new List<ValidationResult>();
            Validator.TryValidateObject(ModelToValidate, validationContext, validationResults, true);
            foreach (var validationResult in validationResults)
            {
                controller.ModelState.AddModelError(validationResult.MemberNames.FirstOrDefault() ?? string.Empty, validationResult.ErrorMessage);
            }
        }
        #region GetAllTasks
        [Test]
        public void GetAllTasksTest_ShouldReturnAllTasks()
        {
            taskServiceMock.Setup(mts => mts.GetAllTasks()).Returns(GetTestTasks());

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
            taskServiceMock.Setup(mts => mts.GetTaskById(It.IsAny<int>())).Returns(
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
            taskServiceMock.Setup(mts => mts.GetTaskById(It.IsAny<int>()));

            var task = GetTestTasks()[0];
            var response = taskController.GetTaskById(task.Id);
         
            Assert.AreEqual(response.StatusCode, HttpStatusCode.NoContent);
        }

        [Test]
        public void GetTaskByIdTest_ShouldCatchEntityException()
        {
            taskServiceMock.Setup(mts => mts.GetTaskById(It.IsAny<int>())).Throws(new EntityException());

            var task = GetTestTasks()[0];
            var response = taskController.GetTaskById(task.Id);

            Assert.AreEqual(response.StatusCode, HttpStatusCode.InternalServerError);
        }
        #endregion
        #region GetAllTasks
        [Test]
        public void GetTaskForPlanTest_ShouldReturnAllTasksForPlan()
        {
            taskServiceMock.Setup(mts => mts.GetTaskForPlan(It.IsAny<int>())).Returns(GetTestTasks()[0]);

            var response = taskController.GetTaskForPlan(1);
            var successfull = response.TryGetContentValue<TaskDTO>(out var taskDTOs);
            var expected = taskServiceMock.Object.GetTaskForPlan(1);
            var actual = taskDTOs;

            Assert.IsTrue(successfull);
            Assert.AreEqual(expected, actual);
            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);
        }

        [Test]
        public void GetTaskForPlanTest_ShouldReturnNoContentResponse()
        {
            taskServiceMock.Setup(ts => ts.GetTaskForPlan(It.IsAny<int>()));

            var response = taskController.GetTaskForPlan(1);

            Assert.AreEqual(response.StatusCode, HttpStatusCode.NoContent);
        }

        [Test]
        public void GetTaskForPlanTest_ShouldCatchEntityException()
        {
            taskServiceMock.Setup(ts => ts.GetTaskForPlan(It.IsAny<int>()))
                .Throws(new EntityException());

            var response = taskController.GetTaskForPlan(1);

            Assert.AreEqual(response.StatusCode, HttpStatusCode.InternalServerError);
        }
        #endregion
        #region Search
        [Test]
        public void SearchTest_ShouldReturnAllTasksByNullKey()
        {
            taskServiceMock.Setup(mts => mts.GetAllTasks()).Returns(GetTestTasks());

            var response = taskController.Search(null);
            var successfull = response.TryGetContentValue<List<TaskDTO>>(out var taskDTOs);
            var expected = taskServiceMock.Object.GetAllTasks().Count();
            var actual = taskDTOs.Count();

            Assert.IsTrue(successfull);
            Assert.AreEqual(expected, actual);
            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);
        }

        [Test]
        public void SearchTest_ShouldReturnTasksByKey()
        {
            taskServiceMock.Setup(mts => mts.Search(It.IsAny<string[]>())).Returns(
                (string[] lines) => GetTestTasksSearch(lines));

            var searchKey = "1";
            var response = taskController.Search(searchKey);
            var successfull = response.TryGetContentValue<List<TaskDTO>>(out var taskDTOs);
            var expected = taskServiceMock.Object.Search(new[] { searchKey });
            var actual = taskDTOs;

            Assert.IsTrue(successfull);
            Assert.AreEqual(expected.Count, actual.Count);
            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);
        }

        [Test]
        public void SearchTest_ShouldReturnNoContentResponse()
        {
            taskServiceMock.Setup(mts => mts.Search(It.IsAny<string[]>())).Returns(
                (string[] lines) => null);

            var searchKey = "1";
            var response = taskController.Search(searchKey);

            Assert.AreEqual(response.StatusCode, HttpStatusCode.NoContent);
        }

        [Test]
        public void SearchTest_ShouldCatchEntityException()
        {
            taskServiceMock.Setup(mts => mts.Search(It.IsAny<string[]>())).Throws(new EntityException());

            var searchKey = "1";
            var response = taskController.Search(searchKey);

            Assert.AreEqual(response.StatusCode, HttpStatusCode.InternalServerError);
        }
        #endregion
        #region PutNewUserTaskStatus
        [Test]
        public void PutNewUserTaskStatusTest_ShouldSuccessfullyPutNewStatus()
        {
            taskServiceMock.Setup(mts => mts.UpdateUserTaskStatus(It.IsAny<int>(), It.IsAny<string>()))
                .Returns(true);

            var response = taskController.PutNewUserTaskStatus(0, "D");

            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);
        }

        [Test]
        public void PutNewUserTaskStatusTest_ShouldCheckNotValidStateParameterAndReturnBadRequestResponse()
        {
            var response = taskController.PutNewUserTaskStatus(0, "status");

            Assert.AreEqual(response.StatusCode, HttpStatusCode.BadRequest);
        }

        [Test]
        public void PutNewUserTaskStatusTest_ShouldCheckNotSuccessfullUpdateTryAndReturnBadRequestResponse()
        {
            taskServiceMock.Setup(mts => mts.UpdateUserTaskStatus(It.IsAny<int>(), It.IsAny<string>()))
                .Returns(false);

            var response = taskController.PutNewUserTaskStatus(0, "D");

            Assert.AreEqual(response.StatusCode, HttpStatusCode.BadRequest);
        }

        [Test]
        public void PutNewUserTaskStatusTest_ShouldCatchEntityException()
        {
            taskServiceMock.Setup(mts => mts.UpdateUserTaskStatus(It.IsAny<int>(), It.IsAny<string>()))
                .Throws(new EntityException());

            var response = taskController.PutNewUserTaskStatus(0,"D");

            Assert.AreEqual(response.StatusCode, HttpStatusCode.InternalServerError);
        }
        #endregion
        #region PostTask
        [Test]
        public void PostTaskTest_ShouldSuccessfullyCreateNewTask()
        {
            
            taskServiceMock.Setup(mts => mts.CreateTask(It.IsAny<TaskDTO>()))
                .Returns(true);
            var newTask = GetTestTasks()[0];
            var response = taskController.Post(newTask);

            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);
        }

        [Test]
        public void PostTaskTest_ShouldCheckNotValidInputParameterAndReturnBadRequestResponse()
        {
            taskServiceMock.Setup(mts => mts.CreateTask(It.IsAny<TaskDTO>()))
                .Returns(true);

            var newTask = new TaskDTO{ };
            ValidateViewModel<TaskDTO, TaskController>(taskController, newTask);
            var response = taskController.Post(newTask);

            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Test]
        public void PostTaskTest_ShouldCheckNotSuccessfullPostTryAndReturnBadRequestResponse()
        {
            taskServiceMock.Setup(mts => mts.CreateTask(It.IsAny<TaskDTO>()))
                .Returns(false);

            var newTask = GetTestTasks()[0];

            var response = taskController.Post(newTask);

            Assert.AreEqual(response.StatusCode, HttpStatusCode.BadRequest);
        }

        [Test]
        public void PostTaskTest_ShouldCatchEntityException()
        {
            taskServiceMock.Setup(mts => mts.CreateTask(It.IsAny<TaskDTO>()))
                .Throws(new EntityException());

            var newTask = GetTestTasks()[0];
            var response = taskController.Post(newTask);

            Assert.AreEqual(response.StatusCode, HttpStatusCode.InternalServerError);
        }
        #endregion
        #region PostAndReturnId
        [Test]
        public void PostAndReturnIdTest_ShouldSuccessfullyCreateNewTaskAndReturnItsId()
        {
            var returnId = 1;
            taskServiceMock.Setup(mts => mts.AddAndGetId(It.IsAny<TaskDTO>()))
                .Returns(returnId);

            var response = taskController.PostAndReturnId(GetTestTasks()[0]);
            var successfull = response.TryGetContentValue<int>(out var taskId);
            var expected = taskServiceMock.Object.AddAndGetId(GetTestTasks()[0]);
            var actual = taskId;

            Assert.IsTrue(successfull);
            Assert.AreEqual(expected, actual);
            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);
        }

        [Test]
        public void PostAndReturnIdTest_ShouldCheckNotValidInputAndReturnBadRequestResponse()
        {
            taskServiceMock.Setup(mts => mts.AddAndGetId(It.IsAny<TaskDTO>()))
                .Returns(1);

            var newTask = new TaskDTO { };
            ValidateViewModel<TaskDTO, TaskController>(taskController, newTask);
            var response = taskController.PostAndReturnId(newTask);

            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Test]
        public void PostAndReturnIdTest_ShouldCheckNotSuccessfullPostTryAndReturnBadRequestResponse()
        {
            taskServiceMock.Setup(mts => mts.AddAndGetId(It.IsAny<TaskDTO>()))
                .Returns(() => null);

            var response = taskController.PostAndReturnId(GetTestTasks()[0]);

            Assert.AreEqual(response.StatusCode, HttpStatusCode.BadRequest);
        }

        [Test]
        public void PostAndReturnIdTest_ShouldCatchEntityException()
        {
            taskServiceMock.Setup(mts => mts.AddAndGetId(It.IsAny<TaskDTO>()))
                .Throws(new EntityException());

            var response = taskController.PostAndReturnId(GetTestTasks()[0]);

            Assert.AreEqual(response.StatusCode, HttpStatusCode.InternalServerError);
        }
        #endregion
        #region Delete
        [Test]
        public void DeleteTest_ShouldSuccessfullyDeleteNewTask()
        {
            taskServiceMock.Setup(mts => mts.RemoveTaskById(It.IsAny<int>()))
                .Returns(true);

            var response = taskController.Delete(1);

            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);
        }
        
        [Test]
        public void DeleteTest_ShouldCheckNotSuccessfullDeleteTryAndReturnBadRequestResponse()
        {
            taskServiceMock.Setup(mts => mts.RemoveTaskById(It.IsAny<int>()))
                .Returns(false);

            var response = taskController.Delete(1);

            Assert.AreEqual(response.StatusCode, HttpStatusCode.BadRequest);
        }

        [Test]
        public void DeleteTest_ShouldCatchEntityException()
        {
            taskServiceMock.Setup(mts => mts.RemoveTaskById(It.IsAny<int>()))
                .Throws(new EntityException());

            var response = taskController.Delete(1);

            Assert.AreEqual(response.StatusCode, HttpStatusCode.InternalServerError);
        }
        #endregion
    }
}
