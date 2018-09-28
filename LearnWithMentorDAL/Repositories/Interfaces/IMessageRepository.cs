using System.Collections.Generic;
using System.Threading.Tasks;
using LearnWithMentorDAL.Entities;

namespace LearnWithMentorDAL.Repositories.Interfaces
{
    public interface IMessageRepository : IRepository<Message>
    {
        Task<Message>GetAsync(int id);
        Task<IEnumerable<Message>> GetByUserTaskIdAsync(int userTaskId);
        Task<bool> SendForUserTaskIdAsync(int userTaskId, Message message);
    }
}
