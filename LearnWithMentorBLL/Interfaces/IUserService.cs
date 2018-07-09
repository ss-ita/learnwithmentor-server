using System.Collections.Generic;
using LearnWithMentorDTO;

namespace LearnWithMentorBLL.Interfaces
{
    public interface IUserService : IDisposableService
    {
        UserDTO Get(int id);
        UserIdentityDTO GetByEmail(string email);
        List<UserDTO> GetAllUsers();
        bool BlockById(int id);
        bool UpdateById(int id, UserDTO user);
        bool Add(UserRegistrationDTO dto);
        List<UserDTO> Search(string[] str, int? role_id);
        List<UserDTO> GetUsersByRole(int roleId);
        List<UserDTO> GetUsersByState(bool state);
        bool SetImage(int planId, byte[] image, string imageName);
        ImageDTO GetImage(int id);
        bool ContainsId(int id);
        bool UpdatePassword(int userId, string password);
    }
}
