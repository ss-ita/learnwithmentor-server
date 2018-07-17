using System.Linq;
using LearnWithMentorDAL.Entities;
using LearnWithMentorDAL.Repositories.Interfaces;

namespace LearnWithMentorDAL.Repositories
{
    public class UserTaskRepository: BaseRepository<UserTask>, IUserTaskRepository
    {
        public UserTaskRepository(LearnWithMentor_DBEntities context) : base(context)
        {
        }

        public UserTask Get(int id)
        {
            return Context.UserTasks.FirstOrDefault(t => t.Id == id);
        }

        public int GetNumberOfTasksByState(int userId, string state)
        {
            return Context.UserTasks.Where(ut => ut.User_Id == userId).Count(ut => ut.State == state);
        }

        public UserTask GetByPlanTaskForUser(int planTaskId, int userId)
        {
            return Context.UserTasks.FirstOrDefault(ut => ut.User_Id == userId && ut.PlanTask_Id == planTaskId);
        }
    }
}
