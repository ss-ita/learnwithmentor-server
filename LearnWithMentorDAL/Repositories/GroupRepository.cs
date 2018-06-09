using System.Linq;
using LearnWithMentorDAL.Entities;

namespace LearnWithMentorDAL.Repositories
{
    public class GroupRepository : BaseRepository<Group>, IGroupRepository
    {
        public GroupRepository(LearnWithMentor_DBEntities context) : base(context)
        {
        }
        public Group Get(int id)
        {
            return context.Groups.FirstOrDefault(g => g.Id == id);
        }
    }
}
