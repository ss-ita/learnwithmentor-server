using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Data.Entity.Core;
using System.Security.Claims;
using NUnit.Framework;
using Moq;
using LearnWithMentor.Controllers;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Results;
using System.Web.Http.Tracing;
using LearnWithMentorBLL.Interfaces;
using LearnWithMentorDTO;


namespace LearnWithMentor.Tests.Controllers.Tests
{

    [TestFixture]
    public class PlanControllerTests
    {

       
        private PlanController planController;
        private Mock<IPlanService> planServiceMock;
        private List<PlanDTO> plans;
        private Mock<ITraceWriter> traceWriterMock;
        private Mock<IUserService> userServiceMock;
        private Mock<ITaskService> taskServiceMock;
        private Mock<IUserIdentityService> userIdentityServiceMock;


        [OneTimeSetUp]
        public void SetUp()
        {
            plans = new List<PlanDTO>()
            {
                new PlanDTO(1, "name1","description1",true,1,"nameCreator1","lastenameCreator1",1,"nameCreator1","lastenameCreator1", DateTime.Now, DateTime.Now),
                new PlanDTO(2, "name2", "description2",true,2,"nameCreator2","lastenameCreator2",2,"nameCreator1","lastenameCreator1",DateTime.Now, DateTime.Now),
                new PlanDTO(3, "name3", "description3",true,3,"nameCreator3","lastenameCreator3",3,"nameCreator1","lastenameCreator1",DateTime.Now, DateTime.Now),
                new PlanDTO(4, "name4", "description4",true,4,"nameCreator4","lastenameCreator4",4,"nameCreator1","lastenameCreator1",DateTime.Now, DateTime.Now)
            };

            planServiceMock = new Mock<IPlanService>();
            traceWriterMock = new Mock<ITraceWriter>();
            taskServiceMock = new Mock<ITaskService>();
            userServiceMock = new Mock<IUserService>();
            userIdentityServiceMock = new Mock<IUserIdentityService>();

            var userPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Role, "Admin")
            }));

            planController = new PlanController(planServiceMock.Object, taskServiceMock.Object, traceWriterMock.Object);
            planController.ControllerContext.RequestContext.Principal = userPrincipal;
            planController.Request = new HttpRequestMessage();
            planController.Configuration = new HttpConfiguration();
            planController.ControllerContext.ControllerDescriptor = new HttpControllerDescriptor(
                planController.Configuration, "PlanController", planController.GetType());
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            planController.Dispose();
            planServiceMock = null;
            taskServiceMock = null;
            userIdentityServiceMock = null;
            traceWriterMock = null;
            plans = null;
        }

        private List<PlanDTO> GetTestPlansSearch(string[] lines)
        {
           
            var result = new List<PlanDTO>();
            foreach (var line in lines)
            {
                result.AddRange(plans.Where(t => t.Name.Contains(line)));
            }
            return result.Distinct().ToList();
        }

        [Test]
        public void GetAllPlansTest_ShouldReturnAllPlans()
        {
            planServiceMock.Setup(mts => mts.GetAll()).Returns(plans);

            var response = planController.Get();
            var successfull = response.TryGetContentValue<IEnumerable<PlanDTO>>(out var planDTOs);
            var expected = planServiceMock.Object.GetAll().Count();
            var actual = planDTOs.Count();

            Assert.IsTrue(successfull);
            Assert.AreEqual(expected, actual);
            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);
        }

        [Test]
        public void GetTasksForPlanTest_ShouldReturnNoContentMessage()
        {
            planServiceMock.Setup(mts => mts.GetTasksForPlan(It.IsAny<int>()))
                .Returns(()=>null);

            var response = planController.GetTasksForPlan(1);            

            Assert.AreEqual(response.StatusCode, HttpStatusCode.NoContent);
        }

        [Test]
        public void UpdatePlanTest_ShouldReturnSuccess()
        {
            planServiceMock.Setup(u => u.UpdateById( It.IsAny<PlanDTO>(), It.IsAny<int>())).Returns(true);

            PlanDTO forUpdating = new PlanDTO(1, "name1", "description1", true, 1, "nameCreator1", "lastenameCreator1", 1, "nameCreator1", "lastenameCreator1", DateTime.Now, DateTime.Now);
            var response = planController.Put(1, forUpdating);
            var expectedStatusCode = HttpStatusCode.OK;
            var actualStatusCode = response.StatusCode;

            Assert.AreEqual(expectedStatusCode, actualStatusCode);
        }

        [Test]
        public void UpdatePlanTest_ShouldReturnBadRequestMessage()
        {
            planServiceMock.Setup(u => u.UpdateById(It.IsAny<PlanDTO>(), It.IsAny<int>())).Returns(false);

            PlanDTO forUpdating = new PlanDTO(1, "name1", "description1", true, 1, "nameCreator1", "lastenameCreator1", 1, "nameCreator1", "lastenameCreator1", DateTime.Now, DateTime.Now);
            var response = planController.Put(1, forUpdating);
            var expectedStatusCode = HttpStatusCode.BadRequest;
            var actualStatusCode = response.StatusCode;

            Assert.AreEqual(expectedStatusCode, actualStatusCode);
        }

        [Test]
        public void UpdatePlanTest_ShouldCatchEntityExeption()
        {
            planServiceMock.Setup(u => u.UpdateById(It.IsAny<PlanDTO>(), It.IsAny<int>()))
                .Throws(new EntityException());

            PlanDTO forUpdating = new PlanDTO(1, "name1", "description1", true, 1, "nameCreator1", "lastenameCreator1", 1, "nameCreator1", "lastenameCreator1", DateTime.Now, DateTime.Now);
            var response = planController.Put(1, forUpdating);
            var expectedStatusCode = HttpStatusCode.InternalServerError;
            var actualStatusCode = response.StatusCode;

            Assert.AreEqual(expectedStatusCode, actualStatusCode);
        }

        [Test]
        public void GetSomeTest_ShouldReturnNoContentMessage()
        {
            planServiceMock.Setup(mts => mts.GetSomeAmount(It.IsAny<int>(), It.IsAny<int>()))
                .Returns( ()=> null);

            var response = planController.GetSome(1,1);

            Assert.AreEqual(HttpStatusCode.NoContent,response.StatusCode);
        }

        [Test]
        public void GetSomeTest_ShouldReturnListOfPlans()
        {
            planServiceMock.Setup(mts => mts.GetSomeAmount(It.IsAny<int>(), It.IsAny<int>()))
                .Returns(plans);

            var response = planController.GetSome(1, 1);

            Assert.AreEqual( HttpStatusCode.OK,response.StatusCode);
        }


        [Test]
        public void GetAllTasksAssignedToPlanTest_ShouldReturnNoContentMessage()
        {
            planServiceMock.Setup(mts => mts.GetAllTasks(It.IsAny<int>()))
                .Returns(()=>null);

            var response = planController.GetAllTasks(1);

            Assert.AreEqual(HttpStatusCode.NoContent, response.StatusCode);
        }

        
        [Test]
        public void GetAllPlansTest_ShouldReturnNoContentResponse()
        {
            planServiceMock.Setup(ps => ps.GetAll());

            var response = planController.Get();

            Assert.AreEqual(response.StatusCode, HttpStatusCode.NoContent);
        }

        [Test]
        public void GetPlanByIdTest_ShouldReturnPlan()
        {
            planServiceMock.Setup(mts => mts.Get(It.IsAny<int>())).Returns(
                (int i) => plans.Where(x => x.Id == i).Single());

            var plan = plans[0];
            var response = planController.Get(plan.Id);
            var successfull = response.TryGetContentValue<PlanDTO>(out var planDTO);
            var expected = planServiceMock.Object.Get(plan.Id);
            var actual = planDTO;

            Assert.IsTrue(successfull);
            Assert.AreEqual(expected.Id, actual.Id);
            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);
        }

        [Test]
        public void GetPlanByIdTest_ShouldReturnNoContentResponse()
        {
            planServiceMock.Setup(mts => mts.Get(It.IsAny<int>()));

            var plan = plans[0];
            var response = planController.Get(plan.Id);

            Assert.AreEqual(response.StatusCode, HttpStatusCode.NoContent);
        }

        [Test]
        public void PostPlanTest_ShouldSuccessfullyCreateNewPlan()
        {

            planServiceMock.Setup(mts => mts.Add(It.IsAny<PlanDTO>())).Returns(true);
            var newPlan = plans[0];
            var response = planController.Post(newPlan);

            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);
        }

        [Test]
        public void PostImageTest_ShouldReturnNoContentResponse()
        {
            planServiceMock.Setup(u => u.ContainsId(It.IsAny<int>())).Returns(false);
            var response = planController.PostImage(1);

            Assert.AreEqual(HttpStatusCode.NoContent,response.StatusCode);
        }
        
        [Test]
        public void PostPlanTest_ShouldCheckNotSuccessfullPostTryAndReturnBadRequestResponse()
        {
            planServiceMock.Setup(mts => mts.Add(It.IsAny<PlanDTO>()))
                .Returns(false);

            var newPlan = plans[0];

            var response = planController.Post(newPlan);

            Assert.AreEqual(response.StatusCode, HttpStatusCode.BadRequest);
        }
        
        [Test]
        public void PostPlanTestAndReturnId_ShouldSuccessfullyCreateNewPlanAndReturnId()
        {

            planServiceMock.Setup(mts => mts.AddAndGetId(It.IsAny<PlanDTO>())).Returns(1);
            var newPlan = plans[0];
            var response = planController.PostAndReturnId(newPlan);

            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);
        }

        [Test]
        public void PostPlanTestAndReturnId_ShouldCheckNotSuccessfullPostTryAndReturnBadRequestResponse()
        {
            planServiceMock.Setup(mts => mts.AddAndGetId(It.IsAny<PlanDTO>()))
                .Returns(()=>null);

            var newPlan = plans[0];

            var response = planController.PostAndReturnId(newPlan);

            Assert.AreEqual(response.StatusCode, HttpStatusCode.BadRequest);
        }

        [Test]
        public void PostPlanTest_ShouldCatchEntityException()
        {
            planServiceMock.Setup(mts => mts.Add(It.IsAny<PlanDTO>()))
                .Throws(new EntityException());

            var newPlan = plans[0];
            var response = planController.Post(newPlan);

            Assert.AreEqual(response.StatusCode, HttpStatusCode.InternalServerError);
        }

        [Test]
        public void PostPlanTestAndReturnId_ShouldCatchEntityException()
        {
            planServiceMock.Setup(mts => mts.AddAndGetId(It.IsAny<PlanDTO>()))
                .Throws(new EntityException());

            var newPlan = plans[0];
            var response = planController.PostAndReturnId(newPlan);

            Assert.AreEqual(response.StatusCode, HttpStatusCode.InternalServerError);
        }

        [Test]
        public void GetImageTest()
        {
            planServiceMock.Setup(u => u.ContainsId(It.IsInRange(1, 8, Range.Inclusive))).Returns(true);
            planServiceMock.Setup(u => u.GetImage(It.IsInRange(1, 3, Range.Inclusive))).Returns(new ImageDTO()
            {
                Base64Data = "test",
                Name = "test"
            });

            var response = planController.GetImage(1);
            response.TryGetContentValue<ImageDTO>(out var imageDTO);
            var expected = "test";
            var actual = imageDTO.Name;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void GetImageTest_ShouldCatchEntityException()
        {
            planServiceMock.Setup(u => u.ContainsId(It.IsInRange(1, 8, Range.Inclusive))).Returns(true);
            planServiceMock.Setup(mts => mts.GetImage(It.IsInRange(1, 3, Range.Inclusive)))
                .Throws(new EntityException());

            
            var response = planController.GetImage(1);

            Assert.AreEqual(response.StatusCode, HttpStatusCode.InternalServerError);
        }

        [Test]
        public void GetImageTest_ShouldReturnNoContentExeption()
        {
            planServiceMock.Setup(u => u.ContainsId(It.IsInRange(1, 8, Range.Inclusive))).Returns(false);      
            
            var response = planController.GetImage(1);

            Assert.AreEqual(response.StatusCode, HttpStatusCode.NoContent);
        }

        [Test]
        public void GetImageTestFromDatabase_ShouldReturnNoContentExeption()
        {
            planServiceMock.Setup(mts => mts.GetImage(It.IsInRange(1, 3, Range.Inclusive)))
          .Returns(() => null);

            var response = planController.GetImage(1);

            Assert.AreEqual(response.StatusCode, HttpStatusCode.NoContent);
        }

        [Test]
        public void SearchTest_ShouldReturnAllPlansByNullKey()
        {
            planServiceMock.Setup(mts => mts.GetAll()).Returns(plans);

            var response = planController.Search(null);
            var successfull = response.TryGetContentValue<List<PlanDTO>>(out var planDTOs);
            var expected = planServiceMock.Object.GetAll().Count();
            var actual = planDTOs.Count();

            Assert.IsTrue(successfull);
            Assert.AreEqual(expected, actual);
            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);
        }

        [Test]
        public void SearchTest_ShouldReturnPlansByKey()
        {
            planServiceMock.Setup(mts => mts.Search(It.IsAny<string[]>())).Returns(
                (string[] lines) => GetTestPlansSearch(lines));

            var searchKey = "1";
            var response = planController.Search(searchKey);
            var successfull = response.TryGetContentValue<List<PlanDTO>>(out var planDTOs);
            var expected = planServiceMock.Object.Search(new[] { searchKey });
            var actual = planDTOs;

            Assert.IsTrue(successfull);
            Assert.AreEqual(expected.Count, actual.Count);
            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);
        }

        [Test]
        public void SearchTest_ShouldReturnNoContentResponse()
        {
            planServiceMock.Setup(mts => mts.Search(It.IsAny<string[]>())).Returns(
                (string[] lines) => null);

            var searchKey = "1";
            var response = planController.Search(searchKey);

            Assert.AreEqual(response.StatusCode, HttpStatusCode.NoContent);
        }

        
    }
}
