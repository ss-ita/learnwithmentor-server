using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LearnWithMentorDAL.Entities;
using Task = System.Threading.Tasks.Task;

namespace LearnWithMentorDAL.Repositories.Interfaces
{
    public interface ICommentRepository: IRepository<Comment>
    {
        Task<Comment> GetAsync(int id);
        Task<bool> ContainsIdAsync(int id);
        void RemoveById(int id);
        Task<IEnumerable<Comment>> GetByPlanTaskIdAsync(int ptId);
    }
}