using System.Collections.Generic;
using LearnWithMentorDAL.Entities;
using System.Threading.Tasks;

namespace LearnWithMentorDAL.Repositories.Interfaces
{
    public interface IPlanRepository : IRepository<Plan>
    {
        IEnumerable<Plan> GetSomePlans(int previousNumberOfPlans, int numberOfPlans);
        IEnumerable<Plan> Search(string[] searchString);
        Plan AddAndReturnElement(Plan plan);
        Task<Plan> Get(int id);
        Task<IEnumerable<Plan>> GetPlansForGroup(int groupId);
        Task<bool> ContainsId(int id);
        Task<bool> AddTaskToPlan(int planId, int taskId, int? sectionId, int? priority);
        Task<IEnumerable<Plan>> GetPlansNotUsedInGroup(int groupId);
        Task<string> GetImageBase64(int planId);
    }
}
