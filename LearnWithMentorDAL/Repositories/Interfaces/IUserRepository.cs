using System.Collections.Generic;
using LearnWithMentorDAL.Entities;
using LearnWithMentorDTO;

namespace LearnWithMentorDAL.Repositories
{
    public interface IUserRepository: IRepository<User>
    {
        User Get(int id);
        void RemoveById(int id);
        void UpdateById(int id, UserDTO user);
        void Add(UserDTO dto, string password);
        IEnumerable<User> Search(string[] str);
    }
}
