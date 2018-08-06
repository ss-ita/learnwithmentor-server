using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
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

      
    }
}
