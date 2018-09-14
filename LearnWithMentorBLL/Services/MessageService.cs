using System.Collections.Generic;
using LearnWithMentorDAL.Entities;
using LearnWithMentorDto;
using LearnWithMentorBLL.Interfaces;
using LearnWithMentorDAL.UnitOfWork;

namespace LearnWithMentorBLL.Services
{
    public class MessageService: BaseService, IMessageService
    {
        public MessageService(IUnitOfWork db) : base(db)
        {
        }

        public IEnumerable<MessageDto> GetMessages(int userTaskId)
        {
            var userTask = db.UserTasks.Get(userTaskId);
            if (userTask == null)
            {
                return null;
            }
            var messages = userTask.Messages;
            var messageDTOs = new List<MessageDto>();
            foreach (var message in messages)
            {
                messageDTOs.Add(new MessageDto(message.Id,
                                       message.User_Id,
                                       message.UserTask_Id,
                                       db.Users.ExtractFullName(message.User_Id),
                                       message.Text,
                                       message.Send_Time));
            }
            return messageDTOs;
        }

        public bool SendMessage(MessageDto newMessage)
        {
            var message = new Message()
            {
                User_Id = newMessage.SenderId,
                Text = newMessage.Text,
                UserTask_Id = newMessage.UserTaskId
            };
            db.Messages.Add(message);
            db.Save();
            return true;
        }
    }
}
