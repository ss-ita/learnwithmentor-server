using System.Collections.Generic;
using LearnWithMentorDTO;
using System.Threading.Tasks;

namespace LearnWithMentorBLL.Interfaces
{
    public interface IRoleService : IDisposableService
    {
        Task<RoleDto> GetAsync(int id);
        List<RoleDto> GetAllRoles();
        Task<RoleDto> GetByNameAsync(string name);
    }
}
