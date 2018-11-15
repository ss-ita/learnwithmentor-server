using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Threading.Tasks;
using LearnWithMentorDAL.Entities;
using LearnWithMentorDAL.Repositories.Interfaces;

namespace LearnWithMentorDAL.Repositories
{
    public class PlanRepository : BaseRepository<Plan>, IPlanRepository
    {
        public PlanRepository(LearnWithMentorContext context) : base(context)
        {

        }

        public Task<Plan> Get(int id)
        {
            return Context.Plans.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<bool> IsRemovableAsync(int id)
        {
            return await Context.Plans.AnyAsync(plan => plan.Id == id);
        }

        public Plan AddAndReturnElement(Plan plan)
        {
            Context.Plans.Add(plan);
            return plan;
        }

        public async Task<IEnumerable<Plan>> GetPlansForGroupAsync(int groupId)
        {
            Group group = await Context.Groups.FirstOrDefaultAsync(g => g.Id == groupId);
            return group?.Plans;
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

        public Task<bool> ContainsId(int id)
        {
            return Context.Plans.AnyAsync(p => p.Id == id);
        }

        public async Task<string> GetImageBase64Async(int planId)
        {
            Plan plan = await Context.Plans.FirstOrDefaultAsync(p => p.Id == planId);
            return plan?.Image;
        }

        public async Task<bool> AddTaskToPlanAsync(int planId, int taskId, int? sectionId, int? priority)
        {
            var taskAdd = await Context.Tasks.FirstOrDefaultAsync(task => task.Id == taskId);
            var planAdd = await Context.Plans.FirstOrDefaultAsync(plan => plan.Id == planId);
            var section = sectionId != null ? await Context.Sections.FirstOrDefaultAsync(s => s.Id == sectionId) : Context.Sections.First();

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

        public async Task<IEnumerable<Plan>> GetPlansNotUsedInGroupAsync(int groupId)
        {
            Group group = await Context.Groups.FirstOrDefaultAsync(g => g.Id == groupId);
            IEnumerable<int> usedPlansId = group?.Plans.Select(p => p.Id);
            return Context.Plans.Where(p => !usedPlansId.Contains(p.Id));
        }
    }
}
