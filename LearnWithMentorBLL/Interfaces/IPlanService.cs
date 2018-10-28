using System.Collections.Generic;
using System.Threading.Tasks;
using LearnWithMentorDTO;

namespace LearnWithMentorBLL.Interfaces
{
    public interface IPlanService : IDisposableService
    {
        Task<int?> AddAndGetIdAsync(PlanDto dto);
        List<PlanDto> Search(string[] searchString);
        Task<List<PlanDto>> GetAll();
        List<PlanDto> GetSomeAmount(int prevAmount, int amount);
        Task<PlanDto> GetAsync(int id);
        Task<List<TaskDto>> GetAllTasksAsync(int planId);
        Task<string> GetInfoAsync(int groupid, int planid);
        Task<List<int>> GetAllPlanTaskidsAsync(int planId);
        Task<List<SectionDto>> GetTasksForPlanAsync(int planId);
        Task<bool> UpdateByIdAsync(PlanDto plan, int id);
        Task<bool> RemovePlanByIdAsync(int planId);
        Task<bool> AddAsync(PlanDto dto);
        Task<bool> ContainsId(int id);
        Task<bool> SetImageAsync(int id, byte[] image, string imageName);
        Task<bool> AddTaskToPlanAsync(int planId, int taskId, int? sectionId, int? priority);
        Task<ImageDto> GetImageAsync(int id);
    }
}
