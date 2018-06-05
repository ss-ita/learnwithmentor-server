using System.Linq;
using LearnWithMentorDAL.Entities;
using LearnWithMentorDTO;

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

        public void Add(CommentDTO comment, int taskId)
        {
            Comment newComment = new Comment()
            {
                Id = comment.Id,
                Task_Id = taskId,
                Create_Id = comment.Id,
                Create_Date = null,
                Mod_Date = null
            };
            context.Comments.Add(newComment);
        }

        public void UpdateById(CommentDTO comment, int id)
        {
            var item = context.Comments.Where(c => c.Id == id);
            if (item == null) return;
            Comment toUpdate = item.First();
            toUpdate.Text = comment.Text;
            Update(toUpdate);
        }

        public void RemoveById(int id)
        {
            var item = context.Comments.Where(c => c.Id == id);
            if (item != null)
            {
                context.Comments.RemoveRange(item);
            }
        }
    }
}