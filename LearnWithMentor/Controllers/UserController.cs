using System;
using System.Collections.Generic;
using System.Web.Http;
using LearnWithMentorDTO;
using System.Net.Http;
using System.Net;
using LearnWithMentor.Filters;
using LearnWithMentorBLL.Interfaces;
using LearnWithMentorBLL.Services;
using System.Web.Http.Tracing;
using LearnWithMentor.Log;

namespace LearnWithMentor.Controllers
{
    /// <summary>
    /// Controller for system users.
    /// </summary>
    [Authorize]
    public class UserController : ApiController
    {
        private readonly IUserService userService;
        private readonly IRoleService roleService;
        private readonly ITraceWriter _tracer;

        /// <summary>
        /// Creates an instance of UserController.
        /// </summary>
        public UserController()
        {
            userService = new UserService();
            roleService = new RoleService();
            _tracer = new NLogger();
        }

        /// <summary>
        /// Returns all users of the system.
        /// </summary>
        [JwtAuthentication]
        [HttpGet]
        [Route("api/user")]
        public HttpResponseMessage Get()
        {
            var users = userService.GetAllUsers();
            if (users.Count != 0)
            {
                return Request.CreateResponse<IEnumerable<UserDTO>>(HttpStatusCode.OK, users);
            }
            var message = "No users in database.";
            return Request.CreateErrorResponse(HttpStatusCode.NoContent, message);
        }

        /// <summary>
        /// Returns all users with specified role.
        /// </summary>
        /// <param name="role_id"> Id of the role. </param>
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
                    return Request.CreateErrorResponse(HttpStatusCode.NoContent, roleErorMessage);
                }
            }
            List<UserDTO> users = userService.GetUsersByRole(role_id);
            if (users.Count == 0)
            {
                var usersErorMessage = "No users with this role_id  in database.";
                return Request.CreateErrorResponse(HttpStatusCode.NoContent, usersErorMessage);
            }
            return Request.CreateResponse<IEnumerable<UserDTO>>(HttpStatusCode.OK, users);
        }

        /// <summary>
        /// Returns all blocked/unblocked users.
        /// </summary>
        /// <param name="state"> Specifies value of Blocked property of user. </param>
        [JwtAuthentication]
        [HttpGet]
        [Route("api/user/instate/{state}")]
        public HttpResponseMessage GetUsersbyState(bool state)
        {
            List<UserDTO> users = userService.GetUsersByState(state);
            if (users.Count == 0)
            {
                var usersErorMessage = "No users with this state in database.";
                return Request.CreateErrorResponse(HttpStatusCode.NoContent, usersErorMessage);
            }
            return Request.CreateResponse<IEnumerable<UserDTO>>(HttpStatusCode.OK, users);
        }

        /// <summary>
        /// Returns specific user by id.
        /// </summary>
        /// <param name="id"> Id of the user. </param>
        [JwtAuthentication]
        [HttpGet]
        [Route("api/user/{id}")]
        public HttpResponseMessage Get(int id)
        {
            UserDTO user = userService.Get(id);
            if (user != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, user);
            }
            var message = "User does not exist in database.";
            return Request.CreateErrorResponse(HttpStatusCode.NoContent, message);
        }

        /// <summary>
        /// Creates new user.
        /// </summary>
        /// <param name="value"> New user. </param>
        [AllowAnonymous]
        [HttpPost]
        [Route("api/user")]
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
                    _tracer.Info(Request, ControllerContext.ControllerDescriptor.ControllerType.FullName, okMessage);
                    return Request.CreateResponse(HttpStatusCode.OK, okMessage);
                }
            }
            catch (Exception exception)
            {
                _tracer.Error(Request, ControllerContext.ControllerDescriptor.ControllerType.FullName, exception);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, exception);
            }
            var message = "Incorrect request syntax.";
            _tracer.Warn(Request, ControllerContext.ControllerDescriptor.ControllerType.FullName, message);
            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, message);
        }

        /// <summary>
        /// Updates user by id.
        /// </summary>
        /// <param name="id"> Id of the user. </param>
        /// <param name="value"> New values. </param>
        [JwtAuthentication]
        [HttpPut]
        [Route("api/user/{id}")]
        public HttpResponseMessage Put(int id, [FromBody]UserDTO value)
        {
            try
            {
                bool success = userService.UpdateById(id, value);
                if (success)
                {
                    var okMessage = $"Succesfully updated user id: {id}.";
                    _tracer.Info(Request, ControllerContext.ControllerDescriptor.ControllerType.FullName, okMessage);
                    return Request.CreateResponse(HttpStatusCode.OK, okMessage);
                }
            }
            catch (Exception exception)
            {
                _tracer.Error(Request, ControllerContext.ControllerDescriptor.ControllerType.FullName, exception);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, exception);
            }
            var message = "Incorrect request syntax or user does not exist.";
            _tracer.Warn(Request, ControllerContext.ControllerDescriptor.ControllerType.FullName, message);
            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, message);
        }

        /// <summary>
        /// Blocks user by Id.
        /// </summary>
        /// <param name="id"> Id of the user. </param>
        [JwtAuthentication]
        [HttpDelete]
        [Route("api/user/{id}")]
        public HttpResponseMessage Delete(int id)
        {
            try
            {
                bool success = userService.BlockById(id);
                if (success)
                {
                    var okMessage = $"Succesfully blocked user id: {id}.";
                    _tracer.Info(Request, ControllerContext.ControllerDescriptor.ControllerType.FullName, okMessage);
                    return Request.CreateResponse(HttpStatusCode.OK, okMessage);
                }
            }
            catch (Exception exception)
            {
                _tracer.Error(Request, ControllerContext.ControllerDescriptor.ControllerType.FullName, exception);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, exception);
            }
            var message = $"Not existing user with id: {id}.";
            _tracer.Warn(Request, ControllerContext.ControllerDescriptor.ControllerType.FullName, message);
            return Request.CreateErrorResponse(HttpStatusCode.NoContent, message);
        }

        /// <summary>
        /// Search for user with match in first or lastname with role criteria.
        /// </summary>
        /// <param name="q"> String to match. </param>
        /// <param name="role"> Role criteria. </param>
        [JwtAuthentication]
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
            return Request.CreateErrorResponse(HttpStatusCode.NoContent, message);
        }

        /// <summary>
        /// Returns all roles of the users.
        /// </summary>
        [JwtAuthentication]
        [HttpGet]
        [Route("api/user/roles")]
        public HttpResponseMessage GetRoles()
        {
            var roles = roleService.GetAllRoles();
            if (roles.Count != 0)
            {
                return Request.CreateResponse<IEnumerable<RoleDTO>>(HttpStatusCode.OK, roles);
            }
            var message = "No roles in database.";
            return Request.CreateErrorResponse(HttpStatusCode.NoContent, message);
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
