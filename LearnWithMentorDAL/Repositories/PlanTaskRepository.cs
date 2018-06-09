using System.Linq;
using LearnWithMentorDAL.Entities;

namespace LearnWithMentorDAL.Repositories
{
    public class PlanTaskRepository: BaseRepository<PlanTask>, IPlanTaskRepository
    {
        public PlanTaskRepository(LearnWithMentor_DBEntities context) : base(context)
        {
        }
        public bool ContainsTaskInPlan(int taskId, int planId)
        {
            return context.PlanTasks.Any(pt => pt.Task_Id == taskId && pt.Plan_Id == planId);
        }
    }
}