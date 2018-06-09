using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        IEnumerable<User> Search(string[] str, int? role_id);
    }
}
