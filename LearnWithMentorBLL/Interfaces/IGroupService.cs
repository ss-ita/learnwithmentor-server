using System.Collections.Generic;
using System.Threading.Tasks;
using LearnWithMentorDTO;

namespace LearnWithMentorBLL.Interfaces
{
    public interface IGroupService: IDisposableService
    {
        GroupDto GetGroupById(int id);
        int GroupsCount();
        int? GetMentorIdByGroup(int groupId);
        IEnumerable<GroupDto> GetGroupsByMentor(int mentorId);
        IEnumerable<UserIdentityDto> GetUsers(int groupId);
        IEnumerable<UserWithImageDto> GetUsersWithImage(int groupId);
        IEnumerable<PlanDto> GetPlans(int groupId);
        bool AddGroup(GroupDto group);        
        bool AddUsersToGroup(int []usersId , int groupId);
        bool AddPlansToGroup(int[] plansId, int groupId);
        IEnumerable<UserIdentityDto> GetUsersNotInGroup(int groupId);
        IEnumerable<UserIdentityDto> SearchUserNotInGroup(string[] searchCases, int groupId);
        IEnumerable<PlanDto> GetPlansNotUsedInGroup(int groupId);
        IEnumerable<PlanDto> SearchPlansNotUsedInGroup(string[] searchCases, int groupId);
        Task<bool> RemoveUserFromGroup(int groupId, int userIdToRemove);
        bool RemovePlanFromGroup(int groupId, int planIdToRemove);
        IEnumerable<GroupDto> GetUserGroups(int userId);
    }
}
