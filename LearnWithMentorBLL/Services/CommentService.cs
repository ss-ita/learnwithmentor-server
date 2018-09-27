using System.Collections.Generic;
using LearnWithMentorDTO;
using LearnWithMentorDAL.Entities;
using LearnWithMentorBLL.Interfaces;
using LearnWithMentorDAL.UnitOfWork;
using System.Threading.Tasks;

namespace LearnWithMentorBLL.Services
{
    public class CommentService : BaseService, ICommentService
    {
        public CommentService(IUnitOfWork db) : base(db)
        {
        }

        public async Task<CommentDto> GetCommentAsync(int commentId)
        {
            Comment comment = await db.Comments.GetAsync(commentId);
            if (comment == null)
            {
                return null;
            }
            var commentDTO = new  CommentDto(comment.Id,
                                   comment.Text,
                                   comment.Create_Id,
                                   await db.Users.ExtractFullName(comment.Create_Id),
                                   comment.Create_Date,
                                   comment.Mod_Date);
            return  commentDTO;
        }

        public async Task<bool> AddCommentToPlanTask(int planTaskId, CommentDto comment)
        {
            var plantask = await db.PlanTasks.Get(planTaskId);
            if (plantask == null)
            {
                return false;
            }
            if ( await db.Users.Get(comment.CreatorId) == null)
            {
                return false;
            }
            var newComment = new Comment()
            {
                Text = comment.Text,
                PlanTask_Id = planTaskId,
                Create_Id = comment.CreatorId,
            };
            db.Comments.Add(newComment);
            db.Save();
            return true;
        }

        public async Task<bool> AddCommentToPlanTask(int planId, int taskId, CommentDto comment)
        {
            var planTaskId = await db.PlanTasks.GetIdByTaskAndPlan(taskId, planId);
            if (planTaskId == null)
            {
                return false;
            }
            return await AddCommentToPlanTask(planTaskId.Value, comment);
        }

        public async Task<bool> UpdateCommentIdTextAsync(int commentId, string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return false;
            }
            Comment comment = await db.Comments.GetAsync(commentId);
            if (comment == null)
            {
                return false;
            }
            comment.Text = text;
            db.Comments.Update(comment);
            db.Save();
            return true;
        }

        public async Task<bool> UpdateCommentAsync(int commentId, CommentDto commentDTO)
        {
            if (commentDTO == null)
            {
                return false;
            }
            if (!await db.Comments.ContainsIdAsync(commentId))
            {
                return false;
            }
            return await UpdateCommentIdTextAsync(commentId, commentDTO.Text);
        }

        public async Task<IEnumerable<CommentDto>> GetCommentsForPlanTask(int taskId, int planId)
        {
            var planTaskId = await db.PlanTasks.GetIdByTaskAndPlan(taskId, planId);
            return planTaskId == null ? null : await GetCommentsForPlanTask(planTaskId.Value);
        }

        public async Task<IEnumerable<CommentDto>> GetCommentsForPlanTask(int planTaskId)
        {
            var commentsList = new List<CommentDto>();
            var planTask = await db.PlanTasks.Get(planTaskId);
            var comments = planTask?.Comments;
            if (comments == null)
            {
                return null;
            }
            foreach (var c in comments)
            {
                commentsList.Add(new CommentDto(c.Id,
                                       c.Text,
                                       c.Create_Id,
                                       await db.Users.ExtractFullName(c.Create_Id),
                                       c.Create_Date,
                                       c.Mod_Date));
            }
            return commentsList;
        }

        public async Task<bool> RemoveByIdAsync(int commentId)
        {
            if (!await db.Comments.ContainsIdAsync(commentId))
            {
                return false;
            }
            db.Comments.RemoveById(commentId);
            db.Save();
            return true;
        }
    }
}
