using System.Collections.Generic;
using LearnWithMentorDTO;
using LearnWithMentorDAL.Entities;

namespace LearnWithMentorDAL.Repositories
{
    public interface ITaskRepository: IRepository<Entities.Task>
    {
        Entities.Task Get(int id);
        bool RemoveById(int id);
        bool UpdateById(int id, TaskDTO user);
        bool Add(TaskDTO dto);
        IEnumerable<Task> Search(string[] str, int planId);
        IEnumerable<Task> Search(string[] str);
    }
}
