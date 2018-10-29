using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Data.Entity.Core;
using System.Security.Claims;
using System.Threading.Tasks;
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
        private List<PlanDto> plans;
        private Mock<ITraceWriter> traceWriterMock;
        private Mock<IUserService> userServiceMock;
        private Mock<ITaskService> taskServiceMock;
        private Mock<IUserIdentityService> userIdentityServiceMock;


        [OneTimeSetUp]
        public void SetUp()
        {
            plans = new List<PlanDto>()
            {
                new PlanDto(1, "name1", "description1", true,1,"nameCreator1","lastenameCreator1",1,"nameCreator1","lastenameCreator1", DateTime.Now, DateTime.Now, false),
                new PlanDto(2, "name2", "description2", true,2,"nameCreator2","lastenameCreator2",2,"nameCreator1","lastenameCreator1",DateTime.Now, DateTime.Now, false),
                new PlanDto(3, "name3", "description3", true,3,"nameCreator3","lastenameCreator3",3,"nameCreator1","lastenameCreator1",DateTime.Now, DateTime.Now, false),
                new PlanDto(4, "name4", "description4", true,4,"nameCreator4","lastenameCreator4",4,"nameCreator1","lastenameCreator1",DateTime.Now, DateTime.Now, false)
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

            planController = new PlanController(planServiceMock.Object, taskServiceMock.Object, traceWriterMock.Object, userIdentityServiceMock.Object);
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

        private List<PlanDto> GetTestPlansSearch(string[] lines)
        {
           
            var result = new List<PlanDto>();
            foreach (var line in lines)
            {
                result.AddRange(plans.Where(t => t.Name.Contains(line)));
            }
            return result.Distinct().ToList();
        }

        [Test]
        public async Task GetAllPlansTest_ShouldReturnAllPlans()
        {
            planServiceMock.Setup(mts => mts.GetAll()).ReturnsAsync(plans);

            var response = await planController.Get();
            var successfull = response.TryGetContentValue<IEnumerable<PlanDto>>(out var planDTOs);
            var getall = await planServiceMock.Object.GetAll();
            var expected = getall.Count;
            var actual = planDTOs.Count();

            Assert.IsTrue(successfull);
            Assert.AreEqual(expected, actual);
            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);
        }

        [Test]
        public async Task GetTasksForPlanTest_ShouldReturnNoContentMessage()
        {
            planServiceMock.Setup(mts => mts.GetTasksForPlanAsync(It.IsAny<int>()))
                .Returns(()=> Task.FromResult<List<SectionDto>>(null));

            HttpResponseMessage response = await planController.GetTasksForPlanAsync(1);            

            Assert.AreEqual(response.StatusCode, HttpStatusCode.NoContent);
        }

        [Test]
        public async Task UpdatePlanTest_ShouldReturnSuccess()
        {
            planServiceMock.Setup(u => u.UpdateByIdAsync( It.IsAny<PlanDto>(), It.IsAny<int>())).ReturnsAsync(true);

            PlanDto forUpdating = new PlanDto(1, "name1", "description1", true, 1, "nameCreator1", "lastenameCreator1", 1, "nameCreator1", "lastenameCreator1", DateTime.Now, DateTime.Now,false);
            var response = await planController.PutAsync(1, forUpdating);
            var expectedStatusCode = HttpStatusCode.OK;
            var actualStatusCode = response.StatusCode;

            Assert.AreEqual(expectedStatusCode, actualStatusCode);
        }

        [Test]
        public async Task UpdatePlanTest_ShouldReturnBadRequestMessage()
        {
            planServiceMock.Setup(u => u.UpdateByIdAsync(It.IsAny<PlanDto>(), It.IsAny<int>())).ReturnsAsync(false);

            PlanDto forUpdating = new PlanDto(1, "name1", "description1", true, 1, "nameCreator1", "lastenameCreator1", 1, "nameCreator1", "lastenameCreator1", DateTime.Now, DateTime.Now, false);
            var response = await planController.PutAsync(1, forUpdating);
            var expectedStatusCode = HttpStatusCode.BadRequest;
            var actualStatusCode = response.StatusCode;

            Assert.AreEqual(expectedStatusCode, actualStatusCode);
        }

        [Test]
        public async Task UpdatePlanTest_ShouldCatchEntityExeption()
        {
            planServiceMock.Setup(u => u.UpdateByIdAsync(It.IsAny<PlanDto>(), It.IsAny<int>()))
                .Throws(new EntityException());

            PlanDto forUpdating = new PlanDto(1, "name1", "description1", true, 1, "nameCreator1", "lastenameCreator1", 1, "nameCreator1", "lastenameCreator1", DateTime.Now, DateTime.Now, false);
            var response = await planController.PutAsync(1, forUpdating);
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
        public async Task GetAllTasksAssignedToPlanTest_ShouldReturnNoContentMessage()
        {
            planServiceMock.Setup(mts => mts.GetAllTasksAsync(It.IsAny<int>()))
                .ReturnsAsync(()=>null);

            HttpResponseMessage response = await planController.GetAllTasksAsync(1);

            Assert.AreEqual(HttpStatusCode.NoContent, response.StatusCode);
        }

        
        [Test]
        public async Task GetAllPlansTest_ShouldReturnNoContentResponse()
        {
            planServiceMock.Setup(ps => ps.GetAll())
                .ReturnsAsync(()=>null);

            var response = await planController.Get();

            Assert.AreEqual(response.StatusCode, HttpStatusCode.NoContent);
        }

        [Test]
        public async Task GetPlanByIdTest_ShouldReturnPlan()
        {
            planServiceMock.Setup(mts => mts.GetAsync(It.IsAny<int>())).ReturnsAsync(
                (int i) => plans.Single(x => x.Id == i));

            var plan = plans[0];
            var response = await planController.GetAsync(plan.Id);
            var successfull = response.TryGetContentValue<PlanDto>(out var planDTO);
            var expected = await planServiceMock.Object.GetAsync(plan.Id);
            var actual = planDTO;

            Assert.IsTrue(successfull);
            Assert.AreEqual(expected.Id, actual.Id);
            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);
        }

        [Test]
        public async Task GetPlanByIdTest_ShouldReturnNoContentResponse()
        {
            planServiceMock.Setup(mts => mts.GetAsync(It.IsAny<int>())).Returns(Task.FromResult<PlanDto>(null));

            var plan = plans[0];
            var response = await planController.GetAsync(plan.Id);

            Assert.AreEqual(response.StatusCode, HttpStatusCode.NoContent);
        }

        [Test]
        public async Task PostPlanTest_ShouldSuccessfullyCreateNewPlan()
        {

            planServiceMock.Setup(mts => mts.AddAsync(It.IsAny<PlanDto>())).ReturnsAsync(true);
            var newPlan = plans[0];
            var response = await planController.PostAsync(newPlan);

            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);
        }

        [Test]
        public async Task PostImageTest_ShouldReturnNoContentResponse()
        {
            planServiceMock.Setup(u => u.ContainsId(It.IsAny<int>())).ReturnsAsync(false);
            var response = await planController.PostImageAsync(1);

            Assert.AreEqual(HttpStatusCode.NoContent,response.StatusCode);
        }
        
        [Test]
        public async Task PostPlanTest_ShouldCheckNotSuccessfullPostTryAndReturnBadRequestResponse()
        {
            planServiceMock.Setup(mts => mts.AddAsync(It.IsAny<PlanDto>()))
                .ReturnsAsync(false);

            var newPlan = plans[0];

            var response = await planController.PostAsync(newPlan);

            Assert.AreEqual(response.StatusCode, HttpStatusCode.BadRequest);
        }
        
        [Test]
        public async Task PostPlanTestAndReturnId_ShouldSuccessfullyCreateNewPlanAndReturnId()
        {

            planServiceMock.Setup(mts => mts.AddAndGetIdAsync(It.IsAny<PlanDto>())).ReturnsAsync(1);
            var newPlan = plans[0];
            HttpResponseMessage response = await planController.PostAndReturnIdAsync(newPlan);

            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);
        }

        [Test]
        public async Task PostPlanTestAndReturnId_ShouldCheckNotSuccessfullPostTryAndReturnBadRequestResponse()
        {
            planServiceMock.Setup(mts => mts.AddAndGetIdAsync(It.IsAny<PlanDto>()))
                .ReturnsAsync(()=>null);

            var newPlan = plans[0];

            HttpResponseMessage response = await planController.PostAndReturnIdAsync(newPlan);

            Assert.AreEqual(response.StatusCode, HttpStatusCode.BadRequest);
        }

        [Test]
        public async Task PostPlanTest_ShouldCatchEntityException()
        {
            planServiceMock.Setup(mts => mts.AddAsync(It.IsAny<PlanDto>()))
                .Throws(new EntityException());

            var newPlan = plans[0];
            var response = await planController.PostAsync(newPlan);

            Assert.AreEqual(response.StatusCode, HttpStatusCode.InternalServerError);
        }

        [Test]
        public async Task PostPlanTestAndReturnId_ShouldCatchEntityException()
        {
            planServiceMock.Setup(mts => mts.AddAndGetIdAsync(It.IsAny<PlanDto>()))
                .Throws(new EntityException());

            var newPlan = plans[0];
            HttpResponseMessage response = await planController.PostAndReturnIdAsync(newPlan);

            Assert.AreEqual(response.StatusCode, HttpStatusCode.InternalServerError);
        }

        [Test]
        public async Task GetImageTest()
        {
            planServiceMock.Setup(u => u.ContainsId(It.IsInRange(1, 8, Range.Inclusive))).ReturnsAsync(true);
            planServiceMock.Setup(u => u.GetImageAsync(It.IsInRange(1, 3, Range.Inclusive))).ReturnsAsync(new ImageDto()
            {
                Base64Data = "test",
                Name = "test"
            });

            var response = await planController.GetImageAsync(1);
            response.TryGetContentValue<ImageDto>(out var imageDTO);
            var expected = "test";
            var actual = imageDTO.Name;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task GetImageTest_ShouldCatchEntityException()
        {
            planServiceMock.Setup(u => u.ContainsId(It.IsInRange(1, 8, Range.Inclusive))).ReturnsAsync(true);
            planServiceMock.Setup(mts => mts.GetImageAsync(It.IsInRange(1, 3, Range.Inclusive)))
                .Throws(new EntityException());

            
            var response = await planController.GetImageAsync(1);

            Assert.AreEqual(response.StatusCode, HttpStatusCode.InternalServerError);
        }

        [Test]
        public async Task GetImageTest_ShouldReturnNoContentExeption()
        {
            planServiceMock.Setup(u => u.ContainsId(It.IsInRange(1, 8, Range.Inclusive))).ReturnsAsync(false);      
            
            var response = await planController.GetImageAsync(1);

            Assert.AreEqual(response.StatusCode, HttpStatusCode.NoContent);
        }

        [Test]
        public async Task GetImageTestFromDatabase_ShouldReturnNoContentExeption()
        {
            planServiceMock.Setup(mts => mts.GetImageAsync(It.IsInRange(1, 3, Range.Inclusive)))
          .Returns(() => null);

            var response = await planController.GetImageAsync(1);

            Assert.AreEqual(response.StatusCode, HttpStatusCode.NoContent);
        }

        [Test]
        public async Task SearchTest_ShouldReturnAllPlansByNullKey()
        {
            planServiceMock.Setup(mts => mts.GetAll()).ReturnsAsync(plans);

            var response = await planController.Search(null);
            var successfull = response.TryGetContentValue<List<PlanDto>>(out var planDTOs);
            var getall =  await planServiceMock.Object.GetAll();
            var expected = getall.Count;
            var actual = planDTOs.Count;

            Assert.IsTrue(successfull);
            Assert.AreEqual(expected, actual);
            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);
        }

        [Test]
        public async Task SearchTest_ShouldReturnPlansByKey()
        {
            planServiceMock.Setup(mts => mts.Search(It.IsAny<string[]>())).Returns(
                (string[] lines) => GetTestPlansSearch(lines));

            var searchKey = "1";
            var response = await planController.Search(searchKey);
            var successfull = response.TryGetContentValue<List<PlanDto>>(out var planDTOs);
            var expected = planServiceMock.Object.Search(new[] { searchKey });
            var actual = planDTOs;

            Assert.IsTrue(successfull);
            Assert.AreEqual(expected.Count, actual.Count);
            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);
        }

        [Test]
        public async Task SearchTest_ShouldReturnNoContentResponse()
        {
            planServiceMock.Setup(mts => mts.Search(It.IsAny<string[]>())).Returns(
                (string[] lines) => null);

            var searchKey = "1";
            var response = await planController.Search(searchKey);

            Assert.AreEqual(response.StatusCode, HttpStatusCode.NoContent);
        }

        
    }
}
