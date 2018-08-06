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
        bool ConfirmEmailById(int id);
        bool UpdateById(int id, UserDTO user);
        bool Add(UserRegistrationDTO dto);
        List<UserDTO> Search(string[] str, int? roleId);
        List<UserDTO> GetUsersByRole(int roleId);
        List<UserDTO> GetUsersByState(bool state);
        bool SetImage(int planId, byte[] image, string imageName);
        ImageDTO GetImage(int id);
        bool ContainsId(int id);
        bool UpdatePassword(int userId, string password);
        PagedListDTO<UserDTO> GetUsers(int pageSize, int pageNumber = 1);
        PagedListDTO<UserDTO> Search(string[] str, int pageSize, int pageNumber, int? roleId);
        PagedListDTO<UserDTO> GetUsersByRole(int roleId, int pageSize, int pageNumber);
        PagedListDTO<UserDTO> GetUsersByState(bool state, int pageSize, int pageNumber);
    }
}
