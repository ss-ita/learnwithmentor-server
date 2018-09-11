using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using LearnWithMentorDTO;
using System.Net.Http;
using System.Net;
using LearnWithMentor.Filters;
using LearnWithMentor.Models;
using LearnWithMentor.Services;
using LearnWithMentorBLL.Interfaces;
using System.Web.Http.Tracing;
using System.Data.Entity.Core;
using System.Web;
using System.IO;
using System.Linq;

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
        private readonly IUserIdentityService userIdentityService;
        private readonly ITraceWriter tracer;
        /// <summary>
        /// Creates an instance of UserController.
        /// </summary>
        public UserController(IUserService userService, IRoleService roleService, ITaskService taskService, IUserIdentityService userIdentityService, ITraceWriter tracer)
        {
            this.userService = userService;
            this.roleService = roleService;
            this.taskService = taskService;
            this.userIdentityService = userIdentityService;
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
            const string message = "No users in database.";
            return Request.CreateErrorResponse(HttpStatusCode.NoContent, message);
        }
        /// <summary>
        /// Returns one page of users
        /// </summary>
        [JwtAuthentication]
        [Authorize(Roles = "Admin, Mentor")]
        [HttpGet]
        [Route("api/user")]
        public HttpResponseMessage Get([FromUri]int pageSize, [FromUri]int pageNumber)
        {
            try
            {
                var users = userService.GetUsers(pageSize, pageNumber);
                return Request.CreateResponse(HttpStatusCode.OK, users);
            }
            catch (EntityException e)
            {
                tracer.Error(Request, ControllerContext.ControllerDescriptor.ControllerType.FullName, e);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }
        }

        /// <summary>
        /// Returns all users with specified role.
        /// </summary>
        /// <param name="roleId"> Id of the role. </param>
        [JwtAuthentication]
        [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("api/user/inrole/{roleId}")]
        public HttpResponseMessage GetUsersbyRole(int roleId)
        {
            if (roleId != Constants.Roles.BlockedIndex)
            {
                var role = roleService.Get(roleId);
                if (role == null)
                {
                    const string roleErorMessage = "No roles with this id  in database.";
                    return Request.CreateErrorResponse(HttpStatusCode.NoContent, roleErorMessage);
                }
            }
            var users = userService.GetUsersByRole(roleId);
            if (users.Count == 0)
            {
                const string usersErorMessage = "No users with this role_id  in database.";
                return Request.CreateErrorResponse(HttpStatusCode.NoContent, usersErorMessage);
            }
            return Request.CreateResponse<IEnumerable<UserDTO>>(HttpStatusCode.OK, users);
        }
        /// <summary>
        /// Returns one page of users with specified role.
        /// </summary>
        /// <param name="roleId"> Id of the role. </param>
        /// <param name="pageSize"> Ammount of users that you want to see on one page</param>
        /// <param name="pageNumber"> Page number</param>
        [JwtAuthentication]
        [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("api/user/inrole/{roleId}")]
        public HttpResponseMessage GetUsersbyRole(int roleId, [FromUri]int pageSize, [FromUri]int pageNumber)
        {
            if (roleId != Constants.Roles.BlockedIndex)
            {
                var role = roleService.Get(roleId);
                if (role == null)
                {
                    const string roleErorMessage = "No roles with this id  in database.";
                    return Request.CreateErrorResponse(HttpStatusCode.NoContent, roleErorMessage);
                }
            }
            var users = userService.GetUsersByRole(roleId, pageSize, pageNumber);
            return Request.CreateResponse(HttpStatusCode.OK, users);
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
            var users = userService.GetUsersByState(state);
            if (users.Count == 0)
            {
                const string usersErorMessage = "No users with this state in database.";
                return Request.CreateErrorResponse(HttpStatusCode.NoContent, usersErorMessage);
            }
            return Request.CreateResponse<IEnumerable<UserDTO>>(HttpStatusCode.OK, users);
        }

        /// <summary>
        /// Returns one page of blocked/unblocked users.
        /// </summary>
        /// <param name="state"> Specifies value of Blocked property of user. </param>
        /// <param name="pageSize"> Ammount of users that you want to see on one page</param>
        /// <param name="pageNumber"> Page number</param>
        [JwtAuthentication]
        [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("api/user/instate/{state}")]
        public HttpResponseMessage GetUsersbyState(bool state, [FromUri]int pageSize, [FromUri]int pageNumber)
        {
            var users = userService.GetUsersByState(state, pageSize, pageNumber);
            return Request.CreateResponse(HttpStatusCode.OK, users);
        }

        /// <summary>
        /// Returns specific user by id if exists or get id from token.
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        [JwtAuthentication]
        [HttpGet]
        [Route("api/user/profile/{id?}")]
        public HttpResponseMessage GetSingle(int id = 0 )
        {
            if (id == 0)
            {
                id = userIdentityService.GetUserId();
            }

            var user = userService.Get(id);
            if (user != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, user);
            }
            const string message = "User does not exist in database.";
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
                var success = userService.Add(value);
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
            const string message = "Incorrect request syntax.";
            tracer.Warn(Request, ControllerContext.ControllerDescriptor.ControllerType.FullName, message);
            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, message);
        }

        /// <summary>
        /// Verifies reset password token.
        /// </summary>
        /// <param name="token"> Users token. </param>
        [AllowAnonymous]
        [HttpGet]
        [Route("api/user/verify-token")]
        public HttpResponseMessage VerifyToken(string token)
        {
            try
            {
                if (JwtAuthenticationAttribute.ValidateToken(token, out string userEmail))
                {
                    var user = userService.GetByEmail(userEmail);
                    if (user == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "User not found");
                    }
                    return Request.CreateResponse(HttpStatusCode.OK, user.Id);
                }
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Token no longer valid");
            }
            catch (EntityException e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }
        }

        /// <summary>
        /// Confirms user email by token.
        /// </summary>
        /// <param name="token"> Users token. </param>
        [AllowAnonymous]
        [HttpGet]
        [Route("api/user/confirm-email")]
        public HttpResponseMessage ConfirmEmail(string token)
        {
            try
            {
                if (JwtAuthenticationAttribute.ValidateToken(token, out string userEmail))
                {
                    var user = userService.GetByEmail(userEmail);
                    if (user == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NoContent, "User not found");
                    }
                    if (userService.ConfirmEmailById(user.Id))
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, "Email confirmed");
                    }
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Confirmation error");
                }
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Token no longer valid");
            }
            catch (EntityException e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }
        }

        /// <summary>
        /// Sends email with link for user's password reset.
        /// </summary>
        /// <param name="emailModel"> User's email. </param>
        /// <param name="resetPasswordLink"> Link on the reset page. </param>
        [AllowAnonymous]
        [HttpPost]
        [Route("api/user/password-reset")]
        public async Task<HttpResponseMessage> SendPasswordResetLink([FromBody] EmailDTO emailModel, string resetPasswordLink)
        {
            try
            {
                if (resetPasswordLink == null)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Password reset link not found");
                }
                if (ModelState.IsValid)
                {
                    var user = userService.GetByEmail(emailModel.Email);
                    if (user == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NoContent, "User not found");
                    }
                    if (!user.EmailConfirmed)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.Forbidden, "Not allowed because email not confirmed");
                    }
                    string token = JwtManager.GenerateToken(user, 0, 1);
                    await EmailService.SendPasswordResetEmail(user.Email, token, resetPasswordLink);

                    return Request.CreateResponse(HttpStatusCode.OK, "Token successfully sent");
                }
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Email is not valid");
            }
            catch (EntityException e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, e);
            }
        }

        /// <summary>
        /// Sends email with link for user's email confirmation.
        /// </summary>
        /// <param name="emailModel"> User's email. </param>
        /// <param name="emailConfirmLink"> Link on the email confirm page. </param>
        [AllowAnonymous]
        [HttpPost]
        [Route("api/user/confirm-email")]
        public async Task<HttpResponseMessage> SendEmailConfirmLink([FromBody] EmailDTO emailModel, string emailConfirmLink)
        {
            try
            {
                if (emailConfirmLink == null)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Email confirmation link not found");
                }
                if (ModelState.IsValid)
                {
                    var user = userService.GetByEmail(emailModel.Email);
                    if (user == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NoContent, "User not found");
                    }
                    if (user.EmailConfirmed)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.Forbidden, "Email already confirmed");
                    }
                    string token = JwtManager.GenerateToken(user, 0, 1);
                    await EmailService.SendConfirmPasswordEmail(user.Email, token, emailConfirmLink);
                    return Request.CreateResponse(HttpStatusCode.OK, "Token successfully sent");
                }
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Email is not valid");
            }
            catch (EntityException e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, e);
            }
        }

        /// <summary>
        /// Returns statistics dto with number of tasks in different states for one user.
        /// </summary>
        [JwtAuthentication]
        [HttpGet]
        [Route("api/user/statistics")]
        public HttpResponseMessage GetStatistics()
        {
            var id = userIdentityService.GetUserId();
            var statistics = taskService.GetUserStatistics(id);
            if (statistics == null)
            {
                const string errorMessage = "No user with this id in database.";
                return Request.CreateResponse(HttpStatusCode.NoContent, errorMessage);
            }
            return Request.CreateResponse(HttpStatusCode.OK, statistics);
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
                const string errorMessage = "No user with this id in database.";
                return Request.CreateResponse(HttpStatusCode.NoContent, errorMessage);
            }
            if (HttpContext.Current.Request.Files.Count != 1)
            {
                const string errorMessage = "Only one image can be sent.";
                return Request.CreateResponse(HttpStatusCode.BadRequest, errorMessage);
            }
            try
            {
                var postedFile = HttpContext.Current.Request.Files[0];
                if (postedFile.ContentLength > 0)
                {
                    var allowedFileExtensions = new List<string>(Constants.ImageRestrictions.Extensions);
                    var extension = postedFile.FileName.Substring(postedFile.FileName.LastIndexOf('.')).ToLower();
                    if (!allowedFileExtensions.Contains(extension))
                    {
                        const string errorMessage = "Types allowed only .jpeg .jpg .png";
                        return Request.CreateResponse(HttpStatusCode.BadRequest, errorMessage);
                    }
                    const int maxContentLength = Constants.ImageRestrictions.MaxSize;
                    if (postedFile.ContentLength > maxContentLength)
                    {
                        const string errorMessage = "Please Upload a file upto 1 mb.";
                        return Request.CreateResponse(HttpStatusCode.BadRequest, errorMessage);
                    }
                    byte[] imageData;
                    using (var binaryReader = new BinaryReader(postedFile.InputStream))
                    {
                        imageData = binaryReader.ReadBytes(postedFile.ContentLength);
                    }
                    userService.SetImage(id, imageData, postedFile.FileName);
                    const string okMessage = "Successfully created image.";
                    return Request.CreateResponse(HttpStatusCode.OK, okMessage);
                }
                const string emptyImageMessage = "Empty image.";
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
                    const string errorMessage = "No user with this id in database.";
                    return Request.CreateResponse(HttpStatusCode.NoContent, errorMessage);
                }
                var dto = userService.GetImage(id);
                if (dto == null)
                {
                    const string message = "No image for this user in database.";
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
                var success = userService.UpdateById(id, value);
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
            const string message = "Incorrect request syntax or user does not exist.";
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
                var success = userService.BlockById(id);
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
        /// <param name="key"> String to match. </param>
        /// <param name="role"> Role criteria. </param>
        [JwtAuthentication]
        [Authorize(Roles = "Admin, Mentor")]
        [HttpGet]
        [Route("api/user/search")]
        public HttpResponseMessage Search(string key, string role)
        {
            if (key == null)
            {
                key = "";
            }
            var criteria = roleService.GetByName(role);
            var lines = key.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            int? searchParametr = null;
            if (role == Constants.Roles.Blocked)
            {
                searchParametr = Constants.Roles.BlockedIndex;
            }
            if (lines.Length > 2)
            {
                lines = lines.Take(2).ToArray();
            }
            var users = criteria != null ? userService.Search(lines, criteria.Id) :
                userService.Search(lines, searchParametr);
            if (users.Count != 0)
            {
                return Request.CreateResponse<IEnumerable<UserDTO>>(HttpStatusCode.OK, users);
            }
            const string message = "No users found.";
            return Request.CreateErrorResponse(HttpStatusCode.NoContent, message);
        }

        /// <summary>
        /// Search for user with match in first or lastname with role criteria.
        /// </summary>
        /// <param name="key"> String to match. </param>
        /// <param name="role"> Role criteria. </param>
        /// <param name="pageSize"> Ammount of users that you want to see on one page</param>
        /// <param name="pageNumber"> Page number</param>
        [JwtAuthentication]
        [Authorize(Roles = "Admin, Mentor")]
        [HttpGet]
        [Route("api/user/search")]
        public HttpResponseMessage Search(string key, string role, [FromUri]int pageSize, [FromUri]int pageNumber)
        {
            if (key == null)
            {
                key = "";
            }
            var criteria = roleService.GetByName(role);
            var lines = key.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            int? searchParametr = null;
            if (role == Constants.Roles.Blocked)
            {
                searchParametr = Constants.Roles.BlockedIndex;
            }
            if (lines.Length > 2)
            {
                lines = lines.Take(2).ToArray();
            }
            var users = criteria != null ? userService.Search(lines, pageSize, pageNumber, criteria.Id) :
                userService.Search(lines, pageSize, pageNumber, searchParametr);
            return Request.CreateResponse(HttpStatusCode.OK, users);
        }

        /// <summary>
        /// Updates user password by id.
        /// </summary>
        /// <param name="password"> New password value. </param>
        /// <param name="id"> Users Id. </param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPut]
        [Route("api/user/resetpasswotd")]
        public HttpResponseMessage ResetPassword([FromBody]string password, int id)
        {
            try
            {
                var success = userService.UpdatePassword(id, password);
                if (success)
                {
                    const string okMessage = "Succesfully updated password.";
                    tracer.Info(Request, ControllerContext.ControllerDescriptor.ControllerType.FullName, okMessage);
                    return Request.CreateResponse(HttpStatusCode.OK, okMessage);
                }
                const string noUserMessage = "No user with this ID in database.";
                return Request.CreateResponse(HttpStatusCode.NoContent, noUserMessage);
            }
            catch (EntityException e)
            {
                tracer.Error(Request, ControllerContext.ControllerDescriptor.ControllerType.FullName, e);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }
        }

        /// <summary>
        /// Updates current user password.
        /// </summary>
        /// <param name="value"> New password value. </param>
        /// <returns></returns>
        [JwtAuthentication]
        [HttpPut]
        [Route("api/user/newpassword")]
        public HttpResponseMessage UpdatePassword([FromBody]string value)
        {
            var id = userIdentityService.GetUserId();
            return ResetPassword(value, id);
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
            const string message = "No roles in database.";
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
