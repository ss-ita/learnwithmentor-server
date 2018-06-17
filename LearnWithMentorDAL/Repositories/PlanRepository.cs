using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
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

        public IEnumerable<Task> GetAllTasks(int planId)
        {
            var tasklIdList = context.PlanTasks.Where(pt => pt.Plan_Id == planId).Select(pt => pt.Task_Id).ToArray<int>();
            var rr = context.Tasks.Where(t => tasklIdList.Contains(t.Id));

            return null;
        }

        public bool RemoveById(int id)
        {
            var item = Get(id);
            if (item != null)
            {
                Remove(item);
                return true;
            }

            return false;
        }

        public bool UpdateById(PlanDTO plan, int id)
        {
            var modified = false;
            var toUpdate = Get(id);
            if (toUpdate != null)
            {
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
            if (string.IsNullOrEmpty(dto.Name)|| dto.Description=="" || !ContainsId(dto.Creatorid))
                return false;
            var plan = new Plan
            {
                Name = dto.Name,
                Description = dto.Description,
                Create_Id = dto.Creatorid
            };
            Add(plan);
            return true;
        }
        public IEnumerable<Plan> Search(string[] str)
        {
            var result = new List<Plan>();
            foreach (var s in str)
            {
                var found =  context.Plans.Where(p => p.Name.Contains(s) ) ;
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
