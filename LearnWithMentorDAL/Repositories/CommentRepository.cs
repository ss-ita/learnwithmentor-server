using System.Linq;
using LearnWithMentorDAL.Entities;
using LearnWithMentorDAL.Repositories.Interfaces;

namespace LearnWithMentorDAL.Repositories
{
    public class CommentRepository: BaseRepository<Comment>, ICommentRepository
    {
        public CommentRepository(LearnWithMentor_DBEntities context) : base(context)
        {
        }

        public Comment Get(int id)
        {
            return Context.Comments.FirstOrDefault(t => t.Id == id);
        }

        public bool ContainsId(int id)
        {
            return Context.Comments.FirstOrDefault(t => t.Id == id)!=null;
        }

        public IQueryable<Comment> GetByPlanTaskId(int ptId)
        {
            return Context.Comments.Where(c =>c.PlanTask_Id==ptId );
        }

        public void RemoveById(int id)
        {
            var item = Context.Comments.Where(c => c.Id == id);
            if (item.Any())
            {
                Context.Comments.RemoveRange(item);
            }
        }
        public void RemoveByPlanTaskId(int planTaskid)
        {
            var item = Context.Comments.FirstOrDefault(c => c.PlanTask_Id == planTaskid);
            Remove(item);
        }

    }
}