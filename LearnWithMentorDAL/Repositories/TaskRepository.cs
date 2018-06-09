using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using LearnWithMentorDAL.Entities;
using LearnWithMentorDTO;

namespace LearnWithMentorDAL.Repositories
{
    public class TaskRepository: BaseRepository<Entities.Task>, ITaskRepository
    {
        public TaskRepository(LearnWithMentor_DBEntities context) : base(context)
        {
        }
        public Entities.Task Get(int id)
        {
            return context.Tasks.FirstOrDefault(t => t.Id == id);
        }
        public void RemoveById(int id)
        {
            var item = context.Tasks.FirstOrDefault(t => t.Id == id);
            if (item!=null)
            {
                context.Tasks.Remove(item);
            }
        }
        public void UpdateById(int id, TaskDTO task)
        {
            var item = context.Tasks.FirstOrDefault(t => t.Id == id);
            if (item!=null)
            {
                Task toUpdate = item;
                toUpdate.Name = task.Name;
                toUpdate.Description = task.Description;
                toUpdate.Private = task.Private;
                Update(toUpdate);
            }
        }
        public void Add(TaskDTO taskDTO)
        {
            Task toAdd = new Task()
            {
                Id = taskDTO.Id,
                Name = taskDTO.Name,
                Description = taskDTO.Description,
                Private = taskDTO.Private,
            };
            context.Tasks.Add(toAdd);
        }
        public IEnumerable<Task> Search(string[] str, int? planId)
        {
            List<Task> ret = new List<Task>();
            foreach (var s in str)
            {
                IQueryable<Task> found;
                if (planId == null)
                    found = context.Tasks.Where(t => t.Name.Contains(s));
                else if (!context.Plans.Any(p=>p.Id==planId))
                    return null;
                else
                    found = context.PlanTasks.Where(p => p.Plan_Id == planId).Select(t => t.Tasks);
                foreach (var f in found)
                {
                    if (!ret.Contains(f))
                    {
                        ret.Add(f);
                    }
                }
            }
            return ret;
        }
    }
}
