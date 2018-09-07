using System.Linq;
using System.Threading.Tasks;
using System.Data.Entity;
using LearnWithMentorDAL.Entities;
using LearnWithMentorDAL.Repositories.Interfaces;
using System.Collections.Generic;

namespace LearnWithMentorDAL.Repositories
{
    public class CommentRepository: BaseRepository<Comment>, ICommentRepository
    {
        public CommentRepository(LearnWithMentor_DBEntities context) : base(context)
        {
        }

        public Comment Get(int id)
        {
            Task<Comment> findCommnet = Context.Comments.FirstOrDefaultAsync(t => t.Id == id);
            return findCommnet.GetAwaiter().GetResult();
        }

        public bool ContainsId(int id)
        {
            Task<bool> checkIdExisting = Context.Comments.AnyAsync(t => t.Id == id);
            return checkIdExisting.GetAwaiter().GetResult();
        }

        public IQueryable<Comment> GetByPlanTaskId(int ptId)
        {
            return Context.Comments.Where(c =>c.PlanTask_Id==ptId );
        }

        public void RemoveById(int id)
        {
            IEnumerable<Comment> comments = Context.Comments.Where(c => c.Id == id);
            if (comments.Any())
            {
                Context.Comments.RemoveRange(comments);
            }
        }

        public void RemoveByPlanTaskId(int planTaskid)
        {
            Task<Comment> findComment = Context.Comments.FirstOrDefaultAsync(c => c.PlanTask_Id == planTaskid);
            Remove(findComment.GetAwaiter().GetResult());
        }

    }
}