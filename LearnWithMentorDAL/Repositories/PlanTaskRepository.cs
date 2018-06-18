using System.Linq;
using LearnWithMentorDAL.Entities;

namespace LearnWithMentorDAL.Repositories
{
    public class PlanTaskRepository: BaseRepository<PlanTask>, IPlanTaskRepository
    {
        public PlanTaskRepository(LearnWithMentor_DBEntities context) : base(context)
        {
        }
        public PlanTask Get(int id)
        {
            return context.PlanTasks.FirstOrDefault(p => p.Id == id);
        }
        public bool ContainsTaskInPlan(int taskId, int planId)
        {
            return context.PlanTasks.Any(pt => pt.Task_Id == taskId && pt.Plan_Id == planId);
        }
        public System.Collections.Generic.IEnumerable<Task> TasksInPlan(int planId)
        {
            int[] consistence = context.PlanTasks.Where(p => p.Plan_Id == planId).Select(o => o.Task_Id).ToArray<int>();
            return context.Tasks.Where(t => consistence.Contains(t.Id));
        }
    }
}