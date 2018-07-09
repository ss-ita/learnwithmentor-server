using System.Net;
using System.Net.Http;
using System.Web.Http;
using LearnWithMentorBLL.Interfaces;
using LearnWithMentorBLL.Services;
using System.Web.Http.Tracing;

namespace LearnWithMentor.Controllers
{
    public class DataBaseController : ApiController
    {
        private readonly IDataBaseService dataBaseService;
        private readonly ITraceWriter tracer;

        public DataBaseController()
        {
            dataBaseService = new DataBaseService();
        }

        [HttpPost]
        [Route("api/database/initializeNow")]
        public HttpResponseMessage InitiateDatabase()
        {
            dataBaseService.DbInitialize();
            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}