using System.Collections.Generic;
using System.Threading.Tasks;
using LearnWithMentorDTO;

namespace LearnWithMentorBLL.Interfaces
{
    public interface IMessageService : IDisposableService
    {
        Task<IEnumerable<MessageDto>> GetMessages(int userTaskId);
        bool SendMessage(MessageDto newMessage);
    }
}
