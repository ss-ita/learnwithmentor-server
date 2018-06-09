using System.Linq;
using LearnWithMentorDAL.Entities;

namespace LearnWithMentorDAL.Repositories
{
    public class RoleRepository : BaseRepository<Role>, IRoleRepository
    {
        public RoleRepository(LearnWithMentor_DBEntities context) : base(context)
        {
        }
        public Role Get(int id)
        {
            return context.Roles.FirstOrDefault(r => r.Id == id);
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
