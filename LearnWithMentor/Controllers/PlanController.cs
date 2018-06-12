using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using LearnWithMentorDAL.Entities;
using LearnWithMentorDAL.UnitOfWork;
using LearnWithMentorDTO;

namespace LearnWithMentor.Controllers
{
    public class PlanController : ApiController
    {
        private IUnitOfWork UoW;
        public PlanController()
        {
            UoW = new UnitOfWork(new LearnWithMentor_DBEntities());
        }
        // GET: api/Plan
        public HttpResponseMessage Get()
        {
            var dto = new List<PlanDTO>();
            bool exists = false;
            foreach (var p in UoW.Plans.GetAll())
            {
                exists = true;
                var firstName = p.Users1?.FirstName;
                var lastName = p.Users1?.LastName;
                dto.Add(
                    new PlanDTO(
                        p.Id, 
                        p.Name, 
                        p.Description, 
                        p.Published, 
                        p.Create_Id, 
                        p.Users.FirstName, 
                        p.Users.LastName, 
                        p.Mod_Id,
                        firstName,
                        lastName, 
                        p.Create_Date, 
                        p.Mod_Date
                        )
                    );
            }

            if (exists)
            {
                return Request.CreateResponse<IEnumerable<PlanDTO>>(HttpStatusCode.OK, dto);
            }
            var message = "No plans in database.";
            return Request.CreateErrorResponse(HttpStatusCode.NotFound, message);
        }

        public HttpResponseMessage Get(int id)
        {
            var p = UoW.Plans.Get(id);
            if (p != null)
            {
                var firstName = p.Users1?.FirstName;
                var lastName = p.Users1?.LastName;
                return Request.CreateResponse(HttpStatusCode.OK, new PlanDTO(p.Id,
                    p.Name,
                    p.Description,
                    p.Published,
                    p.Create_Id,
                    p.Users.FirstName,
                    p.Users.LastName,
                    p.Mod_Id,
                    firstName,
                    lastName,
                    p.Create_Date,
                    p.Mod_Date)
                );
            }
            var message = "Plan does not exist in database.";
            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, message);
        }

        // POST: api/plan
        public HttpResponseMessage Post([FromBody]PlanDTO value)
        {
            var success = UoW.Plans.Add(value);
            if (success)
            {
                try
                {
                    UoW.Save();
                    var okMessage = $"Succesfully created plan: {value.Name}";
                    return Request.CreateResponse(HttpStatusCode.OK, okMessage);
                }
                catch (Exception e)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
                }
            }
            var message = "Incorrect request syntax.";
            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, message);
        }

        // PUT: api/plan/5
        public HttpResponseMessage Put(int id, [FromBody]PlanDTO value)
        {
            var success = UoW.Plans.UpdateById(value, id);
            if (success)
            {
                try
                {
                    UoW.Save();
                    var okMessage = $"Succesfully updated plan: {value.Name}";
                    return Request.CreateResponse(HttpStatusCode.OK, okMessage);
                }
                catch (Exception e)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
                }
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
            bool exists = false;
            string[] lines = q.Split(' ');
            var dto = new List<PlanDTO>();
            foreach (var p in  UoW.Plans.Search(lines))
            {
                exists = true;
                var firstName = p.Users1?.FirstName;
                var lastName = p.Users1?.LastName;
                dto.Add(
                    new PlanDTO
                        (
                            p.Id,
                            p.Name,
                            p.Description,
                            p.Published,
                            p.Create_Id,
                            p.Users.FirstName,
                            p.Users.LastName,
                            p.Mod_Id,
                            firstName,
                            lastName,
                            p.Create_Date,
                            p.Mod_Date
                        )
                    );
            }
            if (exists)
            {
                return Request.CreateResponse<IEnumerable<PlanDTO>>(HttpStatusCode.OK, dto);
            }
            var message = "No plans found.";
            return Request.CreateErrorResponse(HttpStatusCode.NotFound, message);
        }

        // DELETE: api/plan/5
        public HttpResponseMessage Delete(int id)
        {
            var success = UoW.Plans.RemoveById(id);
            UoW.Save();
            if (success)
            {
                try
                {
                    UoW.Save();
                    var okMessage = $"Succesfully deleted plan: {id}";
                    return Request.CreateResponse(HttpStatusCode.OK, okMessage);
                }
                catch (Exception e)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
                }
            }
            var message = $"Not exist plan with id: {id}";
            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, message);
        }

    }
}
