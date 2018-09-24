using System.Collections.Generic;
using System.Threading.Tasks;
using LearnWithMentorDTO;

namespace LearnWithMentorBLL.Interfaces
{
    public interface ICommentService: IDisposableService
    {
        CommentDto GetComment(int commentId);
        Task<IEnumerable<CommentDto>> GetCommentsForPlanTask(int taskId, int planId);
        Task<IEnumerable<CommentDto>> GetCommentsForPlanTask(int planTaskId);
        Task<bool> AddCommentToPlanTask(int planTaskId, CommentDto comment);
        Task<bool> AddCommentToPlanTask(int planId, int taskId, CommentDto comment);
        bool UpdateCommentIdText(int commentId, string text);
        bool UpdateComment(int commentId, CommentDto commentDTO);
        bool RemoveById(int commentId);
    }
}
