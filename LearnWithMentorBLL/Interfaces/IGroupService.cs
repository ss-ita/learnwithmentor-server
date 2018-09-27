using System.Collections.Generic;
using System.Threading.Tasks;
using LearnWithMentorDTO;

namespace LearnWithMentorBLL.Interfaces
{
    public interface IGroupService : IDisposableService
    {
        Task<GroupDto> GetGroupByIdAsync(int id);
        Task<int> GroupsCountAsync();
        Task<int?> GetMentorIdByGroupAsync(int groupId);
        Task<IEnumerable<GroupDto>> GetGroupsByMentorAsync(int mentorId);
        Task<IEnumerable<UserIdentityDto>> GetUsersAsync(int groupId);
        Task<IEnumerable<UserWithImageDto>> GetUsersWithImageAsync(int groupId);
        Task<IEnumerable<PlanDto>> GetPlansAsync(int groupId);
        Task<bool> AddGroupAsync(GroupDto group);        
        Task<bool> AddUsersToGroupAsync(int[] usersId, int groupId);
        Task<bool> AddPlansToGroupAsync(int[] plansId, int groupId);
        Task<IEnumerable<UserIdentityDto>> GetUsersNotInGroupAsync(int groupId);
        Task<IEnumerable<UserIdentityDto>> SearchUserNotInGroupAsync(string[] searchCases, int groupId);
        Task<IEnumerable<PlanDto>> GetPlansNotUsedInGroupAsync(int groupId);
        Task<IEnumerable<PlanDto>> SearchPlansNotUsedInGroupAsync(string[] searchCases, int groupId);
        Task<bool> RemoveUserFromGroupAsync(int groupId, int userIdToRemove);
        Task<bool> RemovePlanFromGroupAsync(int groupId, int planIdToRemove);
        Task<IEnumerable<GroupDto>> GetUserGroupsAsync(int userId);
    }
}
