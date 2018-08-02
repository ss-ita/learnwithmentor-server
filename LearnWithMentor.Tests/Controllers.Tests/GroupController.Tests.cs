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
    public class GroupControllerTests
    {

        private GroupController groupController;
        private Mock<IGroupService> groupServiceMock;
        private List<GroupDTO> groups;
        private Mock<ITraceWriter> traceWriterMock;
        private Mock<IUserService> userServiceMock;
        private Mock<IUserIdentityService> userIdentityServiceMock;
       

        [OneTimeSetUp]
        public void SetUp()
        {
            groups = new List<GroupDTO>()
            {
                new GroupDTO(1, "name1", 1, "mentorName1"),
                new GroupDTO(2, "name2", 2, "mentorName2"),
                new GroupDTO(3, "name3", 3, "mentorName3"),
                new GroupDTO(4, "name4", 4, "mentorName4")
            };

            groupServiceMock = new Mock<IGroupService>();
            traceWriterMock = new Mock<ITraceWriter>();
            userServiceMock = new Mock<IUserService>();
            userIdentityServiceMock = new Mock<IUserIdentityService>();

            var userPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Role, "Admin")
            }));

            groupController = new GroupController( groupServiceMock.Object, userServiceMock.Object, userIdentityServiceMock.Object, traceWriterMock.Object);
            groupController.ControllerContext.RequestContext.Principal = userPrincipal;
            groupController.Request = new HttpRequestMessage();
            groupController.Configuration = new HttpConfiguration();
            groupController.ControllerContext.ControllerDescriptor = new HttpControllerDescriptor(
                groupController.Configuration, "GroupController", groupController.GetType());
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            groupController.Dispose();
            groupServiceMock = null;
            userServiceMock = null;
            userIdentityServiceMock = null;
            traceWriterMock = null;
            groups = null;
        }

        [Test]
        public void GetGroupByIdTest_ShouldReturnTask()
        {
           groupServiceMock.Setup(u => u.GetGroupById(It.IsAny<int>())).Returns(
                (int i) => groups.Where(x => x.Id == i).Single());
            
            var response = groupController.GetById(3);
            var successfull = response.TryGetContentValue<GroupDTO>(out var groupDTO);
            var expected = groupServiceMock.Object.GetGroupById(3);               
            var actual = groupDTO;

            Assert.IsTrue(successfull);
            Assert.AreEqual(expected.Id, actual.Id);
            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);
           
        }

        [Test]
        public void GetGroupByIdTest_ShouldReturnNotFoundResponse()
        {
            groupServiceMock.Setup(u => u.GetGroupById(It.IsAny<int>()));

            var response = groupController.GetById(1);
            var expected = HttpStatusCode.NotFound;
            var actual = response.StatusCode;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void GetGroupByMentor_ShouldReturnGroupsForMentor()
        {
            groupServiceMock.Setup(g => g.GetGroupsByMentor(It.IsAny<int>())).Returns(new List<GroupDTO>());

            var response = groupController.GetByMentor(2);               
            var successfull = response.TryGetContentValue<List<GroupDTO>>(out var groupDTOs);
            var expected = groupServiceMock.Object.GetGroupsByMentor(2);
            var actual = groupDTOs;

            Assert.IsTrue(successfull);
            Assert.AreEqual(expected, actual);
            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);

        }

        [Test]
        public void GetGroupByMentor_ShouldReturnNotFoundResponse()
        {
            groupServiceMock.Setup(u => u.GetGroupsByMentor(It.IsAny<int>()));           

            var response = groupController.GetByMentor(6);
            var expected = HttpStatusCode.NotFound;
            var actual = response.StatusCode;
            Assert.AreEqual(expected, actual);
        }
        
        [Test]
        public void CreateGroupTest()
        {
            groupServiceMock.Setup(u => u.AddGroup(It.IsAny<GroupDTO>())).Returns(true);

            GroupDTO requestValue = new GroupDTO(5,"newName", 2, "newMentorName");            
            var response = groupController.Post(requestValue);
            var expected = HttpStatusCode.OK;
            var actual = response.StatusCode;

            Assert.AreEqual(expected, actual);
        }      
        
     }
}
