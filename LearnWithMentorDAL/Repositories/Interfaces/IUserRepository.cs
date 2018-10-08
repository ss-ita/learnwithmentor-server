using System.Collections.Generic;
using System.Threading.Tasks;
using LearnWithMentorDAL.Entities;

namespace LearnWithMentorDAL.Repositories.Interfaces
{
    public interface IUserRepository: IRepository<GroupUser>
    {
        Task<GroupUser> GetAsync(int id);
        Task<GroupUser> GetByEmailAsync(string email);
        Task<IEnumerable<GroupUser>> SearchAsync(string[] searchString, int? roleId);
        Task<IEnumerable<GroupUser>> GetUsersByRoleAsync(int roleId);
        Task<IEnumerable<GroupUser>> GetUsersByGroupAsync(int groupId);
        Task<IEnumerable<GroupUser>> GetUsersByStateAsync(bool state);
        Task<string> ExtractFullNameAsync(int? id);
        Task<string> GetImageBase64Async(int userId);
        Task<IEnumerable<GroupUser>> GetUsersNotInGroupAsync(int groupId);
        Task<bool> ContainsIdAsync(int id);
    }
}
