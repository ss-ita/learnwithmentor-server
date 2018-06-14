using System.Collections.Generic;
using LearnWithMentorDAL.Entities;
using LearnWithMentorDTO;

namespace LearnWithMentorDAL.Repositories
{
    public interface IPlanRepository: IRepository<Plan>
    {
        Plan Get(int id);
        IEnumerable<Task> GetAllTasks(int planId);
        bool RemoveById(int id);
        bool UpdateById(PlanDTO plan, int id);
        bool Add(PlanDTO dto);
        IEnumerable<Plan> Search(string[] str);
        bool ContainsId(int id);
    }
}
