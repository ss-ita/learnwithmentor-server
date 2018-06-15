using System.Collections.Generic;
using LearnWithMentorDTO;

namespace LearnWithMentorBLL.Interfaces
{
    public interface IRoleService
    {
        RoleDTO Get(int id);
        List<RoleDTO> GetAllRoles();
        RoleDTO GetByName(string name);
    }
}
