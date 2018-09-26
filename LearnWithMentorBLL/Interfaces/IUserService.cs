using System.Collections.Generic;
using System.Threading.Tasks;
using LearnWithMentorDTO;

namespace LearnWithMentorBLL.Interfaces
{
    public interface IUserService : IDisposableService
    {
        Task<UserDto> Get(int id);
        Task<UserIdentityDto> GetByEmail(string email);
        Task<List<UserDto>> GetAllUsers();
        Task<bool> BlockById(int id);
        Task<bool> ConfirmEmailById(int id);
        Task<bool> UpdateById(int id, UserDto user);
        Task<bool> Add(UserRegistrationDto userLoginDTO);
        Task<List<UserDto>> Search(string[] str, int? roleId);
        Task<List<UserDto>> GetUsersByRole(int roleId);
        Task<List<UserDto>> GetUsersByState(bool state);
        Task<bool> SetImage(int id, byte[] image, string imageName);
        Task<ImageDto> GetImage(int id);
        Task<bool> ContainsId(int id);
        Task<bool> UpdatePassword(int userId, string password);
        Task<PagedListDto<UserDto>> GetUsers(int pageSize, int pageNumber = 1);
        Task<PagedListDto<UserDto>> Search(string[] str, int pageSize, int pageNumber, int? roleId);
        Task<PagedListDto<UserDto>> GetUsersByRole(int roleId, int pageSize, int pageNumber);
        Task<PagedListDto<UserDto>> GetUsersByState(bool state, int pageSize, int pageNumber);
    }
}
