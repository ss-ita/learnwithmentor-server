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
    }
}