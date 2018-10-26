using System.Linq;
using System.Data.Entity;
using System.Threading.Tasks;
using LearnWithMentorDAL.Entities;
using LearnWithMentorDAL.Repositories.Interfaces;

namespace LearnWithMentorDAL.Repositories
{
    public class PlanTaskRepository: BaseRepository<PlanTask>, IPlanTaskRepository
    {
        public PlanTaskRepository(LearnWithMentorContext context) : base(context)
        {
        }

        public Task<PlanTask> Get(int id)
        {
            return Context.PlanTasks.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<int?> GetIdByTaskAndPlanAsync(int taskId, int planId)
        {
            PlanTask planTask = await Context.PlanTasks.FirstOrDefaultAsync(pt => pt.PlanId == planId && pt.TaskId==taskId);
            return planTask?.Id;
        }

        public Task<PlanTask> Get(int taskId, int planId)
        {
            return Context.PlanTasks.FirstOrDefaultAsync(pt => pt.PlanId == planId && pt.TaskId == taskId);
        }

        public Task<bool> ContainsTaskInPlan(int taskId, int planId)
        {
            return Context.PlanTasks.AnyAsync(pt => pt.TaskId == taskId && pt.PlanId == planId);
        }

        public async Task<int?> GetTaskPriorityInPlanAsync(int taskId, int planId)
        {
            PlanTask planTask = await Context.PlanTasks.FirstOrDefaultAsync(pt => pt.TaskId == taskId && planId == pt.PlanId);
            return planTask?.Priority;
        }

        public async Task<int?> GetTaskSectionIdInPlanAsync(int taskId, int planId)
        {
            PlanTask planTask = await Context.PlanTasks.FirstOrDefaultAsync(pt => pt.TaskId == taskId && planId == pt.PlanId);
            return planTask?.SectionId;
        }

        public Task<int[]> GetTasksIdForPlan(int planId)
        {
            return Context.PlanTasks.Where(pt => pt.PlanId == planId).Select(pt => pt.TaskId).ToArrayAsync();
        }

        public Task<int[]> GetPlansIdForTask(int taskId)
        {
            return Context.PlanTasks.Where(pt => pt.PlanId == taskId).Select(pt => pt.PlanId).ToArrayAsync();
        }
    }
}
