using System.Linq;
using System.Threading.Tasks;
using System.Data.Entity;
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
            Task<UserTask> findUserTask = Context.UserTasks.FirstOrDefaultAsync(task => task.Id == id);
            return findUserTask.GetAwaiter().GetResult();
        }

        public int GetNumberOfTasksByState(int userId, string state)
        {
            Task<int> countTasksWithState = Context.UserTasks.Where(userTask => userTask.User_Id == userId).CountAsync(userTask => userTask.State == state);
            return countTasksWithState.GetAwaiter().GetResult();
        }

        public UserTask GetByPlanTaskForUser(int planTaskId, int userId)
        {
            Task<UserTask> findUserTask = Context.UserTasks.FirstOrDefaultAsync(userTask => userTask.User_Id == userId && userTask.PlanTask_Id == planTaskId);
            return findUserTask.GetAwaiter().GetResult();
        }
    }
}
