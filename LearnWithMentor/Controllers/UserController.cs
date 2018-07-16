using System;
using System.Collections.Generic;
using System.Web.Http;
using LearnWithMentorDTO;
using System.Net.Http;
using System.Net;
using LearnWithMentor.Filters;
using LearnWithMentorBLL.Interfaces;
using System.Web.Http.Tracing;
using System.Data.Entity.Core;
using System.Web;
using System.IO;
using System.Linq;
using System.Security.Claims;

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
        private readonly ITaskService taskService;
        private readonly ITraceWriter tracer;

        /// <summary>
        /// Creates an instance of UserController.
        /// </summary>
        public UserController(IUserService userService, IRoleService roleService, ITaskService taskService, ITraceWriter tracer)
        {
            this.userService = userService;
            this.roleService = roleService;
            this.taskService = taskService;
            this.tracer = tracer;
        }

        /// <summary>
        /// Returns all users of the system.
        /// </summary>
        [JwtAuthentication]
        [Authorize(Roles = "Admin, Mentor")]
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
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
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
        /// Returns specific user by id in token.
        /// </summary>
        [JwtAuthentication]
        [HttpGet]
        [Route("api/user/profile")]
        public HttpResponseMessage GetSingle()
        {
            var identity = HttpContext.Current.User.Identity as ClaimsIdentity;
            var Id = int.Parse(identity.FindFirst("Id").Value);
            UserDTO user = userService.Get(Id);
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
                    tracer.Info(Request, ControllerContext.ControllerDescriptor.ControllerType.FullName, okMessage);
                    return Request.CreateResponse(HttpStatusCode.OK, okMessage);
                }
            }
            catch (EntityException e)
            {
                tracer.Error(Request, ControllerContext.ControllerDescriptor.ControllerType.FullName, e);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }
            var message = "Incorrect request syntax.";
            tracer.Warn(Request, ControllerContext.ControllerDescriptor.ControllerType.FullName, message);
            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, message);
        }

        /// <summary>
        /// Returns statistics dto with number of tasks in different states for one user.
        /// </summary>
        [JwtAuthentication]
        [HttpGet]
        [Route("api/user/statistics")]
        public HttpResponseMessage GetStatistics()
        {
            var identity = HttpContext.Current.User.Identity as ClaimsIdentity;
            var id = int.Parse(identity.FindFirst("Id").Value);
            var statsDTO = taskService.GetUserStatistics(id);
            if (statsDTO == null)
            {
                var errorMessage = "No user with this id in database.";
                return Request.CreateResponse(HttpStatusCode.NoContent, errorMessage);
            }
            return Request.CreateResponse(HttpStatusCode.OK, statsDTO);
        }

        /// <summary>
        /// Sets user image to database
        /// </summary>
        /// <param name="id"> Id of the user. </param>
        [JwtAuthentication]
        [HttpPost]
        [Route("api/user/{id}/image")]
        public HttpResponseMessage PostImage(int id)
        {
            if (!userService.ContainsId(id))
            {
                var errorMessage = "No user with this id in database.";
                return Request.CreateResponse(HttpStatusCode.NoContent, errorMessage);
            }

            if (HttpContext.Current.Request.Files.Count != 1)
            {
                var errorMessage = "Only one image can be sent.";
                return Request.CreateResponse(HttpStatusCode.BadRequest, errorMessage);
            }

            try
            {
                var postedFile = HttpContext.Current.Request.Files[0];
                if (postedFile != null && postedFile.ContentLength > 0)
                {
                    List<string> allowedFileExtensions = new List<string> { ".jpeg", ".jpg", ".png" };

                    var extension = postedFile.FileName.Substring(postedFile.FileName.LastIndexOf('.')).ToLower();
                    if (!allowedFileExtensions.Contains(extension))
                    {
                        string errorMessage = "Types allowed only .jpeg .jpg .png";
                        return Request.CreateResponse(HttpStatusCode.BadRequest, errorMessage);
                    }

                    int maxContentLength = 1024 * 1024 * 1; //Size = 1 MB  
                    if (postedFile.ContentLength > maxContentLength)
                    {
                        string errorMessage = "Please Upload a file upto 1 mb.";
                        return Request.CreateResponse(HttpStatusCode.BadRequest, errorMessage);
                    }

                    byte[] imageData = null;
                    using (var binaryReader = new BinaryReader(postedFile.InputStream))
                    {
                        imageData = binaryReader.ReadBytes(postedFile.ContentLength);
                    }

                    userService.SetImage(id, imageData, postedFile.FileName);
                    var okMessage = "Successfully created image.";
                    return Request.CreateResponse(HttpStatusCode.OK, okMessage);
                }
                string emptyImageMessage = "Empty image.";
                return Request.CreateErrorResponse(HttpStatusCode.NotModified, emptyImageMessage);
            }
            catch (EntityException e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, e);
            }
        }

        /// <summary>
        /// Reyurns image for specific user
        /// </summary>
        /// <param name="id"> Id of the user. </param>
        [JwtAuthentication]
        [HttpGet]
        [Route("api/user/{id}/image")]
        public HttpResponseMessage GetImage(int id)
        {
            try
            {
                if (!userService.ContainsId(id))
                {
                    var errorMessage = "No user with this id in database.";
                    return Request.CreateResponse(HttpStatusCode.NoContent, errorMessage);
                }
                ImageDTO dto = userService.GetImage(id);
                if (dto == null)
                {
                    var message = "No image for this user in database.";
                    return Request.CreateResponse(HttpStatusCode.NoContent, message);
                }
                return Request.CreateResponse(HttpStatusCode.OK, dto);
            }
            catch (EntityException e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, e);
            }
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
                    tracer.Info(Request, ControllerContext.ControllerDescriptor.ControllerType.FullName, okMessage);
                    return Request.CreateResponse(HttpStatusCode.OK, okMessage);
                }
            }
            catch (EntityException e)
            {
                tracer.Error(Request, ControllerContext.ControllerDescriptor.ControllerType.FullName, e);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }
            var message = "Incorrect request syntax or user does not exist.";
            tracer.Warn(Request, ControllerContext.ControllerDescriptor.ControllerType.FullName, message);
            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, message);
        }

        /// <summary>
        /// Blocks user by Id.
        /// </summary>
        /// <param name="id"> Id of the user. </param>
        [JwtAuthentication]
        [Authorize(Roles = "Admin")]
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
                    tracer.Info(Request, ControllerContext.ControllerDescriptor.ControllerType.FullName, okMessage);
                    return Request.CreateResponse(HttpStatusCode.OK, okMessage);
                }
            }
            catch (EntityException e)
            {
                tracer.Error(Request, ControllerContext.ControllerDescriptor.ControllerType.FullName, e);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }
            var message = $"Not existing user with id: {id}.";
            tracer.Warn(Request, ControllerContext.ControllerDescriptor.ControllerType.FullName, message);
            return Request.CreateErrorResponse(HttpStatusCode.NoContent, message);
        }

        /// <summary>
        /// Search for user with match in first or lastname with role criteria.
        /// </summary>
        /// <param name="q"> String to match. </param>
        /// <param name="role"> Role criteria. </param>
        [JwtAuthentication]
        [Authorize(Roles = "Admin, Mentor")]
        [HttpGet]
        [Route("api/user/search")]
        public HttpResponseMessage Search(string q, string role)
        {
            if (q == null)
            {
                q = "";
            }
            RoleDTO criteria = roleService.GetByName(role);
            string[] lines = q.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            int? searchParametr = null;
            if (role == "blocked")
            {
                searchParametr = -1;
            }
            if (lines.Length > 2)
            {
                lines = lines.Take(2).ToArray();
            }
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
        /// Updates user password.
        /// </summary>
        /// <param name="id"> Id of the user. </param>
        /// <param name="value"> New password value. </param>
        /// <returns></returns>
        [JwtAuthentication]
        [HttpPut]
        [Route("api/user/newpassword")]
        public HttpResponseMessage Post([FromBody]string value)
        {
            try
            {
                var identity = HttpContext.Current.User.Identity as ClaimsIdentity;
                var Id = int.Parse(identity.FindFirst("Id").Value);
                bool success = userService.UpdatePassword(Id, value);
                if (success)
                {
                    var okMessage = $"Succesfully updated password.";
                    tracer.Info(Request, ControllerContext.ControllerDescriptor.ControllerType.FullName, okMessage);
                    return Request.CreateResponse(HttpStatusCode.OK, okMessage);
                }
                var noUserMessage = "No user with this ID in database.";
                return Request.CreateResponse(HttpStatusCode.NoContent, noUserMessage);
            }
            catch (EntityException e)
            {
                tracer.Error(Request, ControllerContext.ControllerDescriptor.ControllerType.FullName, e);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }
        }

        /// <summary>
        /// Returns all roles of the users.
        /// </summary>
        [JwtAuthentication]
        [HttpGet]
        [Authorize(Roles = "Admin")]
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

        [HttpGet]
        [Route("api/user/pageSize/{pageSize}/pageNumber/{pageNumber}")]
        public HttpResponseMessage GetUsers(int pageSize, int pageNumber)
        {
            try
            {
                var users = userService.GetUsers(pageSize, pageNumber);
                if (users != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, users);
                }
                return Request.CreateErrorResponse(HttpStatusCode.NoContent, "There are no users in database.");
            }
            catch (EntityException e)
            {
                tracer.Error(Request, ControllerContext.ControllerDescriptor.ControllerType.FullName, e);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }
        }
    }
}
