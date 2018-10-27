using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.Entity;
using LearnWithMentorDAL.Entities;
using LearnWithMentorDAL.Repositories.Interfaces;

namespace LearnWithMentorDAL.Repositories
{
    public class TaskRepository : BaseRepository<StudentTask>, ITaskRepository
    {
        public TaskRepository(LearnWithMentorContext context) : base(context)
        {
        }

        public Task<StudentTask> GetAsync(int id)
        {
            return Context.Tasks.FirstOrDefaultAsync(task => task.Id == id);

        }

        public async Task<bool> IsRemovableAsync(int id)
        {
            return await Context.PlanTasks.AnyAsync(planTask => planTask.Task_Id == id);

        }

        public StudentTask AddAndReturnElement(StudentTask task)
        {
            Context.Tasks.Add(task);
            return task;
        }

        public async Task<IEnumerable<StudentTask>> SearchAsync(string[] str, int planId)
        {
            bool checkPlanExisting = await Context.Plans.AnyAsync(plan => plan.Id == planId);
            if (!checkPlanExisting)
            {
                return null;
            }
            List<StudentTask> result = new List<StudentTask>();
            foreach (var word in str)
            {
                IEnumerable<StudentTask> tasks = Context.PlanTasks.Where(plan => plan.Plan_Id == planId)
                                             .Select(planTask => planTask.Tasks)
                                             .Where(task => task.Name.Contains(word));
                foreach (var task in tasks)
                {
                    if (!result.Contains(task))
                    {
                        result.Add(task);
                    }
                }
            }
            return result;
        }

        public async Task<IEnumerable<StudentTask>> SearchAsync(string[] str)
        {
            List<StudentTask> result = new List<StudentTask>();
            foreach (var word in str)
            {
                IEnumerable<StudentTask> tasks = await Context.Tasks.Where(task => task.Name.Contains(word)).ToListAsync();
                foreach (var task in tasks)
                {
                    if (!result.Contains(task))
                    {
                        result.Add(task);
                    }
                }
            }
            return result;
        }

        public async Task<IEnumerable<StudentTask>> GetTasksNotInPlanAsync(int planId)
        {
            var usedTasks = await Context.PlanTasks.Where(planTask => planTask.Plan_Id == planId).Select(planTask => planTask.Task_Id).ToListAsync();
            return await Context.Tasks.Where(tasks => !usedTasks.Contains(tasks.Id)).ToListAsync();
        }
    }
}
