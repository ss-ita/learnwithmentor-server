using System;
using System.Collections.Generic;
using System.Web.Http;
using LearnWithMentorDTO;
using System.Net.Http;
using System.Net;
using LearnWithMentor.Filters;
using LearnWithMentorBLL.Interfaces;
using LearnWithMentorBLL.Services;

namespace LearnWithMentor.Controllers
{

    [Authorize]


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

        //  [Authorize(Roles = "Admin")]
        [JwtAuthentication]
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

        [JwtAuthentication]
        [HttpGet]
        [Route("api/user/inrole/{role_id}")]
        public HttpResponseMessage GetUsersbyRole(int role_id)
        {
            if (role_id != -1)
            {
                var role = roleService.Get(role_id);
                if (role == null)
                {
                    var roleErorMessage = "No roles with this id  in database.";
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, roleErorMessage);
                }
            }
            List<UserDTO> users = userService.GetUsersByRole(role_id);
            if (users.Count == 0)
            {
                var usersErorMessage = "No users with this role_id  in database.";
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, usersErorMessage);
            }
            return Request.CreateResponse<IEnumerable<UserDTO>>(HttpStatusCode.OK, users);
        }

        [JwtAuthentication]
        [HttpGet]
        [Route("api/user/instate/{state}")]
        public HttpResponseMessage GetUsersbyState(bool state)
        {
            List<UserDTO> users = userService.GetUsersByState(state);
            if (users.Count == 0)
            {
                var usersErorMessage = "No users with this state in database.";
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, usersErorMessage);
            }
            return Request.CreateResponse<IEnumerable<UserDTO>>(HttpStatusCode.OK, users);
        }

        // GET: api/User/5
        //  [Authorize (Roles="Student")]

        [JwtAuthentication]

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
        
        [AllowAnonymous]
        public HttpResponseMessage Post([FromBody]UserRegistrationDTO value)
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
        [JwtAuthentication]
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
        [JwtAuthentication]
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
        [JwtAuthentication]
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
        [JwtAuthentication]
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
        /// <summary>
        /// Releases memory
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            userService.Dispose();
            roleService.Dispose();
            base.Dispose(disposing);
        }
    }
}
