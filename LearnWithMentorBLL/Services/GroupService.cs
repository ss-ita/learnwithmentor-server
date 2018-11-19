using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LearnWithMentorBLL.Interfaces;
using LearnWithMentorDAL.Entities;
using LearnWithMentorDAL.UnitOfWork;
using LearnWithMentorDTO;
using ThreadTask=System.Threading.Tasks;

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

        private  async ThreadTask.Task SetUserTasksByAddingUserAsync(int userId, int groupId)
        {
            var plans = await db.Plans.GetPlansForGroupAsync(groupId);
            Group group = await db.Groups.GetAsync(groupId);
            if(plans == null || group == null)
            {
                return;
            }
            var planTasks = new List<PlanTask>();
            foreach (var plan in plans)
            {
                planTasks.AddRange(plan.PlanTasks);
            }
            foreach (var planTask in planTasks)
            {
                if ((db.UserTasks.GetByPlanTaskForUserAsync(planTask.Id, userId) == null) && (group.Mentor_Id != null))
                {
                    await db.UserTasks.AddAsync(CreateDefaultUserTask(userId, planTask.Id, group.Mentor_Id.Value));
                }
            }
        }
        private async  ThreadTask.Task SetUserTasksByAddingPlanAsync(int planId, int groupId)
        {
            var users = await db.Users.GetUsersByGroupAsync(groupId);
            Group group = await db.Groups.GetAsync(groupId);
            var plan = await db.Plans.Get(planId);
            if (users == null || group == null || plan == null)
            {
                return;
            }
            var planTasks = plan.PlanTasks;
            foreach (var user in users)
            {
                foreach (var planTask in planTasks)
                {
                    if ((db.UserTasks.GetByPlanTaskForUserAsync(planTask.Id, user.Id) == null) && (group.Mentor_Id != null))
                    {
                        await db.UserTasks.AddAsync(CreateDefaultUserTask(user.Id, planTask.Id, group.Mentor_Id.Value));
                    }
                }
            }

        }

        public async ThreadTask.Task<bool> AddGroupAsync(GroupDto group)
        {
            if (string.IsNullOrEmpty(group.Name) || await db.Groups.GroupNameExistsAsync(group.Name))
                return false;
            var groupNew = new Group
            {
                Name = group.Name,
                Mentor_Id = group.MentorId
            };
            await db.Groups.AddAsync(groupNew);
            db.Save();
            return true;
        }

        public async ThreadTask.Task<GroupDto> GetGroupByIdAsync(int id)
        {
            Group group = await db.Groups.GetAsync(id);
            if (group == null)
                return null;
            return new GroupDto(group.Id,
                               group.Name,
                               group.Mentor_Id,
                               await db.Users.ExtractFullNameAsync(group.Mentor_Id));
        }
        
        public async ThreadTask.Task<int?> GetMentorIdByGroupAsync(int groupId)
        {
            GroupDto group = await GetGroupByIdAsync(groupId);
            return group?.MentorId;
        }

        public async ThreadTask.Task<int> GroupsCountAsync()
        {
            return await db.Groups.CountAsync();
        }

        public async ThreadTask.Task<IEnumerable<PlanDto>> GetPlansAsync(int groupId)
        {
            var group = await db.Groups.GetAsync(groupId);
            var plans = await db.Plans.GetPlansForGroupAsync(groupId);

            if (group == null)
                return Enumerable.Empty<PlanDto>();
            if (plans == null)
                return Enumerable.Empty<PlanDto>();
            var planList = new List<PlanDto>();
            foreach (var plan in plans)
            {
                planList.Add(new PlanDto(plan.Id,
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
                                     plan.Mod_Date,
                                     plan.IsPrivate
                                    ));
            }
            return planList;
        }

        public async ThreadTask.Task<IEnumerable<UserIdentityDto>> GetUsersAsync(int groupId)
        {
            var group = await db.Groups.GetGroupsByMentorAsync(groupId);
            var users = await db.Users.GetUsersByGroupAsync(groupId);
            if (group == null)
            {
                return null;
            }
            if (users == null)
            {
                return null;
            }
            var userList = new List<UserIdentityDto>();
            foreach (var user in users)
            {
                userList.Add(new UserIdentityDto(user.Email,
                                     null,
                                     user.Id,
                                     user.FirstName,
                                     user.LastName,
                                     user.Role.Name,
                                     user.Blocked,
                                     user.Email_Confirmed
                                    ));
            }
            return userList;
        }

        public async ThreadTask.Task<IEnumerable<UserWithImageDto>> GetUsersWithImageAsync(int groupId)
        {
            var group = await db.Groups.GetGroupsByMentorAsync(groupId);
            var users = await db.Users.GetUsersByGroupAsync(groupId);
            if (group == null)
            {
                return null;
            }
            if (users == null)
            {
                return null;
            }
            var userList = new List<UserWithImageDto>();
            foreach (var user in users)
            {
                User userToGetImage = await db.Users.GetAsync(user.Id);
                userList.Add(new UserWithImageDto(user.Email,
                    user.Id,
                    user.FirstName,
                    user.LastName,
                    user.Role.Name,
                    user.Blocked,
                    new ImageDto()
                    {
                        Name = userToGetImage.Image_Name,
                        Base64Data = userToGetImage.Image
                    }
                ));
            }
            return userList;

        }

        public async ThreadTask.Task<IEnumerable<GroupDto>> GetGroupsByMentorAsync(int mentorId)
        {
            var groups = await db.Groups.GetGroupsByMentorAsync(mentorId);
            if (groups == null)
            {
                return null;
            }
            var groupList = new List<GroupDto>();
            foreach (var group in groups)
            {
                groupList.Add(new GroupDto(group.Id,
                                         group.Name,
                                         group.Mentor_Id,
                                         await db.Users.ExtractFullNameAsync(group.Mentor_Id)));
            }
            return groupList;
        }

        public async ThreadTask.Task<IEnumerable<GroupDto>> GetUserGroupsAsync(int userId)
        {
            User user = await db.Users.GetAsync(userId);
            if (user == null)
            {
                return null;
            }
            IEnumerable<Group> groups;
            if (user.Role.Name == "Mentor")
            { 
                groups = await db.Groups.GetGroupsByMentorAsync(userId);
            }
            else if (user.Role.Name == "Student")
            {
                groups =  await db.Groups.GetStudentGroupsAsync(userId);
            }
            else
            {
                groups =  await db.Groups.GetAll();
            }
            if (groups == null)
            {
                return null;
            }
            var groupList = new List<GroupDto>();
            foreach (var group in groups)
            {
                groupList.Add(new GroupDto(group.Id,
                                         group.Name,
                                         group.Mentor_Id,
                                         await db.Users.ExtractFullNameAsync(group.Mentor_Id)));
            }
            if (groupList.Count < 1)
            {
                return null;
            }
            return groupList;
        }

        public async ThreadTask.Task<bool> AddUsersToGroupAsync(int[] usersId, int groupId)
        {
            Group groups = await db.Groups.GetAsync(groupId);
            if (groups == null)
            {
                return false;
            }
            var added = false;
            foreach (var userId in usersId)
            {
                User addUser = await db.Users.GetAsync(userId);
                if (addUser != null)
                {
                    added = await db.Groups.AddUserToGroupAsync(userId, groupId);
                    if(added)
                    {
                        await SetUserTasksByAddingUserAsync(userId, groupId);
                    }
                    db.Save();
                }
            }
            return added;
        }

        public async ThreadTask.Task<bool> AddPlansToGroupAsync(int[] plansId, int groupId)
        {
            Group groups = await db.Groups.GetAsync(groupId);
            if (groups == null)
            {
                return false;
            }
            var added = false;
            foreach (var planId in plansId)
            {
                var addPlan = await db.Plans.Get(planId);
                if (addPlan != null)
                {
                    added = await db.Groups.AddPlanToGroupAsync(planId, groupId);
                    if(added)
                    {
                      await SetUserTasksByAddingPlanAsync(planId, groupId);
                    }
                    db.Save();
                }
            }
            return added;
        }

        public async ThreadTask.Task<IEnumerable<UserIdentityDto>> GetUsersNotInGroupAsync(int groupId)
        {
            Group group = await db.Groups.GetAsync(groupId);
            if (group == null)
            {
                return null;
            }
            var usersNotInGroup = await db.Users.GetUsersNotInGroupAsync(groupId);
            if (usersNotInGroup == null)
            {
                return null;
            }
            var usersNotInGroupList = new List<UserIdentityDto>();
            foreach (var user in usersNotInGroup)
            {
                var rdDto = new UserIdentityDto(user.Email,
                    null,
                    user.Id,
                    user.FirstName,
                    user.LastName,
                    user.Role.Name,
                    user.Blocked,
                    user.Email_Confirmed);
                if (!usersNotInGroupList.Contains(rdDto))
                {
                    usersNotInGroupList.Add(rdDto);
                }
            }
            return usersNotInGroupList;
        }

        public async ThreadTask.Task<IEnumerable<UserIdentityDto>> SearchUserNotInGroupAsync(string[] searchCases, int groupId)
        {
            IEnumerable<UserIdentityDto> usersNotInGroup = await GetUsersNotInGroupAsync(groupId);
            var usersNotInGroupdto = new List<UserIdentityDto>();
            foreach (var searchCase in searchCases)
            {
                foreach (var user in usersNotInGroup)
                {
                    if ((user.FirstName.Contains(searchCase) || user.LastName.Contains(searchCase)) && (!usersNotInGroupdto.Contains((user))))
                    {
                        usersNotInGroupdto.Add(user);
                    }
                }
            }
            return usersNotInGroupdto;
        }

        public async ThreadTask.Task<IEnumerable<PlanDto>> GetPlansNotUsedInGroupAsync(int groupId)
        {
            Group group = await db.Groups.GetAsync(groupId);
            if (group == null)
            {
                return null;
            }
            var plansNotUsedInGroup = await db.Plans.GetPlansNotUsedInGroupAsync(groupId);
            if (plansNotUsedInGroup == null)
            {
                return null;
            }
            var plansNotUsedInGroupList = new List<PlanDto>();
            foreach (var plan in plansNotUsedInGroup)
            {
                var planDto = new PlanDto
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
                    plan.Mod_Date,
                    plan.IsPrivate);

                if (!plansNotUsedInGroupList.Contains(planDto))
                {
                    plansNotUsedInGroupList.Add(planDto);
                }
            }
            return plansNotUsedInGroupList;
        }
        public async ThreadTask.Task<IEnumerable<PlanDto>> SearchPlansNotUsedInGroupAsync(string[] searchCases, int groupId)
        {
            var plansNotInGroup = await GetPlansNotUsedInGroupAsync(groupId);
            plansNotInGroup = plansNotInGroup.ToList();
            var plansNotInGroupdto = new List<PlanDto>();
            foreach (var searchCase in searchCases)
            {
                foreach (var plan in plansNotInGroup)
                {
                    if ((plan.Name.ToLower().Contains(searchCase.ToLower())) && (!plansNotInGroupdto.Contains(plan)))
                    {
                        plansNotInGroupdto.Add(plan);
                    }
                }
            }
            return plansNotInGroupdto;
        }

        private async ThreadTask.Task RemoveMessagesForUserTaskAsync(int userTaskId)
        {
            var messages = await db.Messages.GetByUserTaskIdAsync(userTaskId);
            if (!messages.Any())
            {
                return;
            }
            foreach (var message in messages)
            {
                await db.Messages.RemoveAsync(message);
            }
        }

        private async  ThreadTask.Task<bool> IsSamePlanAndUserInOtherGroup(Plan plan, User user)
        {
            var matchNumber = 0;
            foreach (var group in await db.Groups.GetAll())
            {
                if (group.Users.Contains(user) && group.Plans.Contains(plan))
                {
                    ++matchNumber;
                }
            }
            return matchNumber > 1;
        }

        private async ThreadTask.Task DeleteUserTasksOnRemovingUserAsync(int groupId, int userId)
        {
            Group group = await db.Groups.GetAsync(groupId);
            var user = await db.Users.GetAsync(userId);
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
                if (await IsSamePlanAndUserInOtherGroup(plan, user))
                {
                    continue;
                }
                foreach (var planTask in plan.PlanTasks)
                {
                    UserTask userTask = await db.UserTasks.GetByPlanTaskForUserAsync(planTask.Id, user.Id);
                    if (userTask == null)
                    {
                        continue;
                    }
                    await RemoveMessagesForUserTaskAsync(userTask.Id);
                    await db.UserTasks.RemoveAsync(userTask);
                }
            }
        }

        public async ThreadTask.Task<bool> RemoveUserFromGroupAsync(int groupId, int userIdToRemove)
        {
            var group = await db.Groups.GetAsync(groupId);
            User userToRemove = await db.Users.GetAsync(userIdToRemove);
            if (group == null)
            {
                return false;
            }
            if (userToRemove == null)
            {
                return false;
            }
            await DeleteUserTasksOnRemovingUserAsync(groupId, userIdToRemove);
            group.Users.Remove(userToRemove);
            db.Save();
            return true;
        }

        private async ThreadTask.Task DeleteUserTasksOnRemovingPlanAsync(int groupId, int planId)
        {
            Group group = await db.Groups.GetAsync(groupId);
            var plan = await db.Plans.Get(planId);
            if (group?.Users == null || plan?.PlanTasks == null)
            {
                return;
            }
            foreach (var user in group.Users)
            {
                if (await IsSamePlanAndUserInOtherGroup(plan, user))
                {
                    continue;
                }
                foreach (var planTask in plan.PlanTasks)
                {

                    UserTask userTask = await db.UserTasks.GetByPlanTaskForUserAsync(planTask.Id, user.Id);
                    if (userTask == null)
                    {
                        continue;
                    }
                    await RemoveMessagesForUserTaskAsync(userTask.Id);
                    await db.UserTasks.RemoveAsync(userTask);
                }
            }
        }

        public async ThreadTask.Task<bool> RemovePlanFromGroupAsync(int groupId, int planIdToRemove)
        {
            var group = await db.Groups.GetAsync(groupId);
            var planToRemove = await db.Plans.Get(planIdToRemove);
            if (group == null)
            {
                return false;
            }
            if (planToRemove == null)
            {
                return false;
            }
            await DeleteUserTasksOnRemovingPlanAsync(groupId, planIdToRemove);
            group.Plans.Remove(planToRemove);
            db.Save();
            return true;
        }
    }
}
