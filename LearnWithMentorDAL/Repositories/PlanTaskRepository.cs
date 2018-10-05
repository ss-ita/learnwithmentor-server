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
            PlanTask planTask = await Context.PlanTasks.FirstOrDefaultAsync(pt => pt.Plan_Id == planId && pt.Task_Id==taskId);
            return planTask?.Id;
        }

        public Task<PlanTask> Get(int taskId, int planId)
        {
            return Context.PlanTasks.FirstOrDefaultAsync(pt => pt.Plan_Id == planId && pt.Task_Id == taskId);
        }

        public Task<bool> ContainsTaskInPlan(int taskId, int planId)
        {
            return Context.PlanTasks.AnyAsync(pt => pt.Task_Id == taskId && pt.Plan_Id == planId);
        }

        public async Task<int?> GetTaskPriorityInPlanAsync(int taskId, int planId)
        {
            PlanTask planTask = await Context.PlanTasks.FirstOrDefaultAsync(pt => pt.Task_Id == taskId && planId == pt.Plan_Id);
            return planTask?.Priority;
        }

        public async Task<int?> GetTaskSectionIdInPlanAsync(int taskId, int planId)
        {
            PlanTask planTask = await Context.PlanTasks.FirstOrDefaultAsync(pt => pt.Task_Id == taskId && planId == pt.Plan_Id);
            return planTask?.Section_Id;
        }

        public Task<int[]> GetTasksIdForPlan(int planId)
        {
            return Context.PlanTasks.Where(pt => pt.Plan_Id == planId).Select(pt => pt.Task_Id).ToArrayAsync();
        }

        public Task<int[]> GetPlansIdForTask(int taskId)
        {
            return Context.PlanTasks.Where(pt => pt.Plan_Id == taskId).Select(pt => pt.Plan_Id).ToArrayAsync();
        }
    }
}
