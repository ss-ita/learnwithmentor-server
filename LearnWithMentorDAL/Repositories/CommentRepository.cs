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

        public bool ContainsId(int id)
        {
            return context.Comments.FirstOrDefault(t => t.Id == id)!=null;
        }

        public IQueryable<Comment> GetByPlanTaskId(int ptId)
        {
            return context.Comments.Where(c =>c.PlanTask_Id==ptId );
        }

        public void RemoveById(int id)
        {
            var item = context.Comments.Where(c => c.Id == id);
            if (item.Any())
            {
                context.Comments.RemoveRange(item);
            }
        }
        public void RemoveByPlanTaskId(int planTaskid)
        {
            var item = context.Comments.FirstOrDefault(c => c.PlanTask_Id == planTaskid);
            Remove(item);
        }

    }
}