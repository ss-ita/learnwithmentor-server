using System;
using System.Collections.Generic;
using LearnWithMentorDAL;
using LearnWithMentorDAL.Entities;
using LearnWithMentorDTO;

namespace LearnWithMentorBLL.Interfaces
{
    public interface IMessageService : IDisposableService
    {
        IEnumerable<MessageDTO> GetMessages(int planTaskId);
        bool SendMessage(MessageDTO newMessage);
    }
}
