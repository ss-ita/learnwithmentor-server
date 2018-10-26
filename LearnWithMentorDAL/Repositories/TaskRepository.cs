using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.Entity;
using LearnWithMentorDAL.Entities;
using LearnWithMentorDAL.Repositories.Interfaces;
using TaskEntity = LearnWithMentorDAL.Entities.StudentTask;

namespace LearnWithMentorDAL.Repositories
{
    public class TaskRepository : BaseRepository<TaskEntity>, ITaskRepository
    {
        public TaskRepository(LearnWithMentorContext context) : base(context)
        {
        }

        public Task<TaskEntity> GetAsync(int id)
        {
            return Context.Tasks.FirstOrDefaultAsync(task => task.Id == id);

        }

        public async Task<bool> IsRemovableAsync(int id)
        {
            return await Context.PlanTasks.AnyAsync(planTask => planTask.TaskId == id);

        }

        public TaskEntity AddAndReturnElement(TaskEntity task)
        {
            Context.Tasks.Add(task);
            return task;
        }

        public async Task<IEnumerable<TaskEntity>> SearchAsync(string[] str, int planId)
        {
            bool checkPlanExisting = await Context.Plans.AnyAsync(plan => plan.Id == planId);
            if (!checkPlanExisting)
            {
                return null;
            }
            List<TaskEntity> result = new List<TaskEntity>();
            foreach (var word in str)
            {
                IEnumerable<TaskEntity> tasks = Context.PlanTasks.Where(plan => plan.PlanId == planId)
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

        public async Task<IEnumerable<TaskEntity>> SearchAsync(string[] str)
        {
            List<TaskEntity> result = new List<TaskEntity>();
            foreach (var word in str)
            {
                IEnumerable<TaskEntity> tasks = await Context.Tasks.Where(task => task.Name.Contains(word)).ToListAsync();
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

        public async Task<IEnumerable<TaskEntity>> GetTasksNotInPlanAsync(int planId)
        {
            var usedTasks = await Context.PlanTasks.Where(planTask => planTask.PlanId == planId).Select(planTask => planTask.TaskId).ToListAsync();
            return await Context.Tasks.Where(tasks => !usedTasks.Contains(tasks.Id)).ToListAsync();
        }
    }
}
