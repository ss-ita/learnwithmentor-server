using System.Collections.Generic;
using LearnWithMentorDAL.Entities;
using LearnWithMentorDTO;

namespace LearnWithMentorDAL.Repositories
{
    public interface IUserRepository: IRepository<User>
    {
        User Get(int id);
        bool BlockById(int id);
        bool UpdateById(int id, UserDTO user);
        bool Add(UserLoginDTO dto);
        IEnumerable<User> Search(string[] str, int? role_id);
        IEnumerable<User> GetUsersByRole(int role_id);
        string ExtractFullName(int? id);

    }
}
