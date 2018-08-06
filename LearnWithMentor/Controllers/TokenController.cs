using System.Net;
using System.Net.Http;
using System.Web.Http;
using LearnWithMentor.Models;
using LearnWithMentorBLL.Interfaces;
using LearnWithMentorDTO;

namespace LearnWithMentor.Controllers
{
    /// <summary>
    /// Controller for tokens.
    /// </summary>
    public class TokenController : ApiController
    {
        private readonly IUserService userService;

        /// <summary>
        /// Creates instance of TokenController.
        /// </summary>
        /// <param name="userService"> Dependency injection parameter. </param>
        public TokenController(IUserService userService)
        {
            this.userService = userService;
        }

        /// <summary>
        /// Returns new token for user.
        /// </summary>
        /// <param name="value"> User data. </param>
        /// <returns></returns>
        [AllowAnonymous]
        public HttpResponseMessage Post([FromBody]UserLoginDTO value)
        {
            UserIdentityDTO user = null;
            if (ModelState.IsValid)
            {
                if (CheckUser(value.Email, value.Password, out user))
                {
                    return Request.CreateResponse(HttpStatusCode.OK, JwtManager.GenerateToken(user));
                }
            }
            var message = " Not valid logination data.";
            if (user != null && user.Blocked == true) message = "This user is blocked!";
            return Request.CreateErrorResponse(HttpStatusCode.Unauthorized, message);
        }

        /// <summary>
        /// Checks if user has correct data to enter the system.
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public bool CheckUser(string email, string password, out UserIdentityDTO user)
        {

            user = userService.GetByEmail(email);
            if (user == null || user.Blocked == true)
            {
                return false;
            }
            var result = BCrypt.Net.BCrypt.Verify(password, user.Password);
            return result;
        }

        /// <summary>
        /// Releases memory
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            userService.Dispose();
            base.Dispose(disposing);
        }
    }
}
