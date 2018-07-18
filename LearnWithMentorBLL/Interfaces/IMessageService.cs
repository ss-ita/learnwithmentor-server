using System.Collections.Generic;
using LearnWithMentorDTO;

namespace LearnWithMentorBLL.Interfaces
{
    public interface IMessageService : IDisposableService
    {
        IEnumerable<MessageDTO> GetMessages(int planTaskId);
        bool SendMessage(MessageDTO newMessage);
    }
}
