using System.Linq;
using System.Threading.Tasks;
using System.Data.Entity;
using LearnWithMentorDAL.Entities;
using LearnWithMentorDAL.Repositories.Interfaces;
using System.Collections.Generic;
using Task = System.Threading.Tasks.Task;

namespace LearnWithMentorDAL.Repositories
{
    public class CommentRepository : BaseRepository<Comment>, ICommentRepository
    {
        public CommentRepository(LearnWithMentorContext context) : base(context)
        {
        }

        public async Task<Comment> Get(int id)
        {
            return await Context.Comments.FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<bool> ContainsId(int id)
        {
            return await Context.Comments.AnyAsync(t => t.Id == id);
        }

        public async Task<IEnumerable<Comment>> GetByPlanTaskId(int ptId)
        {
            return await Context.Comments.Where(c => c.PlanTask_Id == ptId).ToListAsync();
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