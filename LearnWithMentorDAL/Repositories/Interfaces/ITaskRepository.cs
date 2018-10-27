using LearnWithMentorDAL.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LearnWithMentorDAL.Repositories.Interfaces
{
    public interface ITaskRepository: IRepository<StudentTask>
    {
        Task<StudentTask> GetAsync(int id);
        Task<bool> IsRemovableAsync(int id);
        StudentTask AddAndReturnElement(StudentTask task);
        Task<IEnumerable<StudentTask>> SearchAsync(string[] str, int planId);
        Task<IEnumerable<StudentTask>> SearchAsync(string[] str);
        Task<IEnumerable<StudentTask>> GetTasksNotInPlanAsync(int planId);
    }
}
