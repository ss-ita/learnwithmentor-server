using System.Collections.Generic;
using System.Linq;
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
            var planTasks = new List<PlanTask>();
            foreach(var plan in plans)
            {
                planTasks.AddRange(plan.PlanTasks);
            }
            foreach(var planTask in planTasks)
            {
                if(db.UserTasks.GetByPlanTaskForUser(planTask.Id, userId) == null)
                {
                    if (group.Mentor_Id != null)
                    {
                        db.UserTasks.Add(CreateDefaultUserTask(userId, planTask.Id, group.Mentor_Id.Value));
                    }
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
                        if (group.Mentor_Id != null)
                        {
                            db.UserTasks.Add(CreateDefaultUserTask(user.Id, planTask.Id, group.Mentor_Id.Value));
                        }
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
            var group = db.Groups.Get(id);
            if (group == null)
                return null;
            return new GroupDTO(group.Id,
                               group.Name,
                               group.Mentor_Id,
                               db.Users.ExtractFullName(group.Mentor_Id));
        }

        public int? GetMentorIdByGroup(int groupId)
        {
            return GetGroupById(groupId)?.MentorId;
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
            var planList = new List<PlanDTO>();
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
            {
                return null;
            }
            if (users == null)
            {
                return null;
            }
            var userList = new List<UserIdentityDTO>();
            foreach (var user in users)
            {
                userList.Add(new UserIdentityDTO(user.Email,
                                     null,
                                     user.Id,
                                     user.FirstName,
                                     user.LastName,
                                     user.Roles.Name,
                                     user.Blocked,
                                     user.Email_Confirmed
                                    ));
            }
            return userList;
        }

        public IEnumerable<UserWithImageDTO> GetUsersWithImage(int groupId)
        {
            var group = db.Groups.GetGroupsByMentor(groupId);
            var users = db.Users.GetUsersByGroup(groupId);
            if (group == null)
            {
                return null;
            }
            if (users == null)
            {
                return null;
            }
            var userList = new List<UserWithImageDTO>();
            foreach (var user in users)
            {
                var userToGetImage = db.Users.Get(user.Id);
                userList.Add(new UserWithImageDTO(user.Email,
                    user.Id,
                    user.FirstName,
                    user.LastName,
                    user.Roles.Name,
                    user.Blocked,
                    new ImageDTO()
                    {
                        Name = userToGetImage.Image_Name,
                        Base64Data = userToGetImage.Image
                    }
                ));
            }
            return userList;

        }

        public IEnumerable<GroupDTO> GetGroupsByMentor(int mentorId)
        {
            var groups = db.Groups.GetGroupsByMentor(mentorId);
            if (groups == null)
            {
                return null;
            }
            var groupList = new List<GroupDTO>();
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
            {
                return null;
            }
            IEnumerable<Group> groups;
            if (user.Roles.Name == "Mentor")
            { 
                groups = db.Groups.GetGroupsByMentor(userId);
            }
            else if (user.Roles.Name == "Student")
            {
                groups = db.Groups.GetStudentGroups(userId);
            }
            else
            {
                groups = db.Groups.GetAll();
            }
            if (groups == null)
            {
                return null;
            }
            var groupList = new List<GroupDTO>();
            foreach (var group in groups)
            {
                groupList.Add(new GroupDTO(group.Id,
                                         group.Name,
                                         group.Mentor_Id,
                                         db.Users.ExtractFullName(group.Mentor_Id)));
            }
            if (groupList.Count < 1)
            {
                return null;
            }
            return groupList;
        }

        public bool AddUsersToGroup(int[] allUsersId, int groupId)
        {
            var groups = db.Groups.Get(groupId);
            if (groups == null)
            {
                return false;
            }
            var added = false;
            foreach (var userId in allUsersId)
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
            {
                return false;
            }
            var added = false;
            foreach (var planId in allPlansId)
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
            {
                return null;
            }
            var usersNotInGroup = db.Users.GetUsersNotInGroup(groupId);
            if (usersNotInGroup == null)
            {
                return null;
            }
            var usersNotInGroupList = new List<UserIdentityDTO>();
            foreach (var user in usersNotInGroup)
            {
                var rdDto = new UserIdentityDTO(user.Email,
                    null,
                    user.Id,
                    user.FirstName,
                    user.LastName,
                    user.Roles.Name,
                    user.Blocked,
                    user.Email_Confirmed);
                if (!usersNotInGroupList.Contains(rdDto))
                {
                    usersNotInGroupList.Add(rdDto);
                }
            }
            return usersNotInGroupList;
        }

        public IEnumerable<UserIdentityDTO> SearchUserNotInGroup(string[] searchCases, int groupId)
        {
            var usersNotInGroup = GetUsersNotInGroup(groupId).ToList();
            var usersNotInGroupdto = new List<UserIdentityDTO>();
            foreach (var searchCase in searchCases)
            {
                foreach (var user in usersNotInGroup)
                {
                    if (user.FirstName.Contains(searchCase) || user.LastName.Contains(searchCase))
                    {
                        if (!usersNotInGroupdto.Contains((user)))
                        {
                            usersNotInGroupdto.Add(user);
                        }
                    }
                }
            }
            return usersNotInGroupdto;
        }

        public IEnumerable<PlanDTO> GetPlansNotUsedInGroup(int groupId)
        {
            var group = db.Groups.Get(groupId);
            if (group == null)
            {
                return null;
            }
            var plansNotUsedInGroup = db.Plans.GetPlansNotUsedInGroup(groupId);
            if (plansNotUsedInGroup == null)
            {
                return null;
            }
            var plansNotUsedInGroupList = new List<PlanDTO>();
            foreach (var plan in plansNotUsedInGroup)
            {
                var planDto = new PlanDTO
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
                {
                    plansNotUsedInGroupList.Add(planDto);
                }
            }
            return plansNotUsedInGroupList;
        }

        public IEnumerable<PlanDTO> SearchPlansNotUsedInGroup(string[] searchCases, int groupId)
        {
            var plansNotInGroup = GetPlansNotUsedInGroup(groupId).ToList();
            var plansNotInGroupdto = new List<PlanDTO>();
            foreach (var searchCase in searchCases)
            {
                foreach (var plan in plansNotInGroup)
                {
                    if (plan.Name.ToLower().Contains(searchCase.ToLower()))
                    {
                        if (!plansNotInGroupdto.Contains(plan))
                        {
                            plansNotInGroupdto.Add(plan);
                        }
                    }
                }
            }
            return plansNotInGroupdto;
        }

        private void RemoveMessagesForUserTask(int userTaskId)
        {
            var messages = db.Messages.GetByUserTaskId(userTaskId).ToList();
            if (!messages.Any())
            {
                return;
            }
            foreach (var message in messages)
            {
                db.Messages.Remove(message);
            }
        }

        private bool IsSamePlanAndUserInOtherGroup(Plan plan, User user)
        {
            var matchNumber = 0;
            foreach (var group in db.Groups.GetAll())
            {
                if (group.Users.Contains(user) && group.Plans.Contains(plan))
                {
                    ++matchNumber;
                }
            }
            return matchNumber > 1;
        }

        private void DeleteUserTasksOnRemovingUser(int groupId, int userId)
        {
            var group = db.Groups.Get(groupId);
            var user = db.Users.Get(userId);
            if (group?.Plans == null || user == null)
            {
                return;
            }
            foreach (var plan in group.Plans)
            {
                if (plan?.PlanTasks == null)
                {
                    continue;
                }
                if (IsSamePlanAndUserInOtherGroup(plan, user))
                {
                    continue;
                }
                foreach (var planTask in plan.PlanTasks)
                {
                    var userTask = db.UserTasks.GetByPlanTaskForUser(planTask.Id, user.Id);
                    if (userTask == null)
                    {
                        continue;
                    }
                    RemoveMessagesForUserTask(userTask.Id);
                    db.UserTasks.Remove(userTask);
                }
            }
        }

        public bool RemoveUserFromGroup(int groupId, int userIdToRemove)
        {
            var group = db.Groups.Get(groupId);
            var userToRemove = db.Users.Get(userIdToRemove);
            if (group == null)
            {
                return false;
            }
            if (userToRemove == null)
            {
                return false;
            }
            DeleteUserTasksOnRemovingUser(groupId, userIdToRemove);
            group.Users.Remove(userToRemove);
            db.Save();
            return true;
        }

        private void DeleteUserTasksOnRemovingPlan(int groupId, int planId)
        {
            var group = db.Groups.Get(groupId);
            var plan = db.Plans.Get(planId);
            if (group?.Users == null || plan?.PlanTasks == null)
            {
                return;
            }
            foreach (var user in group.Users)
            {
                if (IsSamePlanAndUserInOtherGroup(plan, user))
                {
                    continue;
                }
                foreach (var planTask in plan.PlanTasks)
                {
                    var userTask = db.UserTasks.GetByPlanTaskForUser(planTask.Id, user.Id);
                    if (userTask == null)
                    {
                        continue;
                    }
                    RemoveMessagesForUserTask(userTask.Id);
                    db.UserTasks.Remove(userTask);
                }
            }
        }

        public bool RemovePlanFromGroup(int groupId, int planIdToRemove)
        {
            var group = db.Groups.Get(groupId);
            var planToRemove = db.Plans.Get(planIdToRemove);
            if (group == null)
            {
                return false;
            }
            if (planToRemove == null)
            {
                return false;
            }
            DeleteUserTasksOnRemovingPlan(groupId, planIdToRemove);
            group.Plans.Remove(planToRemove);
            db.Save();
            return true;
        }
    }
}
