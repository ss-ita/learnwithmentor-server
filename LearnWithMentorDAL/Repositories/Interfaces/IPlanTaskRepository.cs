using LearnWithMentorDAL.Entities;
using System.Threading.Tasks;

namespace LearnWithMentorDAL.Repositories.Interfaces
{
    public interface IPlanTaskRepository:IRepository<PlanTask>
    {
        Task<PlanTask> Get(int id);
        Task<PlanTask> Get(int taskId, int planId);
        Task<bool> ContainsTaskInPlan(int taskId, int planId);
        Task<int[]> GetTasksIdForPlan(int planId);
        Task<int[]> GetPlansIdForTask(int taskId);
        Task<int?> GetTaskPriorityInPlanAsync(int taskId, int planId);
        Task<int?> GetTaskSectionIdInPlanAsync(int taskId, int planId);
        Task<int?> GetIdByTaskAndPlanAsync(int taskId, int planId);
    }
}