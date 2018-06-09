using System.Linq;
using LearnWithMentorDAL.Entities;

namespace LearnWithMentorDAL.Repositories
{
    public class UserTaskRepository: BaseRepository<UserTask>, IUserTaskRepository
    {
        public UserTaskRepository(LearnWithMentor_DBEntities context) : base(context)
        {
        }
        public UserTask Get(int id)
        {
            return context.UserTasks.FirstOrDefault(t => t.Id == id);
        }
    }
}
