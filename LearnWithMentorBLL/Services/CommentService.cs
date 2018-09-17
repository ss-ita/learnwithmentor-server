using System.Collections.Generic;
using LearnWithMentorDTO;
using LearnWithMentorDAL.Entities;
using LearnWithMentorBLL.Interfaces;
using LearnWithMentorDAL.UnitOfWork;

namespace LearnWithMentorBLL.Services
{
    public class CommentService : BaseService, ICommentService
    {
        public CommentService(IUnitOfWork db) : base(db)
        {
        }

        public CommentDto GetComment(int commentId)
        {
            var comment = db.Comments.Get(commentId);
            if (comment == null)
            {
                return null;
            }
            var commentDTO = new CommentDto(comment.Id,
                                   comment.Text,
                                   comment.Create_Id,
                                   db.Users.ExtractFullName(comment.Create_Id),
                                   comment.Create_Date,
                                   comment.Mod_Date);
            return commentDTO;
        }

        public bool AddCommentToPlanTask(int planTaskId, CommentDto comment)
        {
            var plantask = db.PlanTasks.Get(planTaskId);
            if (plantask == null)
            {
                return false;
            }
            if (db.Users.Get(comment.CreatorId) == null)
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

        public bool AddCommentToPlanTask(int planId, int taskId, CommentDto comment)
        {
            var planTaskId = db.PlanTasks.GetIdByTaskAndPlan(taskId, planId);
            if (planTaskId == null)
            {
                return false;
            }
            return AddCommentToPlanTask(planTaskId.Value, comment);
        }

        public bool UpdateCommentIdText(int commentId, string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return false;
            }
            var comment = db.Comments.Get(commentId);
            if (comment == null)
            {
                return false;
            }
            comment.Text = text;
            db.Comments.Update(comment);
            db.Save();
            return true;
        }

        public bool UpdateComment(int commentId, CommentDto commentDTO)
        {
            if (commentDTO == null)
            {
                return false;
            }
            if (!db.Comments.ContainsId(commentId))
            {
                return false;
            }
            return UpdateCommentIdText(commentId, commentDTO.Text);
        }

        public IEnumerable<CommentDto> GetCommentsForPlanTask(int taskId, int planId)
        {
            var planTaskId = db.PlanTasks.GetIdByTaskAndPlan(taskId, planId);
            return planTaskId == null ? null : GetCommentsForPlanTask(planTaskId.Value);
        }

        public IEnumerable<CommentDto> GetCommentsForPlanTask(int planTaskId)
        {
            var commentsList = new List<CommentDto>();
            var planTask = db.PlanTasks.Get(planTaskId);
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
                                       db.Users.ExtractFullName(c.Create_Id),
                                       c.Create_Date,
                                       c.Mod_Date));
            }
            return commentsList;
        }

        public bool RemoveById(int commentId)
        {
            if (!db.Comments.ContainsId(commentId))
            {
                return false;
            }
            db.Comments.RemoveById(commentId);
            db.Save();
            return true;
        }
    }
}
