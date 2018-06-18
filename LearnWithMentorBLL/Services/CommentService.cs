using System;
using System.Linq;
using System.Data.Entity;
using System.Collections.Generic;
using LearnWithMentorDTO;
using LearnWithMentorDAL.Entities;
using LearnWithMentorBLL.Interfaces;
using LearnWithMentorBLL.Infrastructure;

namespace LearnWithMentorBLL.Services
{
    public class CommentService : BaseService, ICommentService
    {
        public CommentDTO GetComment(int id)
        {
            var comm = db.Comments.Get(id);
            if (comm == null)
                throw new ValidationException("No comment with this id", "id");
            var dto = new CommentDTO(comm.Id,
                                   comm.Text,
                                   comm.Create_Id,
                                   db.Users.ExtractFullName(comm.Create_Id),
                                   comm.Create_Date,
                                   comm.Mod_Date);
            return dto;
        }

        public bool AddCommentToPlanTask(int planTaskId, CommentDTO c)
        {
            var newComment = new Comment()
            {
                Id = c.Id,
                Text = c.Text,
                PlanTask_Id = planTaskId,
                Create_Id = c.CreatorId,
                Create_Date = null,
                Mod_Date = null
            };
            db.Comments.Add(newComment);
            db.Save();
            return true;
        }
        public bool AddCommentToPlanTask(int planId, int taskId, CommentDTO c)
        {
            var iden = db.PlanTasks.GetIdByTaskAndPlan(taskId, planId);
            if (iden == null)
                throw new ValidationException("No task with this id in this plan","");
            var newComment = new Comment()
            {
                Id = c.Id,
                Text = c.Text,
                PlanTask_Id = iden.Value,
                Create_Id = c.CreatorId,
                Create_Date = null,
                Mod_Date = null
            };
            db.Comments.Add(newComment);
            db.Save();
            return true;
        }

        public bool UpdateCommentIdText(int Id, string text)
        {
            if (text == null || text.Equals(string.Empty))
                throw new ValidationException("Can not set empty text as comment","text");
            var comm = db.Comments.Get(Id);
            comm.Text = text;
            db.Comments.Update(comm);
            db.Save();
            return true;
        }

        public bool UpdateComment(int Id, CommentDTO c)
        {
            if (c == null || c.Text.Equals(string.Empty))
                throw new ValidationException("Can not set empty text as comment", "text");
            var comm = db.Comments.Get(Id);
            comm.Text = c.Text;
            db.Comments.Update(comm);
            db.Save();
            return true;
        }

        public IEnumerable<CommentDTO> GetTaskCommentsForPlan(int taskId, int planId)
        {
            List<CommentDTO> dto = new List<CommentDTO>();
            var ptId = db.PlanTasks.GetIdByTaskAndPlan(taskId, planId);
            if (ptId == null)
                throw new ValidationException("Task in this plan does not exists", "");
            var comments = db.Comments.GetByPlanTaskId(ptId.Value);
            if (comments == null || comments.Count() < 1)
                throw new ValidationException("Task in this plan has no comments", "");
            foreach (var c in comments)
            {
                dto.Add(new CommentDTO(c.Id,
                                       c.Text,
                                       c.Create_Id,
                                       db.Users.ExtractFullName(c.Create_Id),
                                       c.Create_Date,
                                       c.Mod_Date));
            }
            return dto;
        }

        public bool RemoveById(int id)
        {
            if (!db.Comments.ContainsId(id))
                return false;
            RemoveById(id);
            db.Save();
            return true;
        }
    }
}

