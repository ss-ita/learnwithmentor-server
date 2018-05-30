using LearnWithMentorDAL.Entities;

namespace LearnWithMentorDAL.Repositories
{
    public class UserRoleViewRepository: BaseRepository<USER_ROLE>, IUserRoleViewRepository

    {
        public UserRoleViewRepository(LearnWithMentor_DBEntities _context) : base(_context)
        {
        }
    }
}