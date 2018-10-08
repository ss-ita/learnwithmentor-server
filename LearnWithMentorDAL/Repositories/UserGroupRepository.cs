using System.Linq;
using System.Data.Entity;
using System.Threading.Tasks;
using LearnWithMentorDAL.Entities;
using LearnWithMentorDAL.Repositories.Interfaces;

namespace LearnWithMentorDAL.Repositories
{
    public class UserGroupRepository: BaseRepository<UserGroup>, IUserGroupRepository
    {
        public UserGroupRepository(LearnWithMentorContext context) : base(context)
        {
        }

        public Task<UserGroup> GetById(int id)
        {
            return Context.UserGroups.FirstOrDefaultAsync(x => x.Id == id);
        }

        public Task<UserGroup> GetByUserAndGroup(int userId, int groupId)
        {
            return Context.UserGroups.FirstOrDefaultAsync(x => x.UserId == userId && x.GroupId == groupId);
        }

        public Task<bool> Contains(int userId, int groupId)
        {
            return Context.UserGroups.AnyAsync(x => x.UserId == userId && x.GroupId == groupId);
        }

        public Task<int[]> GetGroupsForMentor(int mentorId)
        {
            return Context.UserGroups.Where(x => x.UserId == mentorId).Select(x => x.GroupId).ToArrayAsync();
        }

        public Task<int[]> GetMentorsForGroup(int groupId)
        {
            return Context.UserGroups.Where(x => x.UserId == groupId).Select(x => x.UserId).ToArrayAsync();
        }
    }
}
