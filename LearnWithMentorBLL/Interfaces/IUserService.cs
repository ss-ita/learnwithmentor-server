using System.Collections.Generic;
using LearnWithMentorDto;

namespace LearnWithMentorBLL.Interfaces
{
    public interface IUserService : IDisposableService
    {
        UserDto Get(int id);
        UserIdentityDto GetByEmail(string email);
        List<UserDto> GetAllUsers();
        bool BlockById(int id);
        bool ConfirmEmailById(int id);
        bool UpdateById(int id, UserDto user);
        bool Add(UserRegistrationDto userLoginDTO);
        List<UserDto> Search(string[] str, int? roleId);
        List<UserDto> GetUsersByRole(int roleId);
        List<UserDto> GetUsersByState(bool state);
        bool SetImage(int id, byte[] image, string imageName);
        ImageDto GetImage(int id);
        bool ContainsId(int id);
        bool UpdatePassword(int userId, string password);
        PagedListDto<UserDto> GetUsers(int pageSize, int pageNumber = 1);
        PagedListDto<UserDto> Search(string[] str, int pageSize, int pageNumber, int? roleId);
        PagedListDto<UserDto> GetUsersByRole(int roleId, int pageSize, int pageNumber);
        PagedListDto<UserDto> GetUsersByState(bool state, int pageSize, int pageNumber);
    }
}
