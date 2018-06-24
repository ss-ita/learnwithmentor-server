using System.Collections.Generic;
using LearnWithMentorDAL.Entities;
using LearnWithMentorDTO;

namespace LearnWithMentorDAL.Repositories
{
    public interface IUserRepository: IRepository<User>
    {
        User Get(int id);
        User GetByEmail(string email);
        IEnumerable<User> Search(string[] str, int? role_id);
        IEnumerable<User> GetUsersByRole(int role_id);
        IEnumerable<User> GetUsersByGroup(int groupId);
        IEnumerable<User> GetUsersByState(bool state);
        string ExtractFullName(int? id);
    }
}
