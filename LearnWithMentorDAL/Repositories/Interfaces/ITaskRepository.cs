using System.Collections.Generic;
using LearnWithMentorDAL.Entities;

namespace LearnWithMentorDAL.Repositories
{
    public interface ITaskRepository: IRepository<Task>
    {
        Task Get(int id);
        bool RemoveById(int id);
        bool IsRemovable(int id);
        bool UpdateById(int id, Task task);
        IEnumerable<Task> Search(string[] str, int planId);
        IEnumerable<Task> Search(string[] str);
    }
}
