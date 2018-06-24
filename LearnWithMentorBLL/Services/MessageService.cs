using System;
using System.Collections.Generic;
using LearnWithMentorDAL.Entities;
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
        public IEnumerable<MessageDTO> GetMessages(int userTaskId)
        {
            var userTask = db.UserTasks.Get(userTaskId);
            if (userTask == null)
                throw new InternalServiceException("Comment in this plan for this user does not exist", "");
            var messages = userTask.Messages;
            List<MessageDTO> messageDTOs = new List<MessageDTO>();
            foreach (var m in messages)
            {
                messageDTOs.Add(new MessageDTO(m.Id,
                                       m.User_Id,
                                       m.UserTask_Id,
                                       db.Users.ExtractFullName(m.User_Id),
                                       m.Text,
                                       m.Send_Time));
            }
            return messageDTOs;
        }

        public bool SendMessage(MessageDTO messageDTO)
        {
            Message message = new Message()
            {
                User_Id = messageDTO.SenderId,
                Text = messageDTO.Text,
                UserTask_Id = messageDTO.UserTaskId
            };
            db.Messages.Add(message);
            db.Save();
            return true;
        }

    }

}
