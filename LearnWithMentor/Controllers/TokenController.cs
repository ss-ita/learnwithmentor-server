using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
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
        public async  Task<HttpResponseMessage> PostAsync([FromBody]UserLoginDto value)
        {
            UserIdentityDto user = null;
            user = await CheckUserAsync(value.Email);
            bool UserCheck =  BCrypt.Net.BCrypt.Verify(value.Password, user.Password);
            if ((ModelState.IsValid) && (UserCheck))
            {
                return Request.CreateResponse(HttpStatusCode.OK, JwtManager.GenerateToken(user));
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

        public async Task<UserIdentityDto> CheckUserAsync(string email)
        {
            return await userService.GetByEmailAsync(email);
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
