using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LearnWithMentorDAL.Entities;

namespace LearnWithMentorDAL.Repositories
{
    public class TaskRepository: BaseRepository<Entities.Task>, ITaskRepository
    {
        public TaskRepository(LearnWithMentor_DBEntities _context) : base(_context)
        {
        }
        public Entities.Task Get(int id)
        {
            return context.Tasks.Where(t => t.Id == id).FirstOrDefault();
        }
    }
}
