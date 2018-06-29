using System.Collections.Generic;
using LearnWithMentorDTO;

namespace LearnWithMentorBLL.Interfaces
{
    public interface IGroupService: IDisposableService
    {
        GroupDTO GetGroupById(int id);
        IEnumerable<GroupDTO> GetGroupsByMentor(int mentorId);
        IEnumerable<UserIdentityDTO> GetUsers(int groupId);
        IEnumerable<PlanDTO> GetPlans(int groupId);
        bool AddGroup(GroupDTO group);        
        bool AddUsersToGroup(int []usersId , int groupId);
        bool AddPlansToGroup(int[] usersId, int groupId);
        IEnumerable<UserIdentityDTO> GetUsersNotInGroup(int groupId);
        IEnumerable<UserIdentityDTO> SearchUserNotInGroup(string[] searchCases, int groupId);
        IEnumerable<PlanDTO> GetPlansNotUsedInGroup(int groupId);
        IEnumerable<PlanDTO> SearchPlansNotUsedInGroup(string[] searchCases, int groupId);
    }
}
