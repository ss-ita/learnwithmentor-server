using System.Linq;
using System.Threading.Tasks;
using System.Data.Entity;
using LearnWithMentorDAL.Entities;
using LearnWithMentorDAL.Repositories.Interfaces;

namespace LearnWithMentorDAL.Repositories
{
    public class UserTaskRepository: BaseRepository<UserTask>, IUserTaskRepository
    {
        public UserTaskRepository(LearnWithMentorContext context) : base(context)
        {
        }

        public Task<UserTask> GetAsync(int id)
        {
            return Context.UserTasks.FirstOrDefaultAsync(task => task.Id == id);
            
        }

        public Task<int> GetNumberOfTasksByStateAsync(int userId, string state)
        {
            return Context.UserTasks.Where(userTask => userTask.UserId == userId).CountAsync(userTask => userTask.State == state);
            
        }

        public Task<UserTask> GetByPlanTaskForUserAsync(int planTaskId, int userId)
        {
           return  Context.UserTasks.FirstOrDefaultAsync(userTask => userTask.UserId == userId && userTask.PlanTaskId == planTaskId);
            
        }
    }
}
