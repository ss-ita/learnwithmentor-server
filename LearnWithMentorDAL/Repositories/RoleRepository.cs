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
            role = context.Roles.FirstOrDefault(r => r.Name == name);
            return role != null;
        }
    }
}
