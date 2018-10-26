using System.Collections.Generic;
using LearnWithMentorDAL.Entities;
using LearnWithMentorDTO;
using LearnWithMentorBLL.Interfaces;
using LearnWithMentorDAL.UnitOfWork;
using System.Threading.Tasks;

namespace LearnWithMentorBLL.Services
{
    public class MessageService: BaseService, IMessageService
    {
        public MessageService(IUnitOfWork db) : base(db)
        {
        }

        public async Task<IEnumerable<MessageDto>> GetMessagesAsync(int userTaskId)
        {
            UserTask userTask = await db.UserTasks.GetAsync(userTaskId);
            if (userTask == null)
            {
                return null;
            }
            var messages = userTask.Messages;
            var messageDTOs = new List<MessageDto>();
            foreach (var message in messages)
            {
                messageDTOs.Add(new MessageDto(message.Id,
                                       message.UserId,
                                       message.UserTaskId,
                                       await db.Users.ExtractFullNameAsync(message.UserId),
                                       message.Text,
                                       message.SendTime,
                                       message.IsRead
                                       ));
            }
            return messageDTOs;
        }

        public bool SendMessage(MessageDto newMessage)
        {
            var message = new Message()
            {
                UserId = newMessage.SenderId,
                Text = newMessage.Text,
                UserTaskId = newMessage.UserTaskId
            };
            db.Messages.AddAsync(message);
            db.Save();
            return true;
        }

        public async Task<bool> UpdateIsReadStateAsync(int userTaskId, MessageDto message)
        {
            Message GetMessage= await db.Messages.GetAsync(message.Id);
            if (GetMessage == null) return false;
             GetMessage.IsRead = message.IsRead;
            await db.Messages.UpdateAsync(GetMessage);
            db.Save();
            return true;
        }
    }
}
