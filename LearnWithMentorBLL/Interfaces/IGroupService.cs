using System.Collections.Generic;
using System.Threading.Tasks;
using LearnWithMentorDTO;

namespace LearnWithMentorBLL.Interfaces
{
    public interface IGroupService : IDisposableService
    {
        Task<GroupDto> GetGroupByIdAsync(int id);
        Task<int> GroupsCount();
        Task<int?> GetMentorIdByGroup(int groupId);
        Task<IEnumerable<GroupDto>> GetGroupsByMentorAsync(int mentorId);
        Task<IEnumerable<UserIdentityDto>> GetUsersAsync(int groupId);
        Task<IEnumerable<UserWithImageDto>> GetUsersWithImageAsync(int groupId);
        Task<IEnumerable<PlanDto>> GetPlans(int groupId);
        Task<bool> AddGroup(GroupDto group);        
        Task<bool> AddUsersToGroupAsync(int[] usersId, int groupId);
        Task<bool> AddPlansToGroup(int[] plansId, int groupId);
        Task<IEnumerable<UserIdentityDto>> GetUsersNotInGroupAsync(int groupId);
        Task<IEnumerable<UserIdentityDto>> SearchUserNotInGroupAsync(string[] searchCases, int groupId);
        Task<IEnumerable<PlanDto>> GetPlansNotUsedInGroup(int groupId);
        Task<IEnumerable<PlanDto>> SearchPlansNotUsedInGroup(string[] searchCases, int groupId);
        Task<bool> RemoveUserFromGroupAsync(int groupId, int userIdToRemove);
        Task<bool> RemovePlanFromGroupAsync(int groupId, int planIdToRemove);
        Task<IEnumerable<GroupDto>> GetUserGroups(int userId);
    }
}
