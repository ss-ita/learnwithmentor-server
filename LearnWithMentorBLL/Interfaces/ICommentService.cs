using System.Collections.Generic;
using System.Threading.Tasks;
using LearnWithMentorDTO;

namespace LearnWithMentorBLL.Interfaces
{
    public interface ICommentService: IDisposableService
    {
        Task<CommentDto> GetCommentAsync(int commentId);
        Task<IEnumerable<CommentDto>> GetCommentsForPlanTask(int taskId, int planId);
        Task<IEnumerable<CommentDto>> GetCommentsForPlanTask(int planTaskId);
        Task<bool> AddCommentToPlanTask(int planTaskId, CommentDto comment);
        Task<bool> AddCommentToPlanTask(int planId, int taskId, CommentDto comment);
        Task<bool> UpdateCommentIdTextAsync(int commentId, string text);
        Task<bool> UpdateCommentAsync(int commentId, CommentDto commentDTO);
        Task<bool> RemoveByIdAsync(int commentId);
    }
}
