using System.Collections.Generic;
using LearnWithMentorDAL.Entities;

namespace LearnWithMentorDAL.Repositories.Interfaces
{
    public interface IPlanRepository: IRepository<Plan>
    {
        Plan Get(int id);
        Plan AddAndReturnElement(Plan plan);
        IEnumerable<Plan> Search(string[] str);
        IEnumerable<Plan> GetPlansForGroup(int groupId);
        IEnumerable<Plan> GetSomePlans(int previousNumberOfPlans, int numberOfPlans);
        bool ContainsId(int id);
        bool AddTaskToPlan(int planId, int taskId, int? sectionId, int? priority);
        IEnumerable<Plan> GetPlansNotUsedInGroup(int planId);
        string GetImageBase64(int planId);
    }
}
