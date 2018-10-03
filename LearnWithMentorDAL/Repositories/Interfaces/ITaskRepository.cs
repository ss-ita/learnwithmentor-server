using System.Collections.Generic;
using System.Threading.Tasks;
using TaskEntity = LearnWithMentorDAL.Entities.Task;

namespace LearnWithMentorDAL.Repositories.Interfaces
{
    public interface ITaskRepository: IRepository<TaskEntity>
    {
        Task<TaskEntity> GetAsync(int id);
        Task<bool> IsRemovableAsync(int id);
        TaskEntity AddAndReturnElement(TaskEntity task);
        Task<IEnumerable<TaskEntity>> SearchAsync(string[] str, int planId);
        Task<IEnumerable<TaskEntity>> SearchAsync(string[] str);
        Task<IEnumerable<TaskEntity>> GetTasksNotInPlanAsync(int planId);
    }
}
