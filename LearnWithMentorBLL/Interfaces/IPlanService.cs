using System.Collections.Generic;
using System.Linq;
using LearnWithMentorBLL.Interfaces;
using LearnWithMentorDTO;
using LearnWithMentorDAL.Entities;

namespace LearnWithMentorBLL.Interfaces
{
    public interface IPlanService
    {
        PlanDTO Get(int id);
        List<PlanDTO> GetAll();
        List<TaskDTO> GetAllTasks(int planId);
        bool UpdateById(PlanDTO plan, int id);
        bool Add(PlanDTO dto);
        List<PlanDTO> Search(string[] str);
        bool ContainsId(int id);
    }
}
