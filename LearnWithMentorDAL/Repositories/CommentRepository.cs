using System.Linq;
using LearnWithMentorDAL.Entities;
using LearnWithMentorDTO;

namespace LearnWithMentorDAL.Repositories
{
    public class CommentRepository: BaseRepository<Comment>, ICommentRepository
    {
        public CommentRepository(LearnWithMentor_DBEntities context) : base(context)
        {
        }

        public Comment Get(int id)
        {
            return context.Comments.FirstOrDefault(t => t.Id == id);
        }

        //task id here is actually PlanTask id
        public void Add(CommentDTO comment, int taskId)
        {
            var newComment = new Comment()
            {
                Id = comment.Id,
                PlanTask_Id = taskId,
                Create_Id = comment.Id,
                Create_Date = null,
                Mod_Date = null
            };
            context.Comments.Add(newComment);
        }

        public void Update(CommentDTO comment)
        {
            var item = context.Comments.Where(c => c.Id == comment.Id);
            if (item == null) return;
            Comment toUpdate = item.First();
            toUpdate.Text = comment.Text;
            Update(toUpdate);
        }

        public void RemoveById(int id)
        {
            var item = context.Comments.Where(c => c.Id == id);
            if (item.Any())
            {
                context.Comments.RemoveRange(item);
            }
        }
    }
}