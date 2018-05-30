using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LearnWithMentorDAL.Entities;

namespace LearnWithMentorDAL.Repositories
{
    public class UserRepository: BaseRepository<User>, IUserRepository
    {
        public UserRepository(LearnWithMentor_DBEntities _context) : base(_context)
        {
        }
        public User Get(int id)
        {
            return context.Users.Where(u => u.Id == id).FirstOrDefault();
        }
    }
}
