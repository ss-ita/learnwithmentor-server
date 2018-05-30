using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LearnWithMentorDAL.Entities;

namespace LearnWithMentorDAL.Repositories
{
    public interface ITaskRepository: IRepository<Entities.Task>
    {
        Entities.Task Get(int id);
    }
}
