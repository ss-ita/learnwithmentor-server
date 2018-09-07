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
        public MessageRepository(LearnWithMentor_DBEntities context) : base(context)
        {
        }

        public Message Get(int id)
        {
            Task<Message> findMessage = Context.Messages.FirstOrDefaultAsync(m => m.Id == id);
            return findMessage.GetAwaiter().GetResult();
        }

        public IEnumerable<Message> GetByUserTaskId(int userTaskId)
        {
            Task<UserTask> findUserTask = Context.UserTasks.FirstOrDefaultAsync(userTask => userTask.Id == userTaskId);
            return findUserTask.GetAwaiter().GetResult()?.Messages;
        }

        public bool SendForUserTaskId(int userTaskId, Message message)
        {
            Task<UserTask> findUserTask = Context.UserTasks.FirstOrDefaultAsync(task => task.Id == userTaskId);
            if (findUserTask.GetAwaiter().GetResult() != null)
            {
                findUserTask.Result.Messages.Add(message);
                return true;
            }
            return false;
        }
    }
}
