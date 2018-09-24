using System.Linq;
using System.Data.Entity;
using System.Threading.Tasks;
using LearnWithMentorDAL.Entities;
using LearnWithMentorDAL.Repositories.Interfaces;

namespace LearnWithMentorDAL.Repositories
{
    public class RoleRepository : BaseRepository<Role>, IRoleRepository
    {
        public RoleRepository(LearnWithMentorContext context) : base(context)
        {
        }

        public Task<Role> Get(int id)
        {
           return Context.Roles.FirstOrDefaultAsync(r => r.Id == id);
        }

        public Task<Role> TryGetByName(string name)
        {
            return Context.Roles.FirstOrDefaultAsync(r => r.Name == name);
        }
    }
}
