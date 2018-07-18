using System.Collections.Generic;
using LearnWithMentorDAL.Entities;
using LearnWithMentorDTO;
using LearnWithMentorBLL.Interfaces;
using LearnWithMentorDAL.UnitOfWork;

namespace LearnWithMentorBLL.Services
{
    public class MessageService: BaseService, IMessageService
    {
        public MessageService(IUnitOfWork db) : base(db)
        {
        }

        public IEnumerable<MessageDTO> GetMessages(int userTaskId)
        {
            var userTask = db.UserTasks.Get(userTaskId);
            if (userTask == null)
            {
                return null;
            }
            var messages = userTask.Messages;
            var messageDTOs = new List<MessageDTO>();
            foreach (var message in messages)
            {
                messageDTOs.Add(new MessageDTO(message.Id,
                                       message.User_Id,
                                       message.UserTask_Id,
                                       db.Users.ExtractFullName(message.User_Id),
                                       message.Text,
                                       message.Send_Time));
            }
            return messageDTOs;
        }

        public bool SendMessage(MessageDTO messageDTO)
        {
            var message = new Message()
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
