using System.Collections.Generic;
using LearnWithMentorDAL.Entities;

namespace LearnWithMentorDAL.Repositories
{
    public interface IPlanRepository: IRepository<Plan>
    {
        Plan Get(int id);
        IEnumerable<Plan> Search(string[] str);
        IEnumerable<Plan> GetPlansForGroup(int groupId);
        bool ContainsId(int id);
        IEnumerable<Plan> GetPlansNotUsedInGroup(int planId);
    }
}
