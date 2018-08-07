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
using System.Text;
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
            taskServiceMock = new Mock<ITaskService>();
            messageServiceMock = new Mock<IMessageService>();
            traceWriterMock = new Mock<ITraceWriter>();
            userIdentityServiceMock = new Mock<IUserIdentityService>();

            var userPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Role, "Admin"),
                new Claim("Id", "1")
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

        private List<MessageDTO> GetTestMessage()
        {
            var testMessages = new List<MessageDTO>
            {
                new MessageDTO(1, 1, 1, "petro", "good result", DateTime.Now),
                new MessageDTO(2, 1, 1, "petro",  new string('*',5000), DateTime.Now)
            };
            return testMessages;
        }

        private List<UserTaskDTO> GetTestUserTasks()
        {
            var testUserTasks = new List<UserTaskDTO>
            {
                new UserTaskDTO(1, 1 , 1, DateTime.Now, DateTime.Now, 1),
                new UserTaskDTO(2, 1 , 2, DateTime.Now, DateTime.Now, 1),
                new UserTaskDTO(3, 1 , 3, DateTime.Now, DateTime.Now, 1),
                new UserTaskDTO(4, 2 , 1, DateTime.Now, DateTime.Now, 1),
                new UserTaskDTO(5, 2 , 2, DateTime.Now, DateTime.Now, 1),
                new UserTaskDTO(6, 2 , 3, DateTime.Now, DateTime.Now, 1)
            };
            return testUserTasks;
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
        public void ValidateViewModel<TModel, TController>(TController controller, TModel modelToValidate)
        where TController : ApiController
        {
            var validationContext = new ValidationContext(modelToValidate, null, null);
            var validationResults = new List<ValidationResult>();
            Validator.TryValidateObject(modelToValidate, validationContext, validationResults, true);
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
                (int i) => GetTestTasks().Single(x => x.Id == i));

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

            var response = taskController.PutNewUserTaskStatus(0, "D");

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

            var newTask = new TaskDTO();
            ValidateViewModel(taskController, newTask);
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

            var newTask = new TaskDTO();
            ValidateViewModel(taskController, newTask);
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

        #region DeleteProposeEndDate
        [Test]
        public void DeleteProposeEndDateTest_ShouldSuccessfullyDeleteProposeEndDate()
        {
            taskServiceMock.Setup(mts => mts.DeleteProposeEndDate(It.IsAny<int>()))
                .Returns(true);

            var response = taskController.DeleteProposeEndDate(1);

            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);
        }

        [Test]
        public void DeleteProposeEndDateTest_ShouldReturnNoContent()
        {
            taskServiceMock.Setup(mts => mts.DeleteProposeEndDate(It.IsAny<int>()))
                .Returns(false);

            var response = taskController.DeleteProposeEndDate(1);

            Assert.AreEqual(response.StatusCode, HttpStatusCode.NoContent);
        }

        [Test]
        public void DeleteProposeEndDateTest_ShouldCatchEntityException()
        {
            taskServiceMock.Setup(mts => mts.DeleteProposeEndDate(It.IsAny<int>()))
                .Throws(new EntityException());

            var response = taskController.DeleteProposeEndDate(1);

            Assert.AreEqual(response.StatusCode, HttpStatusCode.InternalServerError);
        }


        #endregion
        #region GetUsersTasks
        [Test]
        public void GetUsersTasksTest_ShouldReturnAllUserTasks()
        {
            taskServiceMock.Setup(mts => mts.GetTaskStatesForUser(It.IsAny<int[]>(), It.IsAny<int>())).Returns(
                (int[] i, int user) => GetTestUserTasks().Where( x => x.UserId == user ).ToList());

            var task = GetTestUserTasks()[0];
            var response = taskController.GetUsersTasks(new []{task.PlanTaskId}, new[]{ task.UserId });
            var successfull = response.TryGetContentValue<List<ListUserTasksDTO>>(out var taskDTO);
            var expected = taskServiceMock.Object.GetTaskStatesForUser(new[] { task.PlanTaskId }, task.UserId);
            var actual = taskDTO;

            Assert.IsTrue(successfull);
            Assert.AreEqual(expected[0].Id, actual[0].UserTasks[0].Id);
            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);
        }

        [Test]
        public void GetUsersTasksTest_ShouldReturnNoContentResponse()
        {
            taskServiceMock.Setup(mts => mts.GetTaskStatesForUser(It.IsAny<int[]>(), It.IsAny<int>()));

            var usertask = GetTestUserTasks()[0];
            var response = taskController.GetUsersTasks(new[] { usertask.PlanTaskId}, new[] { usertask.UserId});

            Assert.AreEqual(response.StatusCode, HttpStatusCode.NoContent);
        }

        [Test]
        public void GetUsersTasksTest_ShouldCatchEntityException()
        {
            taskServiceMock.Setup(mts => mts.GetTaskStatesForUser(It.IsAny<int[]>(), It.IsAny<int>())).Throws(new EntityException());

            var usertask = GetTestUserTasks()[0];
            var response = taskController.GetUsersTasks(new[] { usertask.PlanTaskId }, new[] { usertask.UserId });

            Assert.AreEqual(response.StatusCode, HttpStatusCode.InternalServerError);
        }


        #endregion
        #region GetUserTask

        [Test]
        public void GetUserTaskTest_ShouldReturnTask()
        {
            userIdentityServiceMock.Setup(u => u.GetUserId()).Returns(1);
            userIdentityServiceMock.Setup(u => u.GetUserRole()).Returns("Mentor");
            taskServiceMock.Setup(mts => mts.GetUserTaskByUserPlanTaskId(It.IsAny<int>(), It.IsAny<int>())).Returns(
                (int user, int plantask) => GetTestUserTasks().Single(x => x.UserId == user && x.PlanTaskId == plantask));

            var usertask = GetTestUserTasks()[0];
            var response = taskController.GetUserTask(usertask.PlanTaskId,usertask.UserId);
            var successfull = response.TryGetContentValue<UserTaskDTO>(out var usertaskDTO);
            var expected = taskServiceMock.Object.GetUserTaskByUserPlanTaskId(usertask.UserId, usertask.PlanTaskId);
            var actual = usertaskDTO;

            Assert.IsTrue(successfull);
            Assert.AreEqual(expected.Id, actual.Id);
            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);
        }

        [Test]
        public void GetUserTaskTest_ShouldReturnNoContentResponse()
        {
            userIdentityServiceMock.Setup(u => u.GetUserId()).Returns(1);
            userIdentityServiceMock.Setup(u => u.GetUserRole()).Returns("Mentor");
            taskServiceMock.Setup(mts => mts.GetUserTaskByUserPlanTaskId(It.IsAny<int>(), It.IsAny<int>()));

            var usertask = GetTestUserTasks()[0];
            var response = taskController.GetUserTask(usertask.PlanTaskId, usertask.UserId);

            Assert.AreEqual(response.StatusCode, HttpStatusCode.NoContent);
        }

        [Test]
        public void GetUserTaskTest_ShouldReturnUnauthorized()
        {
            userIdentityServiceMock.Setup(u => u.GetUserId());
            userIdentityServiceMock.Setup(u => u.GetUserRole());
            taskServiceMock.Setup(mts => mts.GetUserTaskByUserPlanTaskId(It.IsAny<int>(), It.IsAny<int>()));

            var usertask = GetTestUserTasks()[0];
            var response = taskController.GetUserTask(usertask.PlanTaskId, usertask.UserId);

            Assert.AreEqual(response.StatusCode, HttpStatusCode.Unauthorized);
        }

        [Test]
        public void GetUserTaskTest_ShouldCatchEntityException()
        {
            userIdentityServiceMock.Setup(u => u.GetUserId()).Returns(1);
            userIdentityServiceMock.Setup(u => u.GetUserRole()).Returns("Mentor");
            taskServiceMock.Setup(mts => mts.GetUserTaskByUserPlanTaskId(It.IsAny<int>(), It.IsAny<int>())).Throws(new EntityException());

            var usertask = GetTestUserTasks()[0];
            var response = taskController.GetUserTask(usertask.PlanTaskId, usertask.UserId);

            Assert.AreEqual(response.StatusCode, HttpStatusCode.InternalServerError);
        }


        #endregion
        #region SetNewEndDate
        [Test]
        public void SetNewEndDateTest_ShouldSuccesFullyChangingEndDate()
        {
            taskServiceMock.Setup(mts => mts.SetNewEndDate(It.IsAny<int>()))
                .Returns(true);

            var response = taskController.SetNewEndDate(1);

            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);
        }

        [Test]
        public void SetNewEndDateTest_ShouldReturnNoContent()
        {
            taskServiceMock.Setup(mts => mts.SetNewEndDate(It.IsAny<int>()))
                .Returns(false);

            var response = taskController.SetNewEndDate(1);

            Assert.AreEqual(response.StatusCode, HttpStatusCode.NoContent);
        }

        [Test]
        public void SetNewEndDateTest_ShouldCatchEntityException()
        {
            taskServiceMock.Setup(mts => mts.SetNewEndDate(It.IsAny<int>()))
                .Throws(new EntityException());

            var response = taskController.SetNewEndDate(1);

            Assert.AreEqual(response.StatusCode, HttpStatusCode.InternalServerError);
        }



        #endregion
        #region PutNewUserTaskResult
        [Test]
        public void PutNewUserTaskResultTest_ShouldSuccessfullyPutNewResult()
        {
            taskServiceMock.Setup(mts => mts.UpdateUserTaskResult(It.IsAny<int>(), It.IsAny<string>()))
                .Returns(true);
            var httpRequestMessage = new HttpRequestMessage();
            httpRequestMessage.Content =
                new StringContent("Do not touch this ****", Encoding.UTF8, "application/json");
            var response = taskController.PutNewUserTaskResult(0, httpRequestMessage);

            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);
        }

        [Test]
        public void PutNewUserTaskResultTest_ShouldCheckNotValidStateParameterAndReturnBadRequestResponse()
        {
            var message = new string('*', 5000);
            var httpRequestMessage = new HttpRequestMessage();
            httpRequestMessage.Content =
                new StringContent(message, Encoding.UTF8, "application/json");
            var response = taskController.PutNewUserTaskResult(0, httpRequestMessage);

            Assert.AreEqual(response.StatusCode, HttpStatusCode.BadRequest);
        }

        [Test]
        public void PutNewUserTaskResultTest_ShouldCheckNotSuccessfullUpdateTryAndReturnBadRequestResponse()
        {
            taskServiceMock.Setup(mts => mts.UpdateUserTaskResult(It.IsAny<int>(), It.IsAny<string>()))
                .Returns(false);

            var httpRequestMessage = new HttpRequestMessage();
            httpRequestMessage.Content =
                new StringContent("Do not touch this ****", Encoding.UTF8, "application/json");
            var response = taskController.PutNewUserTaskResult(0, httpRequestMessage);

            Assert.AreEqual(response.StatusCode, HttpStatusCode.BadRequest);
        }

        [Test]
        public void PutNewUserTaskResultTest_ShouldCatchEntityException()
        {
            taskServiceMock.Setup(mts => mts.UpdateUserTaskResult(It.IsAny<int>(), It.IsAny<string>()))
                .Throws(new EntityException());

            var httpRequestMessage = new HttpRequestMessage();
            httpRequestMessage.Content =
                new StringContent("Do not touch this ****", Encoding.UTF8, "application/json");
            var response = taskController.PutNewUserTaskResult(0, httpRequestMessage);

            Assert.AreEqual(response.StatusCode, HttpStatusCode.InternalServerError);
        }


        #endregion
        #region PostNewUserTask
        [Test]
        public void PostNewUserTaskTest_ShouldSuccessfullyCreateNewUserTask()
        {

            taskServiceMock.Setup(mts => mts.CreateUserTask(It.IsAny<UserTaskDTO>()))
                .Returns(true);
            var newTask = GetTestUserTasks()[0];
            var response = taskController.PostNewUserTask(newTask);

            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);
        }

        [Test]
        public void PostNewUserTaskTest_ShouldCheckNotValidInputParameterAndReturnBadRequestResponse()
        {
            taskServiceMock.Setup(mts => mts.CreateUserTask(It.IsAny<UserTaskDTO>()))
                .Returns(true);

            var newUserTask = new UserTaskDTO();
            ValidateViewModel(taskController, newUserTask);
            var response = taskController.PostNewUserTask(newUserTask);

            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Test]
        public void PostNewUserTaskTest_ShouldCheckNotSuccessfullPostTryAndReturnNoContentResponse()
        {
            taskServiceMock.Setup(mts => mts.CreateUserTask(It.IsAny<UserTaskDTO>()))
                .Returns(false);

            var newUserTask = GetTestUserTasks()[0];

            var response = taskController.PostNewUserTask(newUserTask);

            Assert.AreEqual(response.StatusCode, HttpStatusCode.NoContent);
        }

        [Test]
        public void PostNewUserTaskTest_ShouldCatchEntityException()
        {
            taskServiceMock.Setup(mts => mts.CreateUserTask(It.IsAny<UserTaskDTO>()))
                .Throws(new EntityException());

            var newUserTask = GetTestUserTasks()[0];
            var response = taskController.PostNewUserTask(newUserTask);

            Assert.AreEqual(response.StatusCode, HttpStatusCode.InternalServerError);
        }


        #endregion
        #region PostUserTaskMessage
        [Test]
        public void PostUserTaskMessageTest_ShouldSuccessfullyCreateUserTaskMessage()
        {
            userIdentityServiceMock.Setup(u => u.GetUserId()).Returns(1);
            messageServiceMock.Setup(mts => mts.SendMessage(It.IsAny<MessageDTO>()))
                .Returns(true);

            var message = GetTestMessage()[0];
            var response = taskController.PostUserTaskMessage(message.UserTaskId,message);

            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);
        }

        [Test]
        public void PostUserTaskMessageTest_ShouldCheckNotValidInputParameterAndReturnBadRequestResponse()
        {
            userIdentityServiceMock.Setup(u => u.GetUserId()).Returns(1);
            messageServiceMock.Setup(mts => mts.SendMessage(It.IsAny<MessageDTO>()))
                .Returns(true);

            var message = GetTestMessage()[1];
            ValidateViewModel(taskController, message);
            var response = taskController.PostUserTaskMessage(message.UserTaskId, message);

            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Test]
        public void PostUserTaskMessageTest_ShouldCheckNotSuccessfullPostTryAndReturnBadRequestResponse()
        {
            userIdentityServiceMock.Setup(u => u.GetUserId()).Returns(1);
            messageServiceMock.Setup(mts => mts.SendMessage(It.IsAny<MessageDTO>()))
                .Returns(false);

            var message = GetTestMessage()[0];
            var response = taskController.PostUserTaskMessage(message.UserTaskId, message);

            Assert.AreEqual(response.StatusCode, HttpStatusCode.BadRequest);
        }

        [Test]
        public void PostUserTaskMessageTest_ShouldCatchEntityException()
        {
            userIdentityServiceMock.Setup(u => u.GetUserId()).Returns(1);
            messageServiceMock.Setup(mts => mts.SendMessage(It.IsAny<MessageDTO>()))
                .Throws(new EntityException());

            var message = GetTestMessage()[0];
            var response = taskController.PostUserTaskMessage(message.UserTaskId, message);

            Assert.AreEqual(response.StatusCode, HttpStatusCode.InternalServerError);
        }


        #endregion
    }
}
