using System.Collections.Generic;
using System.Threading.Tasks;
using LearnWithMentorDTO;

namespace LearnWithMentorBLL.Interfaces
{
    public interface IUserService : IDisposableService
    {
        UserIdentityDto GetByEmail(string email);
        List<UserDto> GetAllUsers();
        Task<bool> BlockById(int id);
        Task<bool> ConfirmEmailById(int id);
        Task<UserDto> Get(int id);
        Task<bool> UpdateById(int id, UserDto user);
        Task<bool> Add(UserRegistrationDto userLoginDTO);
        Task<bool> SetImage(int id, byte[] image, string imageName);
        Task<ImageDto> GetImage(int id);
        Task<bool> UpdatePassword(int userId, string password);
        List<UserDto> Search(string[] str, int? roleId);
        List<UserDto> GetUsersByRole(int roleId);
        List<UserDto> GetUsersByState(bool state);
        bool ContainsId(int id);
        PagedListDto<UserDto> GetUsers(int pageSize, int pageNumber = 1);
        PagedListDto<UserDto> Search(string[] str, int pageSize, int pageNumber, int? roleId);
        PagedListDto<UserDto> GetUsersByRole(int roleId, int pageSize, int pageNumber);
        PagedListDto<UserDto> GetUsersByState(bool state, int pageSize, int pageNumber);
    }
}
