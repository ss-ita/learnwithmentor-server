using System.Collections.Generic;
using System.Threading.Tasks;
using LearnWithMentorDTO;

namespace LearnWithMentorBLL.Interfaces
{
    public interface IUserService : IDisposableService
    {
        Task<PagedListDto<UserDto>> GetUsers(int pageSize, int pageNumber = 1);
        Task<UserDto> GetAsync(int id);
        Task<UserIdentityDto> GetByEmailAsync(string email);
        Task<List<UserDto>> GetAllUsersAsync();
        Task<bool> BlockByIdAsync(int id);
        Task<bool> ConfirmEmailByIdAsync(int id);
        Task<bool> UpdateByIdAsync(int id, UserDto user);
        Task<bool> AddAsync(UserRegistrationDto userLoginDTO);
        Task<List<UserDto>> SearchAsync(string[] str, int? roleId);
        Task<List<UserDto>> GetUsersByRoleAsync(int roleId);
        Task<List<UserDto>> GetUsersByStateAsync(bool state);
        Task<bool> SetImageAsync(int id, byte[] image, string imageName);
        Task<ImageDto> GetImageAsync(int id);
        Task<bool> ContainsIdAsync(int id);
        Task<bool> UpdatePasswordAsync(int userId, string password);
        Task<PagedListDto<UserDto>> SearchAsync(string[] str, int pageSize, int pageNumber, int? roleId);
        Task<PagedListDto<UserDto>> GetUsersByRoleAsync(int roleId, int pageSize, int pageNumber);
        Task<PagedListDto<UserDto>> GetUsersByStateAsync(bool state, int pageSize, int pageNumber);
    }
}
