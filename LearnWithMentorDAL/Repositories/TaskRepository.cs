using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.Entity;
using LearnWithMentorDAL.Entities;
using LearnWithMentorDAL.Repositories.Interfaces;
using TaskEntity = LearnWithMentorDAL.Entities.Task;

namespace LearnWithMentorDAL.Repositories
{
    public class TaskRepository: BaseRepository<TaskEntity>, ITaskRepository
    {
        public TaskRepository(LearnWithMentor_DBEntities context) : base(context)
        {
        }

        public TaskEntity Get(int id)
        {
            Task<TaskEntity> findTask = Context.Tasks.FirstOrDefaultAsync(task => task.Id == id);
            return findTask.GetAwaiter().GetResult();
        }

        public bool IsRemovable(int id)
        {
            Task<bool> checkTaskExisting = Context.PlanTasks.AnyAsync(planTask => planTask.Task_Id == id);
            return !checkTaskExisting.GetAwaiter().GetResult();
        }

        public TaskEntity AddAndReturnElement(TaskEntity task)
        {
            Context.Tasks.Add(task);
            return task;
        }

        public IEnumerable<TaskEntity> Search(string[] str, int planId)
        {
            Task<bool> checkPlanExisting = Context.Plans.AnyAsync(plan => plan.Id == planId);
            if (!checkPlanExisting.GetAwaiter().GetResult())
            {
                return null;
            }
            List<TaskEntity> result = new List<TaskEntity>();
            foreach (var word in str)
            {
                IEnumerable<TaskEntity> tasks = Context.PlanTasks.Where(plan => plan.Plan_Id == planId)
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

        public IEnumerable<TaskEntity> Search(string[] str)
        {
            List<TaskEntity> result = new List<TaskEntity>();
            foreach (var word in str)
            {
                IEnumerable<TaskEntity> tasks = Context.Tasks.Where(task => task.Name.Contains(word));
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

        public IEnumerable<TaskEntity> GetTasksNotInPlan(int planId)
        {
            var usedTasks = Context.PlanTasks.Where(planTask => planTask.Plan_Id == planId).Select(planTask => planTask.Task_Id);
            return Context.Tasks.Where(tasks => !usedTasks.Contains(tasks.Id));
        }
    }
}
