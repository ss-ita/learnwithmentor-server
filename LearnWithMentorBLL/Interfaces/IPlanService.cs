using System.Collections.Generic;
using LearnWithMentorDTO;

namespace LearnWithMentorBLL.Interfaces
{
    public interface IPlanService : IDisposableService
    {
        PlanDTO Get(int id);
        List<PlanDTO> GetAll();
        List<PlanDTO> GetSomeAmount(int prevAmount, int amount);
        List<TaskDTO> GetAllTasks(int planId);
        bool UpdateById(PlanDTO plan, int id);
        bool Add(PlanDTO dto);
        int? AddAndGetId(PlanDTO dto);
        List<PlanDTO> Search(string[] str);
        bool ContainsId(int id);
        bool SetImage(int planId, byte[] image, string imageName);
        ImageDTO GetImage(int id);
    }
}
