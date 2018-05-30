using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LearnWithMentorDAL.Entities;

namespace LearnWithMentorDAL.Repositories
{
    public class UserTaskRepository: BaseRepository<UserTask>, IUserTaskRepository
    {
        public UserTaskRepository(LearnWithMentor_DBEntities _context) : base(_context)
        {
        }
        public UserTask Get(int id)
        {
            return context.UserTasks.Where(t => t.Id == id).FirstOrDefault();
        }
    }
}
