using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LearnWithMentorDAL.Entities;

namespace LearnWithMentorDAL.Repositories
{
    interface IUserRepository: IRepository<User>
    {
        User Get(int id);
    }
}
