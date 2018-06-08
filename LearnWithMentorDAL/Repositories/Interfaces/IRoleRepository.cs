using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LearnWithMentorDAL.Entities;

namespace LearnWithMentorDAL.Repositories
{
    public interface IRoleRepository : IRepository<Role>
    {
        Role Get(int id);
        bool TryGetByName(string name, out Role role);
    }
}
