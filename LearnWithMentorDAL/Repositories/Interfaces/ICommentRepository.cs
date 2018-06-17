using LearnWithMentorDAL.Entities;
using LearnWithMentorDTO;
using System.Linq;

namespace LearnWithMentorDAL.Repositories
{
    public interface ICommentRepository: IRepository<Comment>
    {
        Comment Get(int id);
        void RemoveById(int id);
        void Update(CommentDTO comment);
        void Add(CommentDTO dto, int taskId);
        IQueryable<Comment> GetByPlanTaskId(int ptId);
    }
}