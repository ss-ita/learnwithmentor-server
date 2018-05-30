using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LearnWithMentorDAL.Entities;

namespace LearnWithMentorDAL.Repositories
{
    public class MessageRepository : BaseRepository<Message>, IMessageRepository
    {
        public MessageRepository(LearnWithMentor_DBEntities _context) : base(_context)
        {
        }
        public Message Get(int id)
        {
            return context.Messages.Where(m => m.Id == id).FirstOrDefault();
        }
    }
}
