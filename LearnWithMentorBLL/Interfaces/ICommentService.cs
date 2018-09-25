using System.Collections.Generic;
using System.Threading.Tasks;
using LearnWithMentorDTO;

namespace LearnWithMentorBLL.Interfaces
{
    public interface ICommentService: IDisposableService
    {
        Task<CommentDto> GetComment(int commentId);
        Task<IEnumerable<CommentDto>> GetCommentsForPlanTask(int taskId, int planId);
        Task<IEnumerable<CommentDto>> GetCommentsForPlanTask(int planTaskId);
        Task<bool> AddCommentToPlanTask(int planTaskId, CommentDto comment);
        Task<bool> AddCommentToPlanTask(int planId, int taskId, CommentDto comment);
        Task<bool> UpdateCommentIdText(int commentId, string text);
        Task<bool> UpdateComment(int commentId, CommentDto commentDTO);
        Task<bool> RemoveById(int commentId);
    }
}
