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

        public Task<Comment> Get(int id)
        {
            return Context.Comments.FirstOrDefaultAsync(t => t.Id == id);
        }

        public Task<bool> ContainsId(int id)
        {
            return Context.Comments.AnyAsync(t => t.Id == id);
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

        public async Task RemoveByPlanTaskId(int planTaskid)
        {
            Comment findComment = await Context.Comments.FirstOrDefaultAsync(c => c.PlanTask_Id == planTaskid);
            Remove(findComment);
        }

    }
}