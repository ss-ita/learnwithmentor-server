using System.Collections.Generic;
using System.Linq;
using LearnWithMentorDAL.Entities;

namespace LearnWithMentorDAL.Repositories
{
    public class PlanRepository: BaseRepository<Plan>, IPlanRepository
    {
        public PlanRepository(LearnWithMentor_DBEntities context) : base(context)
        {
        }

        public Plan Get(int id)
        {
            return context.Plans.FirstOrDefault(p => p.Id == id);
        }
        public IEnumerable<Plan> GetPlansForGroup(int groupId)
        {
            return context.Groups.FirstOrDefault(g => g.Id == groupId)?.Plans;
        }

        public IEnumerable<Plan> Search(string[] str)
        {
            var result = new List<Plan>();
            foreach (var s in str)
            {
                IQueryable<Plan> found = context.Plans.Where(p => p.Name.Contains(s));
                foreach (var f in found)
                {
                    if (!result.Contains(f))
                    {
                        result.Add(f);
                    }
                }
            }
            return result;
        }
        
        public bool ContainsId(int id)
        {
            return context.Plans.Any(p => p.Id == id);
        }
    }
}
