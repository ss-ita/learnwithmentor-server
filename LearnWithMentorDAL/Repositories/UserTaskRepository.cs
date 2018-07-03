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

        public int GetNumberOfTasksByState(int userId, string state)
        {
            return context.UserTasks.Where(ut => ut.User_Id == userId).Count(ut => ut.State == state);
        }

        public UserTask GetByPlanTaskForUser(int planTaskId, int userId)
        {
            return context.UserTasks.FirstOrDefault(ut => ut.User_Id == userId && ut.PlanTask_Id == planTaskId);
        }
    }
}
