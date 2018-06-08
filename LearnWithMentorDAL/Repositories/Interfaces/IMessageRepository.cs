using LearnWithMentorDAL.Entities;

namespace LearnWithMentorDAL.Repositories
{
    public interface IMessageRepository : IRepository<Message>
    {
        Message Get(int id);
    }
}
