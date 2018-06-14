using System.Collections.Generic;
using LearnWithMentorDTO;
using LearnWithMentorDAL.Entities;

namespace LearnWithMentorDAL.Repositories
{
    public interface ITaskRepository: IRepository<Task>
    {
        Task Get(int id);
        bool RemoveById(int id);
        bool IsRemovable(int id);
        bool UpdateById(int id, TaskDTO task);
        bool UpdateForPlan(int taskId, int planId, TaskDTO task);
        bool Add(TaskDTO dto);
        IEnumerable<Task> Search(string[] str, int planId);
        IEnumerable<Task> Search(string[] str);
    }
}
