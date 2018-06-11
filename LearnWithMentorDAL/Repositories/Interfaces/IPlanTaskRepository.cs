using LearnWithMentorDAL.Entities;

namespace LearnWithMentorDAL.Repositories
{
    public interface IPlanTaskRepository:IRepository<PlanTask>
    {
        bool ContainsTaskInPlan(int taskId, int planId);
        PlanTask Get(int id);
    }
}