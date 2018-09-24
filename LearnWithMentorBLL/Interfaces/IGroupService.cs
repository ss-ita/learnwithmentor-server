using System.Collections.Generic;
using System.Threading.Tasks;
using LearnWithMentorDTO;
using TaskThread = System.Threading.Tasks;

namespace LearnWithMentorBLL.Interfaces
{
    public interface IGroupService : IDisposableService
    {
        GroupDto GetGroupById(int id);
        int GroupsCount();
        int? GetMentorIdByGroup(int groupId);
        IEnumerable<GroupDto> GetGroupsByMentor(int mentorId);
        IEnumerable<UserIdentityDto> GetUsers(int groupId);
        Task<IEnumerable<UserWithImageDto>> GetUsersWithImage(int groupId);
        TaskThread.Task<IEnumerable<PlanDto>> GetPlans(int groupId);
        bool AddGroup(GroupDto group);
        Task<bool> AddUsersToGroup(int[] usersId, int groupId);
        TaskThread.Task<bool> AddPlansToGroup(int[] plansId, int groupId);
        IEnumerable<UserIdentityDto> GetUsersNotInGroup(int groupId);
        IEnumerable<UserIdentityDto> SearchUserNotInGroup(string[] searchCases, int groupId);
        TaskThread.Task<IEnumerable<PlanDto>> GetPlansNotUsedInGroup(int groupId);
        TaskThread.Task<IEnumerable<PlanDto>> SearchPlansNotUsedInGroup(string[] searchCases, int groupId);
        Task<bool> RemoveUserFromGroup(int groupId, int userIdToRemove);
        TaskThread.Task<bool> RemovePlanFromGroup(int groupId, int planIdToRemove);
        Task<IEnumerable<GroupDto>> GetUserGroups(int userId);
    }
}
