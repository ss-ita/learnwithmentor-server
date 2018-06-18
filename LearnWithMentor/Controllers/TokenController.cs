using System.Linq;
using System.Net;
using System.Web.Http;
using LearnWithMentorDAL.Entities;
using LearnWithMentorDAL.UnitOfWork;
using LearnWithMentorDTO;


namespace LearnWithMentor.Controllers
{
    public class TokenController : ApiController
    {
        private IUnitOfWork UoW;
        public TokenController()
        {
            UoW = new UnitOfWork(new LearnWithMentor_DBEntities());
        }
        [AllowAnonymous]

        public string Post([FromBody]UserLoginDTO value)
        {
            if (ModelState.IsValid)
            {

                int id;
                if (CheckUser(value.Email, value.Password, out id))
                {
                    return JwtManager.GenerateToken(value.Email, id);
                }
            }

            throw new HttpResponseException(HttpStatusCode.Unauthorized);
        }

        public bool CheckUser(string email, string password, out int id)
        {
            // should check in the database
            //need add more specific method for email search
            id = 0;
            var allusers = UoW.Users.GetAll();
            var user = allusers.FirstOrDefault(u => u.Email == email);
            if (user == null) return false;
            id = user.Id;
            var result = BCrypt.Net.BCrypt.Verify(password, BCrypt.Net.BCrypt.HashPassword(user.Password));
            return result;
        }
    }
}
