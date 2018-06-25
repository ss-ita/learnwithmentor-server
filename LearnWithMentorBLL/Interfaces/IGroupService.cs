using System;
using System.Collections.Generic;
using LearnWithMentorDAL;
using LearnWithMentorDAL.Entities;
using LearnWithMentorDTO;

namespace LearnWithMentorBLL.Interfaces
{
    public interface IGroupService: IDisposableService
    {
        GroupDTO GetGroupById(int id);
        IEnumerable<GroupDTO> GetGroupsByMentor(int mentorId);
        IEnumerable<UserDTO> GetUsers(int groupId);
        IEnumerable<PlanDTO> GetPlans(int groupId);
        bool AddGroup(GroupDTO group);        
        bool AddUsersToGroup(int []usersId , int groupId);
        bool AddPlansToGroup(int[] usersId, int groupId);
    }
}
