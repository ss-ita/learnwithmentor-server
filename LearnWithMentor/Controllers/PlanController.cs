using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using LearnWithMentorDTO;
using LearnWithMentorBLL.Interfaces;
using LearnWithMentorBLL.Services;

namespace LearnWithMentor.Controllers
{
    public class PlanController : ApiController
    {
        private readonly IPlanService planService;
        public PlanController()
        {
            planService = new PlanService();
        }

        // GET: api/Plan
        public HttpResponseMessage Get()
        {
            List<PlanDTO> dtoList = planService.GetAll();
            if (dtoList == null || dtoList.Count == 0)
            {
                var errorMessage = "No plans in database.";
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, errorMessage);
            }
            return Request.CreateResponse<IEnumerable<PlanDTO>>(HttpStatusCode.OK, dtoList);
        }

        public HttpResponseMessage Get(int id)
        {
            var plan = planService.Get(id);
            if (plan == null)
            {
                var message = "Plan does not exist in database.";
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, message);
            }
            return Request.CreateResponse<PlanDTO>(HttpStatusCode.OK, plan);
        }

        // POST: api/plan
        public HttpResponseMessage Post([FromBody]PlanDTO value)
        {
            try
            {
                var success = planService.Add(value);
                if (success)
                {
                    var okMessage = $"Succesfully created plan: {value.Name}";
                    return Request.CreateResponse(HttpStatusCode.OK, okMessage);
                }
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }
            var message = "Incorrect request syntax.";
            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, message);
        }

        // PUT: api/plan/5
        public HttpResponseMessage Put(int id, [FromBody]PlanDTO value)
        {
            try
            {
                var success = planService.UpdateById(value, id);
                if (success)
                {
                    var okMessage = $"Succesfully updated plan: {value.Name}";
                    return Request.CreateResponse(HttpStatusCode.OK, okMessage);
                }
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }
            var message = "Incorrect request syntax or plan does not exist.";
            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, message);
        }

        [HttpGet]
        [Route("api/plan/search")]
        public HttpResponseMessage Search(string q)
        {
            if (string.IsNullOrEmpty(q))
            {
                return Get();
            }
            string[] lines = q.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            var dto = planService.Search(lines);
            if (dto == null || dto.Count == 0)
            {
                var message = "No plans found.";
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, message);
            }
            return Request.CreateResponse<IEnumerable<PlanDTO>>(HttpStatusCode.OK, dto);
        }
    }
}
