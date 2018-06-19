using System.Linq;
using LearnWithMentorDAL.Entities;
using System.Collections.Generic;

namespace LearnWithMentorDAL.Repositories
{
    public class GroupRepository : BaseRepository<Group>, IGroupRepository
    {
        public GroupRepository(LearnWithMentor_DBEntities context) : base(context)
        {
        }
        public Group Get(int id)
        {
            return context.Groups.FirstOrDefault(group => group.Id == id);
        }

        public IEnumerable<Group> GetGroupsByMentor(int mentor_id)
        {
            return context.Groups.Where(group => group.Mentor_Id == mentor_id);
        }

    }
}
