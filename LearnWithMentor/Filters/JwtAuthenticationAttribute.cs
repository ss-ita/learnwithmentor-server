using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Filters;
using LearnWithMentor.Models;
using LearnWithMentorBLL.Interfaces;
using LearnWithMentorBLL.Services;
using LearnWithMentorDAL.UnitOfWork;

namespace LearnWithMentor.Filters
{
    public class JwtAuthenticationAttribute : Attribute, IAuthenticationFilter
    {
        public string Realm { get; set; }
        public bool AllowMultiple => false;
        private readonly IUserService userService = new UserService(new UnitOfWork(new LearnWithMentorDAL.Entities.LearnWithMentor_DBEntities()));

        public async Task AuthenticateAsync(HttpAuthenticationContext context, CancellationToken cancellationToken)
        {
            var request = context.Request;
            var authorization = request.Headers.Authorization;
            if (authorization == null || authorization.Scheme != "Bearer")
            {
                return;
            }

            if (string.IsNullOrEmpty(authorization.Parameter))
            {
                context.ErrorResult = new AuthenticationFailureResult("Missing Jwt Token", request);
                return;
            }

            var token = authorization.Parameter;
            var principal = await AuthenticateJwtToken(token);
            if (principal == null)
            {
                context.ErrorResult = new AuthenticationFailureResult("Invalid token", request);
            }
            else
            {
                context.Principal = principal;
            }
        }
        
        public static bool ValidateToken(string token, out string userEmail)
        {
            if (ValidateToken(token, out string email, out string userrole))
            {
                userEmail = email;
                return true;
            }
            userEmail = null;
            return false;
        }

        private static bool ValidateToken(string token, out string email,out string userrole)
        {
            email = null;
            userrole = null;
            var simplePrinciple = JwtManager.GetPrincipal(token);
            var identity = simplePrinciple?.Identity as ClaimsIdentity;
            if (identity == null)
            {
                return false;
            }

            if (!identity.IsAuthenticated)
            {
                return false;
            }

            var useremailClaim = identity.FindFirst(ClaimTypes.Email);
            email = useremailClaim?.Value;
            var userroleClaim = identity.FindFirst(ClaimTypes.Role);
            userrole = userroleClaim?.Value;
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(userroleClaim?.Value))
            {
                return false;
            }

            return true;
        }

        protected Task<IPrincipal> AuthenticateJwtToken(string token)
        {
            if (ValidateToken(token, out string email, out string userrole))
            {
                var userDTO = userService.GetByEmail(email);
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Role, userrole),
                    new Claim("Id", userDTO.Id.ToString()),
                    new Claim(ClaimTypes.Name, userDTO.FirstName)
                };
                var identity = new ClaimsIdentity(claims, "Jwt");
                IPrincipal user = new ClaimsPrincipal(identity);
                return Task.FromResult(user);
            }

            return Task.FromResult<IPrincipal>(null);
        }

        public Task ChallengeAsync(HttpAuthenticationChallengeContext context, CancellationToken cancellationToken)
        {
            Challenge(context);
            return Task.FromResult(0);
        }

        private void Challenge(HttpAuthenticationChallengeContext context)
        {
            string parameter = null;
            if (!string.IsNullOrEmpty(Realm))
            {
                parameter = "realm=\"" + Realm + "\"";
            }
            context.ChallengeWith("Bearer", parameter);
        }
    }
}