using System.Linq;
using LearnWithMentorDAL.Entities;

namespace LearnWithMentorDAL.Repositories.Interfaces
{
    public interface ICommentRepository: IRepository<Comment>
    {
        Comment Get(int id);
        bool ContainsId(int id);
        void RemoveById(int id);
        IQueryable<Comment> GetByPlanTaskId(int ptId);
    }
}