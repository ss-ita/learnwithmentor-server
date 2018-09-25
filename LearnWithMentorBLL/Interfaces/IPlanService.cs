using System.Collections.Generic;
using System.Threading.Tasks;
using LearnWithMentorDTO;
using TaskThread = System.Threading.Tasks;

namespace LearnWithMentorBLL.Interfaces
{
    public interface IPlanService : IDisposableService
    {
        int? AddAndGetId(PlanDto dto);
        List<PlanDto> Search(string[] searchString);
        List<PlanDto> GetAll();
        List<PlanDto> GetSomeAmount(int prevAmount, int amount);
        TaskThread.Task<PlanDto> Get(int id);
        TaskThread.Task<List<TaskDto>> GetAllTasks(int planId);
        TaskThread.Task<string> GetInfo(int groupid, int planid);
        TaskThread.Task<List<int>> GetAllPlanTaskids(int planId);
        TaskThread.Task<List<SectionDto>> GetTasksForPlan(int planId);
        TaskThread.Task<bool> UpdateById(PlanDto plan, int id);
        TaskThread.Task<bool> Add(PlanDto dto);
        TaskThread.Task<bool> ContainsId(int id);
        TaskThread.Task<bool> SetImage(int id, byte[] image, string imageName);
        TaskThread.Task<bool> AddTaskToPlan(int planId, int taskId, int? sectionId, int? priority);
        TaskThread.Task<ImageDto> GetImage(int id);
    }
}
