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
                throw new ValidationException("Comment in this plan for this user does not exist", "");
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

        public bool SendMessages(MessageDTO newMessage)
        {

            Message message = new Message()
            {
                Id = newMessage.Id,
                UserTask_Id = newMessage.UserTaskId,
                User_Id = newMessage.SenderId,
                Text = newMessage.Text,
                Send_Time = DateTime.Now
            };
            db.Messages.Add(message);
            db.Save();
            return true;
        }
    }
    
}
