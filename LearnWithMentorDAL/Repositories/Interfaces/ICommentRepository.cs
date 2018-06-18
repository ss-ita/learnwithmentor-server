using LearnWithMentorDAL.Entities;
using LearnWithMentorDTO;
using System.Linq;

namespace LearnWithMentorDAL.Repositories
{
    public interface ICommentRepository: IRepository<Comment>
    {
        Comment Get(int id);
        bool ContainsId(int id);
        void RemoveById(int id);
        IQueryable<Comment> GetByPlanTaskId(int ptId);
    }
}