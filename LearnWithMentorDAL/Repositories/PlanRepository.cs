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

        public IEnumerable<Plan> Search(string[] searchString)
        {
            var result = new List<Plan>();
            foreach (var word in searchString)
            {
                IQueryable<Plan> found = context.Plans.Where(p => p.Name.Contains(word));
                foreach (var match in found)
                {
                    if (!result.Contains(match))
                    {
                        result.Add(match);
                    }
                }
            }
            return result;
        }
        
        public bool ContainsId(int id)
        {
            return context.Plans.Any(p => p.Id == id);
        }

        public IEnumerable<Plan> GetPlansNotUsedInGroup(int groupId)
        {
            return context.Plans.Where(p => !p.Groups.Select(g => g.Id).Contains(groupId));
        }
    }
}
