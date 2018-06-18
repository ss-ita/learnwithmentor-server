using System.Collections.Generic;
using LearnWithMentorDAL.Entities;

namespace LearnWithMentorDAL.Repositories
{
    public interface IPlanRepository: IRepository<Plan>
    {
        Plan Get(int id);
        IEnumerable<Plan> Search(string[] str);
        bool ContainsId(int id);
    }
}
