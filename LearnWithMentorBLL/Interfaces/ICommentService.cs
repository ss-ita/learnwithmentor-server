using System;
using System.Collections.Generic;
using LearnWithMentorDAL;
using LearnWithMentorDAL.Entities;
using LearnWithMentorDTO;

namespace LearnWithMentorBLL.Interfaces
{
    public interface ICommentService
    {
        IEnumerable<CommentDTO> GetTaskCommentsForPlan(int taskId, int planId);
    }
}
