using System.Collections.Generic;
using LearnWithMentorDTO;
using System.Threading.Tasks;

namespace LearnWithMentorBLL.Interfaces
{
    public interface IRoleService : IDisposableService
    {
        Task<RoleDto> Get(int id);
        List<RoleDto> GetAllRoles();
        Task<RoleDto> GetByName(string name);
    }
}
