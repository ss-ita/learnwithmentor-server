using LearnWithMentorDAL.Entities;
namespace LearnWithMentorDAL.Repositories
{
    public interface ICommentRepository: IRepository<Comment>
    {
        Comment Get(int id);
    }
}