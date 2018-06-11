using LearnWithMentorDAL.Entities;
using LearnWithMentorDTO;

namespace LearnWithMentorDAL.Repositories
{
    public interface IPlanRepository: IRepository<Plan>
    {
        Plan Get(int id);
        void RemoveById(int id);
        bool UpdateById(PlanDTO plan, int id);
        bool Add(PlanDTO dto);
        bool ContainsId(int id);
    }
}
