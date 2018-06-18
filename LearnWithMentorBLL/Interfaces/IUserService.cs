using System.Collections.Generic;
using LearnWithMentorDTO;

namespace LearnWithMentorBLL.Interfaces
{
    public interface IUserService
    {
        UserDTO Get(int id);
        List<UserDTO> GetAllUsers();
        bool BlockById(int id);
        bool UpdateById(int id, UserDTO user);
        bool Add(UserLoginDTO dto);
        List<UserDTO> Search(string[] str, int? role_id);
        List<UserDTO> GetUsersByRole(int roleId);
    }
}
