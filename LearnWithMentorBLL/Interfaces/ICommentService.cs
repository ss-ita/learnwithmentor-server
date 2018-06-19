using System;
using System.Collections.Generic;
using LearnWithMentorDAL;
using LearnWithMentorDAL.Entities;
using LearnWithMentorDTO;

namespace LearnWithMentorBLL.Interfaces
{
    public interface ICommentService
    {
        CommentDTO GetComment(int id);
        IEnumerable<CommentDTO> GetCommentsForPlanTask(int taskId, int planId);
        IEnumerable<CommentDTO> GetCommentsForPlanTask(int planTaskId);
        bool AddCommentToPlanTask(int planTaskId, CommentDTO c);
        bool AddCommentToPlanTask(int planId, int taskId, CommentDTO c);
        bool UpdateCommentIdText(int Id, string text);
        bool UpdateComment(int Id, CommentDTO c);
        bool RemoveById(int id);
    }
}
