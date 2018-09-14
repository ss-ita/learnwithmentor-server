using System.Collections.Generic;
using LearnWithMentorDto;

namespace LearnWithMentorBLL.Interfaces
{
    public interface ICommentService: IDisposableService
    {
        CommentDto GetComment(int commentId);
        IEnumerable<CommentDto> GetCommentsForPlanTask(int taskId, int planId);
        IEnumerable<CommentDto> GetCommentsForPlanTask(int planTaskId);
        bool AddCommentToPlanTask(int planTaskId, CommentDto comment);
        bool AddCommentToPlanTask(int planId, int taskId, CommentDto comment);
        bool UpdateCommentIdText(int commentId, string text);
        bool UpdateComment(int commentId, CommentDto commentDTO);
        bool RemoveById(int commentId);
    }
}
