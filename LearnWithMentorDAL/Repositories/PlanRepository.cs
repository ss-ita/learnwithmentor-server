using System.Collections.Generic;
using System.Linq;
using LearnWithMentorDTO;
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

        public void RemoveById(int id)
        {
            IEnumerable<Plan> item = context.Plans.Where(c => c.Id == id);
            if (item.Any())
            {
                context.Plans.RemoveRange(item);
            }
        }

        public void UpdateById(PlanDTO plan, int id)
        {
            var item = context.Plans.Where(c => c.Id == id);
            if (!item.Any()) return;
            var toUpdate = item.First();
            toUpdate.Name = plan.Name;
            toUpdate.Description = plan.Description;
            toUpdate.Mod_Id = plan.Modid;
            toUpdate.Published = plan.Published;
            Update(toUpdate);
        }

        public void Add(PlanDTO dto )
        {
            var plan = new Plan
            {
                Name = dto.Name,
                Description = dto.Description,
                Create_Id = dto.Creatorid
            };
            context.Plans.Add(plan);
        }
        public bool ContainsId(int id)
        {
            return context.Plans.Any(p => p.Id == id);
        }
    }
}
