using System.Collections.Generic;
using LearnWithMentorDTO;
using LearnWithMentorBLL.Interfaces;
using LearnWithMentorBLL.Infrastructure;

namespace LearnWithMentorBLL.Services
{
    public class MessageService: BaseService, IMessageService
    {
        public MessageService() : base()
        {
        }
        public IEnumerable<MessageDTO> GetMessages(int userId, int userTaskId)
        {
            int? utId = db.UserTasks.Get(userTaskId)?.Id;
            if (utId == null)
                throw new ValidationException("No task in this plan for this user", "");
            var messages = db.Messages.GetByUserTaskId(utId.Value);

            List<MessageDTO> dto = new List<MessageDTO>();
            foreach (var m in messages)
            {
                dto.Add(new MessageDTO(m.Id,
                                       m.User_Id,
                                       userTaskId,
                                       db.Users.ExtractFullName(m.User_Id),
                                       m.Text,
                                       m.Send_Time));
            }
            return dto;
        }

    }
    
}
