using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Threading.Tasks;
using LearnWithMentorDAL.Entities;
using LearnWithMentorDAL.Repositories.Interfaces;

namespace LearnWithMentorDAL.Repositories
{
    public class MessageRepository : BaseRepository<Message>, IMessageRepository
    {
        public MessageRepository(LearnWithMentorContext context) : base(context)
        {
        }

        public async Task<Message> Get(int id)
        {
            Message findMessage = await Context.Messages.FirstOrDefaultAsync(m => m.Id == id);
            return findMessage;
        }

        public async Task<IEnumerable<Message>> GetByUserTaskId(int userTaskId)
        {
            UserTask findUserTask = await Context.UserTasks.FirstOrDefaultAsync(userTask => userTask.Id == userTaskId);
            return findUserTask?.Messages;
        }

        public async Task<bool> SendForUserTaskId(int userTaskId, Message message)
        {
            UserTask findUserTask = await Context.UserTasks.FirstOrDefaultAsync(task => task.Id == userTaskId);
            if (findUserTask != null)
            {
                findUserTask.Messages.Add(message);
                return true;
            }
            return false;
        }
    }
}
