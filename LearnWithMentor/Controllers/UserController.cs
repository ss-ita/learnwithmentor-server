using System;
using System.Collections.Generic;
using System.Web.Http;
using LearnWithMentorDAL.Entities;
using LearnWithMentorDAL.UnitOfWork;
using LearnWithMentorDTO;
using System.Net.Http;
using System.Net;

namespace LearnWithMentor.Controllers
{
    public class UserController : ApiController
    {
        private IUnitOfWork UoW;
        public UserController()
        {
            UoW = new UnitOfWork(new LearnWithMentor_DBEntities());
        }
        // GET: api/User
        public HttpResponseMessage Get()
        {
            List<UserDTO> dto = new List<UserDTO>();
            bool exists = false;
            foreach (var u in UoW.Users.GetAll())
            {
                exists = true;
                dto.Add(new UserDTO(u.Id, u.FirstName, u.LastName, u.Roles.Name, u.Blocked));
            }
            if (exists)
            {
                return Request.CreateResponse<IEnumerable<UserDTO>>(HttpStatusCode.OK, dto);
            }
            var message = "No users in database.";
            return Request.CreateErrorResponse(HttpStatusCode.NotFound, message);
        }

        [HttpGet]
        [Route("api/user/inrole/{role_id}")]
        public HttpResponseMessage GetUsersbyRole(int role_id)
        {
            var role = UoW.Roles.Get(role_id);
            bool exists = false;
            var dto = new List<UserDTO>();
            if (role != null)
            {
                foreach (var u in UoW.Users.GetUsersByRole(role_id))
                {
                    exists = true;
                    dto.Add(new UserDTO(u.Id, u.FirstName, u.LastName, u.Roles.Name, u.Blocked));
                }

            } 
           
            if (exists)
            {
                return Request.CreateResponse<IEnumerable<UserDTO>>(HttpStatusCode.OK, dto);
            }
            var message = "No user with this role_id  in database.";
            return Request.CreateErrorResponse(HttpStatusCode.NotFound, message);
        }
        // GET: api/User/5
        public HttpResponseMessage Get(int id)
        {
            User u = UoW.Users.Get(id);
            if (u != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new UserDTO(u.Id, 
                                                                            u.FirstName, 
                                                                            u.LastName,  
                                                                            u.Roles.Name,
                                                                            u.Blocked));
            }
            var message = "User does not exist in database.";
            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, message);
        }

        // POST: api/User
        public HttpResponseMessage Post([FromBody]UserLoginDTO value)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
            bool success = UoW.Users.Add(value);
            if (success)
            {
                try
                {
                    UoW.Save();
                    var okMessage = $"Succesfully created user: {value.Email}.";
                    return Request.CreateResponse(HttpStatusCode.OK, okMessage);
                }
                catch (Exception exception)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, exception);
                }
            }
            var message = "Incorrect request syntax.";
            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, message);
        }

        // PUT: api/User/5
        public HttpResponseMessage Put(int id, [FromBody]UserDTO value)
        {
            bool success = UoW.Users.UpdateById(id, value);
            if (success)
            {
                try
                {
                    UoW.Save();
                    var okMessage = $"Succesfully updated user id: {id}.";
                    return Request.CreateResponse(HttpStatusCode.OK, okMessage);
                }
                catch (Exception exception)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, exception);
                }
            }
            var message = "Incorrect request syntax or user does not exist.";
            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, message);
        }

        // DELETE: api/user/5
        public HttpResponseMessage Delete(int id)
        {
            bool success = UoW.Users.BlockById(id);
            if (success)
            {
                try
                {
                    UoW.Save();
                    var okMessage = $"Succesfully blocked user id: {id}.";
                    return Request.CreateResponse(HttpStatusCode.OK, okMessage);
                }
                catch (Exception exception)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, exception);
                }
            }
            var message = $"Not existing user with id: {id}.";
            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, message);
        }

        [HttpGet]
        [Route("api/user/search")]
        public HttpResponseMessage Search(string q, string role)
        {
            if (q == null)
            {
                return Get();
            }
            else
            {
                bool exists = false;
                Role criteria;
                bool existsRole = UoW.Roles.TryGetByName(role, out criteria);
                string[] lines = q.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                List<UserDTO> dto = new List<UserDTO>();
                int? searchParametr = null;
                if (role == "blocked")
                    searchParametr = -1;
                foreach (var u in existsRole ? UoW.Users.Search(lines,criteria.Id) : 
                    UoW.Users.Search(lines, searchParametr))
                {
                    exists = true;
                    dto.Add(new UserDTO(u.Id, u.FirstName, u.LastName, u.Roles.Name, u.Blocked));
                }
                if (exists)
                {
                    return Request.CreateResponse<IEnumerable<UserDTO>>(HttpStatusCode.OK, dto);
                }
                var message = "No users found.";
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, message);
            }
        }

        [Route("api/user/roles")]
        public HttpResponseMessage GetRoles()
        {
            bool exists = false;
            var dtos = new List<RoleDTO>();
            foreach(var role in UoW.Roles.GetAll())
            {
                exists = true;
                dtos.Add(new RoleDTO(role.Id, role.Name));
            }
            if (exists)
            {
                return Request.CreateResponse<IEnumerable<RoleDTO>>(HttpStatusCode.OK, dtos);
            }
            var message = "No roles in database.";
            return Request.CreateErrorResponse(HttpStatusCode.NotFound, message);
        }
    }
}
