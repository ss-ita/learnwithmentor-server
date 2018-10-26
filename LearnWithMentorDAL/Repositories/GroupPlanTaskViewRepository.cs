using LearnWithMentorDAL.Entities;
using LearnWithMentorDAL.Repositories.Interfaces;

namespace LearnWithMentorDAL.Repositories
{
    public class GroupPlanTaskViewRepository: BaseRepository<GroupPlanTask>, IGroupPlanTaskViewRepository
    {
        public GroupPlanTaskViewRepository(LearnWithMentorContext context) : base(context) { }
    }
}