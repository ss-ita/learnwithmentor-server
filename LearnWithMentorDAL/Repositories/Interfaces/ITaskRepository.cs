using System.Collections.Generic;
using LearnWithMentorDAL.Entities;

namespace LearnWithMentorDAL.Repositories.Interfaces
{
    public interface ITaskRepository: IRepository<Task>
    {
        Task Get(int id);
        bool IsRemovable(int id);
        Task AddAndReturnElement(Task task);
        IEnumerable<Task> Search(string[] str, int planId);
        IEnumerable<Task> Search(string[] str);
        IEnumerable<Task> GetTasksNotInPlan(int planId);
    }
}
