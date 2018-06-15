using System.Collections.Generic;
using System.Linq;
using LearnWithMentorDAL.Entities;

namespace LearnWithMentorDAL.Repositories
{
    public class TaskRepository: BaseRepository<Task>, ITaskRepository
    {
        public TaskRepository(LearnWithMentor_DBEntities context) : base(context)
        {
        }
        public Task Get(int id)
        {
            return context.Tasks.FirstOrDefault(t => t.Id == id);
        }
        public bool RemoveById(int id)
        {
            var item = Get(id);
            if (item!=null && IsRemovable(id))
            {
                Remove(item);
                return true;
            }
            return false;
        }
        public bool IsRemovable(int id)
        {
            return (!context.PlanTasks.Any(pt=>pt.Task_Id==id));
        }
        public bool UpdateById(int id, Task task)
        {
            //var item = Get(id);
            //if (item!=null)
            //{
            //    Task toUpdate = item;
            //    toUpdate.Name = task.Name;
            //    toUpdate.Description = task.Description;
            //    toUpdate.Private = task.Private;
            //    toUpdate.Mod_Id = task.ModifierId;
            //    Update(toUpdate);
            //    return true;
            //}
            //return false;

            var item = Get(id);
            if (item != null)
            {
                Update(item);
                return true;
            }
            return false;
        }
        
        public IEnumerable<Task> Search(string[] str, int planId)
        {
            List<Task> result = new List<Task>();
            foreach (var s in str)
            {
                IQueryable<Task> found;
                if (!context.Plans.Any(p => p.Id == planId))
                    return null;
                found = context.PlanTasks.Where(p => p.Plan_Id == planId)
                                             .Select(t => t.Tasks)
                                             .Where(t => t.Name.Contains(s));
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
        public IEnumerable<Task> Search(string[] str)
        {
            List<Task> result = new List<Task>();
            foreach (var s in str)
            {
                IQueryable<Task> found;
                found = context.Tasks.Where(t => t.Name.Contains(s));
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
    }
}
