using LearnWithMentorDAL.Entities;
using LearnWithMentorDTO;

namespace LearnWithMentorDAL.Repositories
{
    public interface ICommentRepository: IRepository<Comment>
    {
        Comment Get(int id);
        void RemoveById(int id);
        void UpdateById(CommentDTO comment, int id);
        void Add(CommentDTO dto, int taskId);
    }
}