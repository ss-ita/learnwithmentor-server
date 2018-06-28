using System;
using System.Collections.Generic;
using LearnWithMentorBLL.Infrastructure;
using LearnWithMentorBLL.Interfaces;
using LearnWithMentorDAL;
using LearnWithMentorDAL.Entities;
using LearnWithMentorDTO;

namespace LearnWithMentorBLL.Services
{
    public class GroupService : BaseService, IGroupService
    {
        public GroupService() : base()
        {
        }
        public bool AddGroup(GroupDTO group)
        {           
                if (string.IsNullOrEmpty(group.Name))
                    return false;
                var groupNew = new Group
                {
                    Name = group.Name,
                    Mentor_Id = group.MentorID
                };
                db.Groups.Add(groupNew);
                db.Save();
                return true;            
        }
        
        
        public GroupDTO GetGroupById(int id)
        {
            Group group = db.Groups.Get(id);
            if (group == null)
                return null;
            return new GroupDTO(group.Id,
                               group.Name,
                               group.Mentor_Id
                               );
        }
        public IEnumerable<PlanDTO> GetPlans(int groupId)
        {
            var group = db.Groups.GetGroupsByMentor(groupId);
            var plans = db.Plans.GetPlansForGroup(groupId);
            if (group == null)
                return null;
            if (plans == null)
                return null;
            List<PlanDTO> planList = new List<PlanDTO>();
            foreach (var plan in plans)
            {
                planList.Add(new PlanDTO(plan.Id,
                                     plan.Name,
                                     plan.Description,
                                     plan.Published,
                                     plan.Create_Id,
                                     plan.Creator.FirstName,
                                     plan.Creator.LastName,
                                     plan.Mod_Id,
                                     plan.Modifier?.FirstName,
                                     plan.Modifier?.LastName,
                                     plan.Create_Date,
                                     plan.Mod_Date
                                    ));
            }
            return planList;
        }
            public IEnumerable<UserDTO> GetUsers(int groupId)
        {
            var group = db.Groups.GetGroupsByMentor(groupId);
            var users = db.Users.GetUsersByGroup(groupId);
            if (group == null)
                return null;
            if (users == null)
                return null;
            List<UserDTO> userList = new List<UserDTO>();
            foreach (var user in users)
            {
                userList.Add(new UserDTO(user.Id,
                                     user.FirstName,
                                     user.LastName,
                                     user.Roles.Name,
                                     user.Blocked
                                    ));
            }
            return userList;

        }

            public IEnumerable<GroupDTO> GetGroupsByMentor(int mentorId)
        {

           var groups = db.Groups.GetGroupsByMentor(mentorId);
            if (groups == null)
                return null;
            List<GroupDTO> groupList = new List<GroupDTO>();
            foreach (var group in groups)
            {
                groupList.Add(new GroupDTO(group.Id,
                                         group.Name,
                                         group.Mentor_Id
                                        ));
            }
            return groupList;
        }

        public bool AddUsersToGroup(int[] allUsersId, int groupId)
        {
            var groups = db.Groups.Get(groupId);
            if (groups == null)
                return false;
            bool added = false;
            foreach (int userId in allUsersId)
            {
                var addUser = db.Users.Get(userId);
                if(addUser != null)
                {
                    added = db.Groups.AddUserToGroup(userId, groupId);
                    db.Save();
                }
            }
            return added;
        }

        public bool AddPlansToGroup(int[] allPlansId, int groupId)
        {
            var groups = db.Groups.Get(groupId);
            if (groups == null)
                return false;
            bool added = false;
            foreach (int planId in allPlansId)
            {
                var addPlan = db.Plans.Get(planId);
                if (addPlan != null)
                {
                    added = db.Groups.AddPlanToGroup(planId, groupId);
                    db.Save();
                }
            }
            return added;
        }

        public IEnumerable<UserDTO> GetUsersNotInGroup(int groupId)
        {
            var group = db.Groups.Get(groupId);
            if (group == null)
                return null;
            var usersNotInGroup = db.Users.GetUsersNotInGroup(groupId);
            if (usersNotInGroup == null)
                return null;
            List<UserDTO> usersNotInGroupList = new List<UserDTO>();
            foreach (var user in usersNotInGroup)
            {
                UserDTO rdDto = new UserDTO(
                    user.Id,
                    user.FirstName,
                    user.LastName,
                    user.Roles.Name,
                    user.Blocked);
                if (!usersNotInGroupList.Contains(rdDto))
                    usersNotInGroupList.Add(rdDto);
            }
            return usersNotInGroupList;
        }

        public IEnumerable<UserDTO> SearchUserNotInGroup(string[] searchCases, int groupId)
        {
            var usersNotInGroup = GetUsersNotInGroup(groupId);
            List<UserDTO> usersNotInGroupdto = new List<UserDTO>();
            foreach (var searchCase in searchCases)
            {
                foreach (var user in usersNotInGroup)
                {
                    if (user.FirstName.Contains(searchCase) || user.LastName.Contains(searchCase))
                    {
                        if (!usersNotInGroupdto.Contains((user)))
                            usersNotInGroupdto.Add(user);
                    }
                }
            }
            return usersNotInGroupdto;
        }

    }
}
