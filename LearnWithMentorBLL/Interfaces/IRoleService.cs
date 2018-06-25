using System.Collections.Generic;
using LearnWithMentorDTO;

namespace LearnWithMentorBLL.Interfaces
{
    public interface IRoleService : IDisposableService
    {
        RoleDTO Get(int id);
        List<RoleDTO> GetAllRoles();
        RoleDTO GetByName(string name);
    }
}
