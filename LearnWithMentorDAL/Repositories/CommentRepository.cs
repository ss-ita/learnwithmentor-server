using System.Linq;
using LearnWithMentorDAL.Entities;

namespace LearnWithMentorDAL.Repositories
{
    public class CommentRepository: BaseRepository<Comment>, ICommentRepository
    {
        public CommentRepository(LearnWithMentor_DBEntities _context) : base(_context)
        {
        }
        public Comment Get(int id)
        {
            return context.Comments.Where(t => t.Id == id).FirstOrDefault();
        }
    }
}