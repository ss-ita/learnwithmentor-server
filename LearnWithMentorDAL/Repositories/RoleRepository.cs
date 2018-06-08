using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LearnWithMentorDAL.Entities;

namespace LearnWithMentorDAL.Repositories
{
    public class RoleRepository : BaseRepository<Role>, IRoleRepository
    {
        public RoleRepository(LearnWithMentor_DBEntities _context) : base(_context)
        {
        }
        public Role Get(int id)
        {
            return context.Roles.Where(r => r.Id == id).FirstOrDefault();
        }
        public bool TryGetByName(string name, out Role role)
        {
            var ret = context.Roles.Where(r => r.Name == name);
            if (ret.Count() != 0)
            {
                role = ret.FirstOrDefault();
            }
            else
            {
                role = null;
            }
            return ret.Count() != 0;
        }
    }
}
