﻿using System.Collections.Generic;
using LearnWithMentorBLL.Interfaces;
using LearnWithMentorDAL.Entities;
using LearnWithMentorDAL.UnitOfWork;
using LearnWithMentorDTO;

namespace LearnWithMentorBLL.Services
{
    public class GroupService : BaseService, IGroupService
    {
        public GroupService(IUnitOfWork db) : base(db)
        {
        }

        private UserTask CreateDefaultUserTask(int userId, int planTaskId, int mentorId)
        {
            return new UserTask()
            {
                User_Id = userId,
                PlanTask_Id = planTaskId,
                Mentor_Id = mentorId,
                Result = "",
                State = "P"
            };
        }

        private void SetUserTasksByAddingUser(int userId, int groupId)
        {
            var plans = db.Plans.GetPlansForGroup(groupId);
            var group = db.Groups.Get(groupId);
            if(plans == null || group == null)
            {
                return;
            }
            List<PlanTask> planTasks = new List<PlanTask>();
            foreach(var plan in plans)
            {
                planTasks.AddRange(plan.PlanTasks);
            }
            foreach(var planTask in planTasks)
            {
                if(db.UserTasks.GetByPlanTaskForUser(planTask.Id, userId) == null)
                {
                    db.UserTasks.Add(CreateDefaultUserTask(userId, planTask.Id, group.Mentor_Id.Value));
                }
            }
        }

        private void SetUserTasksByAddingPlan(int planId, int groupId)
        {
            var users = db.Users.GetUsersByGroup(groupId);
            var group = db.Groups.Get(groupId);
            var plan = db.Plans.Get(planId);
            if (users == null || group == null || plan == null)
            {
                return;
            }
            var planTasks = plan.PlanTasks;
            foreach(var user in users)
            {
                foreach( var planTask in planTasks)
                {
                    if (db.UserTasks.GetByPlanTaskForUser(planTask.Id, user.Id) == null)
                    {
                        db.UserTasks.Add(CreateDefaultUserTask(user.Id, planTask.Id, group.Mentor_Id.Value));
                    }
                }
            }
           
        }

        public bool AddGroup(GroupDTO group)
        {
            if (string.IsNullOrEmpty(group.Name) || db.Groups.GroupNameExists(group.Name))
                return false;
            var groupNew = new Group
            {
                Name = group.Name,
                Mentor_Id = group.MentorId
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
                               group.Mentor_Id,
                               db.Users.ExtractFullName(group.Mentor_Id));
        }
        public int GroupsCount()
        {
            return db.Groups.Count();
        }
        public IEnumerable<PlanDTO> GetPlans(int groupId)
        {
            var group = db.Groups.Get(groupId);
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

        public IEnumerable<UserIdentityDTO> GetUsers(int groupId)
        {
            var group = db.Groups.GetGroupsByMentor(groupId);
            var users = db.Users.GetUsersByGroup(groupId);
            if (group == null)
                return null;
            if (users == null)
                return null;
            List<UserIdentityDTO> userList = new List<UserIdentityDTO>();
            foreach (var user in users)
            {
                userList.Add(new UserIdentityDTO(user.Email,
                                     null,
                                     user.Id,
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
                                         group.Mentor_Id,
                                         db.Users.ExtractFullName(group.Mentor_Id)));
            }
            return groupList;
        }

        public IEnumerable<GroupDTO> GetUserGroups(int userId)
        {
            var user = db.Users.Get(userId);
            if (user == null)
                return null;
            IEnumerable<Group> groups;
            if (user.Roles.Name == "Mentor")
                groups = db.Groups.GetGroupsByMentor(userId);
            else if (user.Roles.Name == "Student")
                groups = db.Groups.GetStudentGroups(userId);
            else
                groups = db.Groups.GetAll();
            if (groups == null)
                return null;
            List<GroupDTO> groupList = new List<GroupDTO>();
            foreach (var group in groups)
            {
                groupList.Add(new GroupDTO(group.Id,
                                         group.Name,
                                         group.Mentor_Id,
                                         db.Users.ExtractFullName(group.Mentor_Id)));
            }
            if (groupList.Count < 1)
                return null;
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
                if (addUser != null)
                {
                    added = db.Groups.AddUserToGroup(userId, groupId);
                    if(added)
                    {
                        SetUserTasksByAddingUser(userId, groupId);
                    }
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
                    if(added)
                    {
                        SetUserTasksByAddingPlan(planId, groupId);
                    }
                    db.Save();
                }
            }
            return added;
        }

        public IEnumerable<UserIdentityDTO> GetUsersNotInGroup(int groupId)
        {
            var group = db.Groups.Get(groupId);
            if (group == null)
                return null;
            var usersNotInGroup = db.Users.GetUsersNotInGroup(groupId);
            if (usersNotInGroup == null)
                return null;
            List<UserIdentityDTO> usersNotInGroupList = new List<UserIdentityDTO>();
            foreach (var user in usersNotInGroup)
            {
                UserIdentityDTO rdDto = new UserIdentityDTO(user.Email,
                    null,
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

        public IEnumerable<UserIdentityDTO> SearchUserNotInGroup(string[] searchCases, int groupId)
        {
            var usersNotInGroup = GetUsersNotInGroup(groupId);
            List<UserIdentityDTO> usersNotInGroupdto = new List<UserIdentityDTO>();
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

        public IEnumerable<PlanDTO> GetPlansNotUsedInGroup(int groupId)
        {
            var group = db.Groups.Get(groupId);
            if (group == null)
                return null;
            var plansNotUsedInGroup = db.Plans.GetPlansNotUsedInGroup(groupId);
            if (plansNotUsedInGroup == null)
                return null;
            List<PlanDTO> plansNotUsedInGroupList = new List<PlanDTO>();
            foreach (var plan in plansNotUsedInGroup)
            {
                PlanDTO planDto = new PlanDTO
                (plan.Id,
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
                    plan.Mod_Date);

                if (!plansNotUsedInGroupList.Contains(planDto))
                    plansNotUsedInGroupList.Add(planDto);
            }
            return plansNotUsedInGroupList;
        }

        public IEnumerable<PlanDTO> SearchPlansNotUsedInGroup(string[] searchCases, int groupId)
        {
            var plansNotInGroup = GetPlansNotUsedInGroup(groupId);
            List<PlanDTO> plansNotInGroupdto = new List<PlanDTO>();
            foreach (var searchCase in searchCases)
            {
                foreach (var plan in plansNotInGroup)
                {
                    if (plan.Name.Contains(searchCase))
                    {
                        if (!plansNotInGroupdto.Contains(plan))
                            plansNotInGroupdto.Add(plan);
                    }
                }
            }
            return plansNotInGroupdto;
        }

        public bool RemoveUserFromGroup(int groupId, int userIdToRemove)
        {
            var group = db.Groups.Get(groupId);
            var userToRemove = db.Users.Get(userIdToRemove);
            if (group == null)
                return false;
            if (userToRemove == null)
                return false;
            group.Users.Remove(userToRemove);
            db.Save();
            return true;
        }

        public bool RemovePlanFromGroup(int groupId, int planIdToRemove)
        {
            var group = db.Groups.Get(groupId);
            var planToRemove = db.Plans.Get(planIdToRemove);
            if (group == null)
                return false;
            if (planToRemove == null)
                return false;
            group.Plans.Remove(planToRemove);
            db.Save();
            return true;
        }
    }
}
