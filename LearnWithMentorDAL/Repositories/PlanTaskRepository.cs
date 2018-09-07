using System.Linq;
using System.Data.Entity;
using System.Threading.Tasks;
using LearnWithMentorDAL.Entities;
using LearnWithMentorDAL.Repositories.Interfaces;

namespace LearnWithMentorDAL.Repositories
{
    public class PlanTaskRepository: BaseRepository<PlanTask>, IPlanTaskRepository
    {
        public PlanTaskRepository(LearnWithMentor_DBEntities context) : base(context)
        {
        }

        public PlanTask Get(int id)
        {
            Task<PlanTask> findPlanTask = Context.PlanTasks.FirstOrDefaultAsync(p => p.Id == id);
            return findPlanTask.GetAwaiter().GetResult();
        }

        public int? GetIdByTaskAndPlan(int taskId, int planId)
        {
            Task<PlanTask> findPlanTask = Context.PlanTasks.FirstOrDefaultAsync(pt => pt.Plan_Id == planId && pt.Task_Id==taskId);
            return findPlanTask.GetAwaiter().GetResult()?.Id;
        }

        public PlanTask Get(int taskId, int planId)
        {
            Task<PlanTask> findPlanTask = Context.PlanTasks.FirstOrDefaultAsync(pt => pt.Plan_Id == planId && pt.Task_Id == taskId);
            return findPlanTask.GetAwaiter().GetResult();
        }

        public bool ContainsTaskInPlan(int taskId, int planId)
        {
            Task<bool> checkPlanTaskEsixting = Context.PlanTasks.AnyAsync(pt => pt.Task_Id == taskId && pt.Plan_Id == planId);
            return checkPlanTaskEsixting.GetAwaiter().GetResult();
        }

        public int? GetTaskPriorityInPlan(int taskId, int planId)
        {
            Task<PlanTask> findPlanTask = Context.PlanTasks.FirstOrDefaultAsync(pt => pt.Task_Id == taskId && planId == pt.Plan_Id);
            return findPlanTask.GetAwaiter().GetResult()?.Priority;
        }

        public int? GetTaskSectionIdInPlan(int taskId, int planId)
        {
            Task<PlanTask> findPlanTask = Context.PlanTasks.FirstOrDefaultAsync(pt => pt.Task_Id == taskId && planId == pt.Plan_Id);
            return findPlanTask.GetAwaiter().GetResult()?.Section_Id;
        }

        public int[] GetTasksIdForPlan(int planId)
        {
            Task<int[]> getTasksId = Context.PlanTasks.Where(pt => pt.Plan_Id == planId).Select(pt => pt.Task_Id).ToArrayAsync();
            return getTasksId.GetAwaiter().GetResult();
        }

        public int[] GetPlansIdForTask(int taskId)
        {
            Task<int[]> getPlansId = Context.PlanTasks.Where(pt => pt.Plan_Id == taskId).Select(pt => pt.Plan_Id).ToArrayAsync();
            return getPlansId.GetAwaiter().GetResult();
        }
    }
}
