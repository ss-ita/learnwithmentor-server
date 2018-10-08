using LearnWithMentorDAL.Entities;
using System.Threading.Tasks;

namespace LearnWithMentorDAL.Repositories.Interfaces
{
    public interface IUserGroupRepository:IRepository<UserGroup>
    {
        Task<UserGroup> GetById(int id);
        Task<UserGroup> GetByUserAndGroup(int userId, int groupId);
        Task<bool> Contains(int userId, int groupId);
        Task<int[]> GetGroupsForMentor(int mentorId);
        Task<int[]> GetMentorsForGroup(int groupId);
    }
}