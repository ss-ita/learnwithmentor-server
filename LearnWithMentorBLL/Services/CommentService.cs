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
    public class CommentService:  BaseService, ICommentService
    {
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
    }
}
