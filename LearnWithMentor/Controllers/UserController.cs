using System;
using System.Collections.Generic;
using System.Web.Http;
using LearnWithMentorDTO;
using System.Net.Http;
using System.Net;
using LearnWithMentorBLL.Interfaces;
using LearnWithMentorBLL.Services;

namespace LearnWithMentor.Controllers
{
    public class UserController : ApiController
    {
        private readonly IUserService userService;
        private readonly IRoleService roleService;
        public UserController()
        {
            userService = new UserService();
            roleService = new RoleService();
        }
        // GET: api/User
        public HttpResponseMessage Get()
        {
            var users = userService.GetAllUsers();
            if (users.Count != 0)
            {
                return Request.CreateResponse<IEnumerable<UserDTO>>(HttpStatusCode.OK, users);
            }
            var message = "No users in database.";
            return Request.CreateErrorResponse(HttpStatusCode.NotFound, message);
        }

        [HttpGet]
        [Route("api/user/inrole/{role_id}")]
        public HttpResponseMessage GetUsersbyRole(int role_id)
        {
            var role = roleService.Get(role_id);
            if (role == null)
            {
                var roleErorMessage = "No roles with this id  in database.";
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, roleErorMessage);
            }
            List<UserDTO> users = userService.GetUsersByRole(role.Id);
            if (users.Count == 0)
            {
                var usersErorMessage = "No users with this role_id  in database.";
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, usersErorMessage);
            }
            return Request.CreateResponse<IEnumerable<UserDTO>>(HttpStatusCode.OK, users);
        }
        // GET: api/User/5
        public HttpResponseMessage Get(int id)
        {
            UserDTO user = userService.Get(id);
            if (user != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, user);
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
            try
            {
                bool success = userService.Add(value);
                if (success)
                {
                    var okMessage = $"Succesfully created user: {value.Email}.";
                    return Request.CreateResponse(HttpStatusCode.OK, okMessage);
                }
            }
            catch (Exception exception)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, exception);
            }
            var message = "Incorrect request syntax.";
            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, message);
        }

        // PUT: api/User/5
        public HttpResponseMessage Put(int id, [FromBody]UserDTO value)
        {
            try
            {
                bool success = userService.UpdateById(id, value);
                if (success)
                {
                    var okMessage = $"Succesfully updated user id: {id}.";
                    return Request.CreateResponse(HttpStatusCode.OK, okMessage);
                }
            }
            catch (Exception exception)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, exception);
            }
            var message = "Incorrect request syntax or user does not exist.";
            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, message);
        }

        // DELETE: api/user/5
        public HttpResponseMessage Delete(int id)
        {
            try
            {
                bool success = userService.BlockById(id);
                if (success)
                {
                    var okMessage = $"Succesfully blocked user id: {id}.";
                    return Request.CreateResponse(HttpStatusCode.OK, okMessage);
                }
            }
            catch (Exception exception)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, exception);
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
            RoleDTO criteria = roleService.GetByName(role);
            string[] lines = q.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            int? searchParametr = null;
            if (role == "blocked")
                searchParametr = -1;
            List<UserDTO> users = criteria != null ? userService.Search(lines, criteria.Id) :
                userService.Search(lines, searchParametr);
            if (users.Count != 0)
            {
                return Request.CreateResponse<IEnumerable<UserDTO>>(HttpStatusCode.OK, users);
            }
            var message = "No users found.";
            return Request.CreateErrorResponse(HttpStatusCode.NotFound, message);
        }

        [Route("api/user/roles")]
        public HttpResponseMessage GetRoles()
        {
            var roles = roleService.GetAllRoles();
            if (roles.Count != 0)
            {
                return Request.CreateResponse<IEnumerable<RoleDTO>>(HttpStatusCode.OK, roles);
            }
            var message = "No roles in database.";
            return Request.CreateErrorResponse(HttpStatusCode.NotFound, message);
        }
    }
}
