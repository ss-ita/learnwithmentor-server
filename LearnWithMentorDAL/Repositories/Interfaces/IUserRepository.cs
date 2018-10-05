using System.Collections.Generic;
using System.Threading.Tasks;
using LearnWithMentorDAL.Entities;

namespace LearnWithMentorDAL.Repositories.Interfaces
{
    public interface IUserRepository: IRepository<User>
    {
        Task<User> GetAsync(int id);
        Task<User> GetByEmailAsync(string email);
        Task<IEnumerable<User>> SearchAsync(string[] searchString, int? roleId);
        Task<IEnumerable<User>> GetUsersByRoleAsync(int roleId);
        Task<IEnumerable<User>> GetUsersByGroupAsync(int groupId);
        Task<IEnumerable<User>> GetUsersByStateAsync(bool state);
        Task<string> ExtractFullNameAsync(int? id);
        Task<string> GetImageBase64Async(int userId);
        Task<IEnumerable<User>> GetUsersNotInGroupAsync(int groupId);
        Task<bool> ContainsIdAsync(int id);
    }
}
