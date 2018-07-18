using System.Collections.Generic;
using LearnWithMentorDTO;

namespace LearnWithMentorBLL.Interfaces
{
    public interface ICommentService: IDisposableService
    {
        CommentDTO GetComment(int id);
        IEnumerable<CommentDTO> GetCommentsForPlanTask(int taskId, int planId);
        IEnumerable<CommentDTO> GetCommentsForPlanTask(int planTaskId);
        bool AddCommentToPlanTask(int planTaskId, CommentDTO c);
        bool AddCommentToPlanTask(int planId, int taskId, CommentDTO c);
        bool UpdateCommentIdText(int id, string text);
        bool UpdateComment(int id, CommentDTO c);
        bool RemoveById(int id);
    }
}
