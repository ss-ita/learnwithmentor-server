using System.Linq;
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
            return Context.Roles.FirstOrDefault(r => r.Id == id);
        }
        public bool TryGetByName(string name, out Role role)
        {
            role = Context.Roles.FirstOrDefault(r => r.Name == name);
            return role != null;
        }
    }
}
