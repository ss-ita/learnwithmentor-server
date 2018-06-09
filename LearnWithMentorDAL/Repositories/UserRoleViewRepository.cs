using LearnWithMentorDAL.Entities;

namespace LearnWithMentorDAL.Repositories
{
    public class UserRoleViewRepository: BaseRepository<USER_ROLE>, IUserRoleViewRepository

    {
        public UserRoleViewRepository(LearnWithMentor_DBEntities context) : base(context)
        {
        }
    }
}