using System.Linq;
using LearnWithMentorDAL.Entities;

namespace LearnWithMentorDAL.Repositories
{
    public class TaskRepository: BaseRepository<Entities.Task>, ITaskRepository
    {
        public TaskRepository(LearnWithMentor_DBEntities context) : base(context)
        {
        }
        public Entities.Task Get(int id)
        {
            return context.Tasks.FirstOrDefault(t => t.Id == id);
        }
    }
}
