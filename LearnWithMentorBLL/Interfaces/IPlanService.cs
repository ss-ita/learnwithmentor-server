using System.Collections.Generic;
using System.Threading.Tasks;
using LearnWithMentorDTO;

namespace LearnWithMentorBLL.Interfaces
{
    public interface IPlanService : IDisposableService
    {
        Task<int?> AddAndGetIdAsync(PlanDto dto);
        List<PlanDto> Search(string[] searchString);
        List<PlanDto> GetAll();
        List<PlanDto> GetSomeAmount(int prevAmount, int amount);
        Task<PlanDto> Get(int id);
        Task<List<TaskDto>> GetAllTasksAsync(int planId);
        Task<string> GetInfo(int groupid, int planid);
        Task<List<int>> GetAllPlanTaskids(int planId);
        Task<List<SectionDto>> GetTasksForPlanAsync(int planId);
        Task<bool> UpdateByIdAsync(PlanDto plan, int id);
        Task<bool> Add(PlanDto dto);
        Task<bool> ContainsId(int id);
        Task<bool> SetImage(int id, byte[] image, string imageName);
        Task<bool> AddTaskToPlanAsync(int planId, int taskId, int? sectionId, int? priority);
        Task<ImageDto> GetImage(int id);
    }
}
