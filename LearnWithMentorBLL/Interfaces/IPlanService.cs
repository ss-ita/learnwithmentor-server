using System.Collections.Generic;
using LearnWithMentorDTO;

namespace LearnWithMentorBLL.Interfaces
{
    public interface IPlanService : IDisposableService
    {
        PlanDto Get(int id);
        List<PlanDto> GetAll();
        List<PlanDto> GetSomeAmount(int prevAmount, int amount);
        List<TaskDto> GetAllTasks(int planId);
        string GetInfo(int groupid, int planid);
        List<int> GetAllPlanTaskids(int planId);
        List<SectionDto> GetTasksForPlan(int planId);
        bool UpdateById(PlanDto plan, int id);
        bool Add(PlanDto dto);
        int? AddAndGetId(PlanDto dto);
        List<PlanDto> Search(string[] searchString);
        bool ContainsId(int id);
        bool SetImage(int id, byte[] image, string imageName);
        bool AddTaskToPlan(int planId, int taskId, int? sectionId, int? priority);
        ImageDto GetImage(int id);
    }
}
