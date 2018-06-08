using LearnWithMentorDAL.Entities;

namespace LearnWithMentorDAL.Repositories
{
    public class GroupPlanTaskViewRepository: BaseRepository<GROUP_PLAN_TASK>, IGroupPlanTaskViewRepository
    {
        public GroupPlanTaskViewRepository(LearnWithMentor_DBEntities context) : base(context) { }
    }
}