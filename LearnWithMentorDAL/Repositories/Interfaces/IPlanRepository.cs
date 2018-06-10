using LearnWithMentorDAL.Entities;
using LearnWithMentorDTO;

namespace LearnWithMentorDAL.Repositories
{
    public interface IPlanRepository: IRepository<Plan>
    {
        Plan Get(int id);
        void RemoveById(int id);
        void UpdateById(PlanDTO plan, int id);
        void Add(PlanDTO dto);
        bool ContainsId(int id);
    }
}
