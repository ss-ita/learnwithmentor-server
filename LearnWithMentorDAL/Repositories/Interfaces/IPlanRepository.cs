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
        Task<bool> ContainsId(int id);
        Task<bool> AddTaskToPlanAsync(int planId, int taskId, int? sectionId, int? priority);
        Task<IEnumerable<Plan>> GetPlansNotUsedInGroupAsync(int groupId);
        Task<string> GetImageBase64Async(int planId);
        Task<IEnumerable<Plan>> GetPlansForGroupAsync(int groupId);
    }
}
