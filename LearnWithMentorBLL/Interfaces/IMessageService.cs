using System.Collections.Generic;
using LearnWithMentorDto;

namespace LearnWithMentorBLL.Interfaces
{
    public interface IMessageService : IDisposableService
    {
        IEnumerable<MessageDto> GetMessages(int userTaskId);
        bool SendMessage(MessageDto newMessage);
    }
}
