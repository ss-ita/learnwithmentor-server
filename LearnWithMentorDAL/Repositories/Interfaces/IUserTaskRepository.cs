using LearnWithMentorDAL.Entities;
using System.Threading.Tasks;

namespace LearnWithMentorDAL.Repositories.Interfaces
{
    public interface IUserTaskRepository: IRepository<UserTask>
    {
        Task<UserTask> GetAsync(int id);
        Task<UserTask> GetByPlanTaskForUserAsync(int planTaskId, int userId);
        Task<int> GetNumberOfTasksByStateAsync(int userId, string state);
    }
}
