using LearnWithMentorDAL.Entities;
using LearnWithMentorDAL.Repositories.Interfaces;

namespace LearnWithMentorDAL.Repositories
{
    public class UserRoleViewRepository: BaseRepository<UserRole>, IUserRoleViewRepository
    {
        public UserRoleViewRepository(LearnWithMentorContext context) : base(context)
        {
        }
    }
}