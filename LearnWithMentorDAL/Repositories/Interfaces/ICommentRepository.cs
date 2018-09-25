using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LearnWithMentorDAL.Entities;

namespace LearnWithMentorDAL.Repositories.Interfaces
{
    public interface ICommentRepository: IRepository<Comment>
    {
        Task<Comment> Get(int id);
        Task<bool> ContainsId(int id);
        void RemoveById(int id);
        Task<IEnumerable<Comment>> GetByPlanTaskId(int ptId);
    }
}