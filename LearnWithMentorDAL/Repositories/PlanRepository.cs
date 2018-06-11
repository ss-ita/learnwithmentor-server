using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Formatters;
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

        public bool UpdateById(PlanDTO plan, int id)
        {
            var modified = false;
            var item = context.Plans.Where(c => c.Id == id);
            if (item.Any())
            {
                var toUpdate = item.First();
                if (!string.IsNullOrEmpty(plan.Name))
                {
                    toUpdate.Name = plan.Name;
                    modified = true;
                }

                if (plan.Description != null)
                {
                    toUpdate.Description = plan.Description;
                    modified = true;
                }

                if (plan.Modid != null)
                {
                    toUpdate.Mod_Id = plan.Modid;
                    modified = true;
                }

                toUpdate.Published = plan.Published;
                Update(toUpdate);
            }

            return modified;
        }

        public bool Add(PlanDTO dto )
        {
            if (string.IsNullOrEmpty(dto.Name)|| dto.Description=="" || !context.Users.Any(u => u.Id == dto.Creatorid))
                return false;
            var plan = new Plan
            {
                Name = dto.Name,
                Description = dto.Description,
                Create_Id = dto.Creatorid
            };
            context.Plans.Add(plan);
            return true;
        }
        public bool ContainsId(int id)
        {
            return context.Plans.Any(p => p.Id == id);
        }
    }
}
