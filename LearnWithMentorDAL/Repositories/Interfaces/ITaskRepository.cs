using System.Collections.Generic;
using System.Threading.Tasks;
using TaskEntity = LearnWithMentorDAL.Entities.Task;

namespace LearnWithMentorDAL.Repositories.Interfaces
{
    public interface ITaskRepository: IRepository<TaskEntity>
    {
        Task<TaskEntity> Get(int id);
        Task<bool> IsRemovable(int id);
        TaskEntity AddAndReturnElement(TaskEntity task);
        Task<IEnumerable<TaskEntity>> Search(string[] str, int planId);
        Task<IEnumerable<TaskEntity>> Search(string[] str);
        Task<IEnumerable<TaskEntity>> GetTasksNotInPlan(int planId);
    }
}
