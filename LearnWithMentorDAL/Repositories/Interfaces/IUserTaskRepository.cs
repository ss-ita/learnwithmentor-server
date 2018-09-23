using LearnWithMentorDAL.Entities;
using System.Threading.Tasks;

namespace LearnWithMentorDAL.Repositories.Interfaces
{
    public interface IUserTaskRepository: IRepository<UserTask>
    {
        Task<UserTask> Get(int id);
        Task<UserTask> GetByPlanTaskForUser(int planTaskId, int userId);
        Task<int> GetNumberOfTasksByState(int userId, string state);
    }
}
