using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LearnWithMentorDAL.Entities;

namespace LearnWithMentorDAL.Repositories
{
    class RoleRepository : BaseRepository<Role>, IRoleRepository
    {
        public RoleRepository(LearnWithMentor_DBEntities _context) : base(_context)
        {
        }
        public Role Get(int id)
        {
            return context.Roles.Where(r => r.Id == id).FirstOrDefault();
        }
    }
}
