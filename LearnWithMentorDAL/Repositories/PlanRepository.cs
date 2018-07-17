using System.Collections.Generic;
using System.Linq;
using LearnWithMentorDAL.Entities;
using LearnWithMentorDAL.Repositories.Interfaces;

namespace LearnWithMentorDAL.Repositories
{
    public class PlanRepository : BaseRepository<Plan>, IPlanRepository
    {
        public PlanRepository(LearnWithMentor_DBEntities context) : base(context)
        {
            
        }

        public Plan Get(int id)
        {
            return Context.Plans.FirstOrDefault(p => p.Id == id);
        }
        public Plan AddAndReturnElement(Plan plan)
        {
            Context.Plans.Add(plan);
            return plan;
        }
        public IEnumerable<Plan> GetPlansForGroup(int groupId)
        {
            return Context.Groups.FirstOrDefault(g => g.Id == groupId)?.Plans;
        }

        public IEnumerable<Plan> Search(string[] searchString)
        {
            var result = new List<Plan>();
            foreach (var word in searchString)
            {
                IQueryable<Plan> found = Context.Plans.Where(p => p.Name.Contains(word));
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
            return Context.Plans.Any(p => p.Id == id);
        }

        public string GetImageBase64(int planId)
        {
            return Context.Plans.FirstOrDefault(p => p.Id == planId)?.Image;
        }

        public bool AddTaskToPlan(int planId, int taskId, int? sectionId, int? priority)
        {
            var taskAdd = Context.Tasks.FirstOrDefault(task => task.Id == taskId);
            var planAdd = Context.Plans.FirstOrDefault(plan => plan.Id == planId);
            var section = sectionId != null ? Context.Sections.FirstOrDefault(s => s.Id == sectionId) : Context.Sections.First();

            if (taskAdd == null || planAdd == null)
            {
                return false;
            }
            PlanTask toInsert = new PlanTask()
            {
                Plan_Id = planId,
                Task_Id = taskId,
                Priority = priority,
                Section_Id = section?.Id
            };

            Context.PlanTasks.Add(toInsert);
            return true;
        }
        public IEnumerable<Plan> GetSomePlans(int previousNumberOfPlans, int numberOfPlans)
        {
            return Context.Plans.OrderBy(p => p.Id).Skip(previousNumberOfPlans).Take(numberOfPlans);
        }

        public IEnumerable<Plan> GetPlansNotUsedInGroup(int groupId)
        {
            var usedPlans = Context.Groups.FirstOrDefault(g => g.Id == groupId)?.Plans.Select(p => p.Id);
            return Context.Plans.Where(p => !usedPlans.Contains(p.Id));
        }
    }
}
