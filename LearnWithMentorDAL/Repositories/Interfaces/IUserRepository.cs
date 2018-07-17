using System.Collections.Generic;
using LearnWithMentorDAL.Entities;

namespace LearnWithMentorDAL.Repositories.Interfaces
{
    public interface IUserRepository: IRepository<User>
    {
        User Get(int id);
        User GetByEmail(string email);
        IEnumerable<User> Search(string[] str, int? roleId);
        IEnumerable<User> GetUsersByRole(int roleId);
        IEnumerable<User> GetUsersByGroup(int groupId);
        IEnumerable<User> GetUsersByState(bool state);
        string ExtractFullName(int? id);
        string GetImageBase64(int userId);
        IEnumerable<User> GetUsersNotInGroup(int groupId);
        bool ContainsId(int id);
    }
}
