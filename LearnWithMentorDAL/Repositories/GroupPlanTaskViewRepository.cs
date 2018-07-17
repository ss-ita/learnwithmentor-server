using LearnWithMentorDAL.Entities;
using LearnWithMentorDAL.Repositories.Interfaces;

namespace LearnWithMentorDAL.Repositories
{
    public class GroupPlanTaskViewRepository: BaseRepository<GROUP_PLAN_TASK>, IGroupPlanTaskViewRepository
    {
        public GroupPlanTaskViewRepository(LearnWithMentor_DBEntities context) : base(context) { }
    }
}