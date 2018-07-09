﻿using System.Net;
using System.Net.Http;
using System.Web.Http;
using LearnWithMentor.Models;
using LearnWithMentorBLL.Interfaces;
using LearnWithMentorBLL.Services;
using LearnWithMentorDTO;

namespace LearnWithMentor.Controllers
{
    public class TokenController : ApiController
    {
        private readonly IUserService userService;
        public TokenController(IUserService userService)
        {
            this.userService = userService;
        }
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

            string message = " Not valid logination data.";
            if (user != null && user.Blocked == true) message = "This user is blocked!";
            return Request.CreateErrorResponse(HttpStatusCode.Unauthorized, message);
        }

        public bool CheckUser(string email, string password, out UserIdentityDTO user)
        {

            user = userService.GetByEmail(email);
            if (user == null || user.Blocked== true) return false;
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
