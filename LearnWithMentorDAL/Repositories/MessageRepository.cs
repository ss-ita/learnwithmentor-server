using System.Collections.Generic;
using System.Linq;
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
            return Context.Messages.FirstOrDefault(m => m.Id == id);
        }

        public IEnumerable<Message> GetByUserTaskId(int utId)
        {
            return Context.UserTasks.FirstOrDefault(ut => ut.Id == utId)?.Messages;
        }
        public bool SendForUserTaskId(int utId,Message m)
        {
            var ut = Context.UserTasks.FirstOrDefault(t => t.Id == utId);
            if(ut!=null)
                return true;
            return false;
        }
    }
}
