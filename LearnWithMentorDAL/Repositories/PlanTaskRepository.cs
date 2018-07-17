using System.Linq;
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
            return Context.PlanTasks.FirstOrDefault(p => p.Id == id);
        }
        public int? GetIdByTaskAndPlan(int taskId, int planId)
        {
            return Context.PlanTasks.FirstOrDefault(pt => pt.Plan_Id == planId && pt.Task_Id==taskId)?.Id;
        }

        public PlanTask Get(int taskId, int planId)
        {
            return Context.PlanTasks.FirstOrDefault(pt => pt.Plan_Id == planId && pt.Task_Id == taskId);
        }

        public bool ContainsTaskInPlan(int taskId, int planId)
        {
            return Context.PlanTasks.Any(pt => pt.Task_Id == taskId && pt.Plan_Id == planId);
        }

        public int? GetTaskPriorityInPlan(int taskId, int planId)
        {
            return Context.PlanTasks.FirstOrDefault(pt => pt.Task_Id == taskId && planId == pt.Plan_Id)?.Priority;
        }
        public int? GetTaskSectionIdInPlan(int taskId, int planId)
        {
            return Context.PlanTasks.FirstOrDefault(pt => pt.Task_Id == taskId && planId == pt.Plan_Id)?.Section_Id;
        }
        public int[] GetTasksIdForPlan(int planId)
        {
            return Context.PlanTasks.Where(pt => pt.Plan_Id == planId).Select(pt => pt.Task_Id).ToArray();
        }

        public int[] GetPlansIdForTask(int taskId)
        {
            return Context.PlanTasks.Where(pt => pt.Plan_Id == taskId).Select(pt => pt.Plan_Id).ToArray();
        }
    }
}