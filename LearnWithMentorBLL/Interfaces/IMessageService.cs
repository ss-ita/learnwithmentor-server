using System.Collections.Generic;
using LearnWithMentorDTO;

namespace LearnWithMentorBLL.Interfaces
{
    public interface IMessageService : IDisposableService
    {
        IEnumerable<MessageDto> GetMessages(int userTaskId);
        bool SendMessage(MessageDto newMessage);
    }
}
