using System;
using System.Collections.Generic;
using LearnWithMentorDAL;
using LearnWithMentorDAL.Entities;
using LearnWithMentorDTO;

namespace LearnWithMentorBLL.Interfaces
{
    public interface IGroupService
    {
        GroupDTO GetGroupById(int id);
        IEnumerable<GroupDTO> GetGroupsByMentor(int mentorId);
        IEnumerable<UserDTO> GetUsers(int groupId);
        bool AddGroup(GroupDTO group);
        bool AddUserToGroup(int userId, int groupId);
        bool AddPlanToGroup(int planId, int groupId);
    }
}
