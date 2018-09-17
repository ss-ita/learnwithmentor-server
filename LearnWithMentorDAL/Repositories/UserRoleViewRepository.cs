using LearnWithMentorDAL.Entities;
using LearnWithMentorDAL.Repositories.Interfaces;

namespace LearnWithMentorDAL.Repositories
{
    public class UserRoleViewRepository: BaseRepository<USER_ROLE>, IUserRoleViewRepository
    {
        public UserRoleViewRepository(LearnWithMentorContext context) : base(context)
        {
        }
    }
}