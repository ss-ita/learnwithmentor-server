using System.Collections.Generic;
using System.Threading.Tasks;
using LearnWithMentorDAL.Entities;

namespace LearnWithMentorDAL.Repositories.Interfaces
{
    public interface IMessageRepository : IRepository<Message>
    {
        Task<Message>Get(int id);
        Task<IEnumerable<Message>> GetByUserTaskId(int userTaskId);
        Task<bool> SendForUserTaskId(int userTaskId, Message message);
    }
}
