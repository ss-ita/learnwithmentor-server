using System.Linq;
using System.Data.Entity;
using System.Threading.Tasks;
using LearnWithMentorDAL.Entities;
using LearnWithMentorDAL.Repositories.Interfaces;

namespace LearnWithMentorDAL.Repositories
{
    public class RoleRepository : BaseRepository<Role>, IRoleRepository
    {
        public RoleRepository(LearnWithMentor_DBEntities context) : base(context)
        {
        }

        public Role Get(int id)
        {
            Task<Role> findRole = Context.Roles.FirstOrDefaultAsync(r => r.Id == id);
            return findRole.GetAwaiter().GetResult();
        }

        public bool TryGetByName(string name, out Role role)
        {
            Task<Role> findRole = Context.Roles.FirstOrDefaultAsync(r => r.Name == name);
            role = findRole.GetAwaiter().GetResult();
            return role != null;
        }
    }
}
