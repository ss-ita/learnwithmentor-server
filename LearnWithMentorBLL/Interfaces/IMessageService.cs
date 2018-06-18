using System;
using System.Collections.Generic;
using LearnWithMentorDAL;
using LearnWithMentorDAL.Entities;
using LearnWithMentorDTO;

namespace LearnWithMentorBLL.Interfaces
{
    public interface IMessageService
    {
        IEnumerable<MessageDTO> GetMessages(int userId, int taskId, int planId);
    }
}
