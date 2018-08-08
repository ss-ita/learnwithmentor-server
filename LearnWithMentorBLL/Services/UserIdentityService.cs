using System.Security.Claims;
using System.Web;
using LearnWithMentorBLL.Interfaces;

namespace LearnWithMentorBLL.Services
{
    public class UserIdentityService:  IUserIdentityService
    {
        public int GetUserId()
        {
            var identity = HttpContext.Current.User.Identity as ClaimsIdentity;
            if (identity == null)
            {
                return -1;
            }
            return int.Parse(identity.FindFirst("Id").Value);
        }

        public string GetUserRole()
        {
            var identity = HttpContext.Current.User.Identity as ClaimsIdentity;
            return identity == null ? "" : identity.FindFirst(identity.RoleClaimType).Value;
        }
    }
}
