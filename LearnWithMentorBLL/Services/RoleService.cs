using System.Collections.Generic;
using LearnWithMentorBLL.Interfaces;
using LearnWithMentorDTO;
using LearnWithMentorDAL.UnitOfWork;
using System.Threading.Tasks;

namespace LearnWithMentorBLL.Services
{
    public class RoleService : BaseService, IRoleService
    {
        public RoleService(IUnitOfWork db) : base(db)
        {
        }
        public async Task<RoleDto> GetAsync(int id)
        {
            var role = await db.Roles.Get(id);
            return role == null ? null :
                new RoleDto(role.Id, role.Name);
        }
        public List<RoleDto> GetAllRoles()
        {
            var roles = db.Roles.GetAll();
            if (roles == null)
            {
                return null;
            }
            var dtos = new List<RoleDto>();
            foreach (var role in roles)
            {
                dtos.Add(new RoleDto(role.Id, role.Name));
            }
            return dtos;
        }
        public async Task<RoleDto> GetByNameAsync(string name)
        {
            var role = await db.Roles.TryGetByName(name);
            if (role == null)
            {
                return null;
            }
            return new RoleDto(role.Id, role.Name);
        }
    }
}
