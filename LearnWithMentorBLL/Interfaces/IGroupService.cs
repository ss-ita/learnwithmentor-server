using System.Collections.Generic;
using System.Threading.Tasks;
using LearnWithMentorDTO;

namespace LearnWithMentorBLL.Interfaces
{
    public interface IGroupService : IDisposableService
    {
        Task<GroupDto> GetGroupById(int id);
        Task<int> GroupsCount();
        Task<int?> GetMentorIdByGroup(int groupId);
        Task<IEnumerable<GroupDto>> GetGroupsByMentor(int mentorId);
        Task<IEnumerable<UserIdentityDto>> GetUsers(int groupId);
        Task<IEnumerable<UserWithImageDto>> GetUsersWithImage(int groupId);
        Task<IEnumerable<PlanDto>> GetPlans(int groupId);
        Task<bool> AddGroup(GroupDto group);        
        Task<bool> AddUsersToGroup(int[] usersId, int groupId);
        Task<bool> AddPlansToGroup(int[] plansId, int groupId);
        Task<IEnumerable<UserIdentityDto>> GetUsersNotInGroup(int groupId);
        Task<IEnumerable<UserIdentityDto>> SearchUserNotInGroup(string[] searchCases, int groupId);
        Task<IEnumerable<PlanDto>> GetPlansNotUsedInGroup(int groupId);
        Task<IEnumerable<PlanDto>> SearchPlansNotUsedInGroup(string[] searchCases, int groupId);
        Task<bool> RemoveUserFromGroup(int groupId, int userIdToRemove);
        Task<bool> RemovePlanFromGroup(int groupId, int planIdToRemove);
        Task<IEnumerable<GroupDto>> GetUserGroups(int userId);
    }
}
