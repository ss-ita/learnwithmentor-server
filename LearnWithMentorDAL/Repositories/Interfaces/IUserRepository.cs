using System.Collections.Generic;
using System.Threading.Tasks;
using LearnWithMentorDAL.Entities;

namespace LearnWithMentorDAL.Repositories.Interfaces
{
    public interface IUserRepository: IRepository<User>
    {
        Task<User> Get(int id);
        Task<User> GetByEmail(string email);
        Task<IEnumerable<User>> Search(string[] searchString, int? roleId);
        Task<IEnumerable<User>> GetUsersByRole(int roleId);
        Task<IEnumerable<User>> GetUsersByGroup(int groupId);
        Task<IEnumerable<User>> GetUsersByState(bool state);
        Task<string> ExtractFullName(int? id);
        Task<string> GetImageBase64(int userId);
        Task<IEnumerable<User>> GetUsersNotInGroup(int groupId);
        Task<bool> ContainsId(int id);
    }
}
