using System.Collections.Generic;
using System.Threading.Tasks;
using LearnWithMentorDTO;

namespace LearnWithMentorBLL.Interfaces
{
    public interface ICommentService: IDisposableService
    {
        Task<CommentDto> GetCommentAsync(int commentId);
        Task<IEnumerable<CommentDto>> GetCommentsForPlanTaskAsync(int taskId, int planId);
        Task<IEnumerable<CommentDto>> GetCommentsForPlanTaskAsync(int planTaskId);
        Task<bool> AddCommentToPlanTaskAsync(int planTaskId, CommentDto comment);
        Task<bool> AddCommentToPlanTaskAsync(int planId, int taskId, CommentDto comment);
        Task<bool> UpdateCommentIdTextAsync(int commentId, string text);
        Task<bool> UpdateCommentAsync(int commentId, CommentDto commentDTO);
        Task<bool> RemoveByIdAsync(int commentId);
    }
}
