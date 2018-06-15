using System.Collections.Generic;
using System.Linq;
using LearnWithMentorDAL.Entities;
using LearnWithMentorDTO;

namespace LearnWithMentorDAL.Repositories
{
    public class UserRepository: BaseRepository<User>, IUserRepository
    {
        public UserRepository(LearnWithMentor_DBEntities context) : base(context)
        {
        }

        public User Get(int id)
        {
            return context.Users.FirstOrDefault(u => u.Id == id);
        }
        
        public IEnumerable<User> Search(string[] str, int? roleId)
        {
            List<User> result = new List<User>();
            foreach (var s in str)
            {
                IQueryable<User> found;
                if (roleId == null)
                {
                    found = context.Users.Where(u => u.FirstName.Contains(s) || u.LastName.Contains(s));
                }
                else if (roleId == -1)
                {
                    found = context.Users.Where(u => (u.FirstName.Contains(s) || u.LastName.Contains(s)) && (u.Blocked));
                }
                else
                {
                    found = context.Users.Where(u => u.Role_Id == roleId).Where(u => u.FirstName.Contains(s) || u.LastName.Contains(s));
                }
                foreach (var f in found)
                {
                    if (!result.Contains(f))
                    {
                        result.Add(f);
                    }
                }
            }
            return result;
        }

        public IEnumerable<User> GetUsersByRole(int role_id)
        {
            return context.Users.Where(u => u.Role_Id == role_id);
        }
    }
}
