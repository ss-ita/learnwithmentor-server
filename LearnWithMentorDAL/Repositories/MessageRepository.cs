using System.Linq;
using LearnWithMentorDAL.Entities;

namespace LearnWithMentorDAL.Repositories
{
    public class MessageRepository : BaseRepository<Message>, IMessageRepository
    {
        public MessageRepository(LearnWithMentor_DBEntities context) : base(context)
        {
        }
        public Message Get(int id)
        {
            return context.Messages.FirstOrDefault(m => m.Id == id);
        }
    }
}
