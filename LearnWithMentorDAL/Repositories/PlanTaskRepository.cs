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
        public int? GetIdByTaskAndPlan(int taskId, int planId)
        {
            return context.PlanTasks.FirstOrDefault(pt => pt.Plan_Id == planId && pt.Task_Id==taskId)?.Id;
        }
        public bool ContainsTaskInPlan(int taskId, int planId)
        {
            return context.PlanTasks.Any(pt => pt.Task_Id == taskId && pt.Plan_Id == planId);
        }

        public int? GetTaskPriorityInPlan(int taskId, int planId)
        {
            return context.PlanTasks.FirstOrDefault(pt => pt.Task_Id == taskId && pt.Plan_Id == pt.Plan_Id)?.Priority;
        }
        public int? GetTaskSectionIdInPlan(int taskId, int planId)
        {
            return context.PlanTasks.FirstOrDefault(pt => pt.Task_Id == taskId && pt.Plan_Id == pt.Plan_Id)?.Section_Id;
        }
        public int[] GetTasksIdForPlan(int planId)
        {
            return context.PlanTasks.Where(pt => pt.Plan_Id == planId).Select(pt => pt.Task_Id).ToArray<int>();
        }

        public int[] GetPlansIdForTask(int taskId)
        {
            return context.PlanTasks.Where(pt => pt.Plan_Id == taskId).Select(pt => pt.Plan_Id).ToArray<int>();
        }
    }
}