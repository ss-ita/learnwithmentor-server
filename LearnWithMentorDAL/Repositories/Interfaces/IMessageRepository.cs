using System.Collections.Generic;
using LearnWithMentorDAL.Entities;

namespace LearnWithMentorDAL.Repositories.Interfaces
{
    public interface IMessageRepository : IRepository<Message>
    {
        Message Get(int id);
        IEnumerable<Message> GetByUserTaskId(int utId);
        bool SendForUserTaskId(int utId, Message m);
    }
}
