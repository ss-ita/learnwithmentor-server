using System.Collections.Generic;
using System.Threading.Tasks;
using LearnWithMentorDTO;

namespace LearnWithMentorBLL.Interfaces
{
    public interface IMessageService : IDisposableService
    {
        Task<IEnumerable<MessageDto>> GetMessagesAsync(int userTaskId);
        bool SendMessage(MessageDto newMessage);
        Task<bool> UpdateIsReadStateAsync(int userTaskId, MessageDto message);
    }
}
