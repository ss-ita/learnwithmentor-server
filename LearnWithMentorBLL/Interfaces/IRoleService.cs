using System.Collections.Generic;
using LearnWithMentorDTO;

namespace LearnWithMentorBLL.Interfaces
{
    public interface IRoleService : IDisposableService
    {
        RoleDto Get(int id);
        List<RoleDto> GetAllRoles();
        RoleDto GetByName(string name);
    }
}
