using System.Collections.Generic;
using LearnWithMentorDTO;
using LearnWithMentorDAL.Entities;

namespace LearnWithMentorDAL.Repositories
{
    public interface ITaskRepository: IRepository<Entities.Task>
    {
        Entities.Task Get(int id);
        void RemoveById(int id);
        void UpdateById(int id, TaskDTO user);
        void Add(TaskDTO dto);
        IEnumerable<Task> Search(string[] str, int? planId);
    }
}
