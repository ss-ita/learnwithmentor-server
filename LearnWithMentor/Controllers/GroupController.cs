using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using LearnWithMentorDTO;
using LearnWithMentorBLL.Interfaces;
using LearnWithMentorBLL.Infrastructure;
using LearnWithMentorBLL.Services;

namespace LearnWithMentor.Controllers
{
    public class GroupController : ApiController
    {
        private readonly IGroupService groupService;

        public GroupController()
        {
            groupService = new GroupService();
        }
        // GET api/<controller>
        /// <summary>
        /// Returns group by mentor Id
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("api/group/mentor/{id}")]
        public HttpResponseMessage Get(int mentorId)
        {
            var allGroups = groupService.GetGroupsByMentor(mentorId);
            if (allGroups != null)
                return Request.CreateResponse(HttpStatusCode.OK, allGroups);
            else
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No groups for the mentor in database. (mentorId = ${mentorId})");
        }
        [HttpGet]
        [Route("api/group/{id}")]
        public HttpResponseMessage GetById(int id)
        {
            var group = groupService.GetGroupById(id);
            if (group != null)
                return Request.CreateResponse(HttpStatusCode.OK, group);
            else
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "There isn't group with id = ${id}");
        }

        // POST api/<controller>
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}