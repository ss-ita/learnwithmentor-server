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
        public UserTask GetByPlanTaskForUser(int planTaskId, int userId)
        {
            return context.UserTasks.FirstOrDefault(t => t.User_Id == userId && t.PlanTask_Id == planTaskId);
        }
    }
}
