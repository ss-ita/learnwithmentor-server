using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LearnWithMentorDAL.Entities;

namespace LearnWithMentorDAL.Repositories
{
    public class GroupRepository : BaseRepository<Group>, IGroupRepository
    {
        public GroupRepository(LearnWithMentor_DBEntities _context) : base(_context)
        {
        }
        public Group Get(int id)
        {
            return context.Groups.Where(g => g.Id == id).FirstOrDefault();
        }
    }
}
