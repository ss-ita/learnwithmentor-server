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

        private  async ThreadTask.Task SetUserTasksByAddingUser(int userId, int groupId)
        {
            var plans = await db.Plans.GetPlansForGroup(groupId);
            Group group = await db.Groups.Get(groupId);
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
                if ((db.UserTasks.GetByPlanTaskForUser(planTask.Id, userId) == null) && (group.Mentor_Id != null))
                {
                    db.UserTasks.Add(CreateDefaultUserTask(userId, planTask.Id, group.Mentor_Id.Value));
                }
            }
        }
        private async  ThreadTask.Task SetUserTasksByAddingPlan(int planId, int groupId)
        {
            var users = await db.Users.GetUsersByGroup(groupId);
            Group group = await db.Groups.Get(groupId);
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
                    if ((db.UserTasks.GetByPlanTaskForUser(planTask.Id, user.Id) == null) && (group.Mentor_Id != null))
                    {
                        db.UserTasks.Add(CreateDefaultUserTask(user.Id, planTask.Id, group.Mentor_Id.Value));
                    }
                }
            }

        }

        public async ThreadTask.Task<bool> AddGroup(GroupDto group)
        {
            if (string.IsNullOrEmpty(group.Name) || await db.Groups.GroupNameExists(group.Name))
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

        public async ThreadTask.Task<GroupDto> GetGroupById(int id)
        {
            Group group = await db.Groups.Get(id);
            if (group == null)
                return null;
            return new GroupDto(group.Id,
                               group.Name,
                               group.Mentor_Id,
                               await db.Users.ExtractFullName(group.Mentor_Id));
        }
        
        public async ThreadTask.Task<int?> GetMentorIdByGroup(int groupId)
        {
            GroupDto group = await GetGroupById(groupId);
            return group?.MentorId;
        }

        public async ThreadTask.Task<int> GroupsCount()
        {
            return await db.Groups.Count();
        }

        public async ThreadTask.Task<IEnumerable<PlanDto>> GetPlans(int groupId)
        {
            var group = await db.Groups.Get(groupId);
            var plans = await db.Plans.GetPlansForGroup(groupId);

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
                                     plan.Mod_Date
                                    ));
            }
            return planList;
        }

        public async ThreadTask.Task<IEnumerable<UserIdentityDto>> GetUsers(int groupId)
        {
            var group = await db.Groups.GetGroupsByMentor(groupId);
            var users = await db.Users.GetUsersByGroup(groupId);
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

        public async ThreadTask.Task<IEnumerable<UserWithImageDto>> GetUsersWithImage(int groupId)
        {
            var group = await db.Groups.GetGroupsByMentor(groupId);
            var users = await db.Users.GetUsersByGroup(groupId);
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
                User userToGetImage = await db.Users.Get(user.Id);
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

        public async ThreadTask.Task<IEnumerable<GroupDto>> GetGroupsByMentor(int mentorId)
        {
            var groups = await db.Groups.GetGroupsByMentor(mentorId);
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
                                         await db.Users.ExtractFullName(group.Mentor_Id)));
            }
            return groupList;
        }

        public async ThreadTask.Task<IEnumerable<GroupDto>> GetUserGroups(int userId)
        {
            User user = await db.Users.Get(userId);
            if (user == null)
            {
                return null;
            }
            IEnumerable<Group> groups;
            if (user.Role.Name == "Mentor")
            { 
                groups = await db.Groups.GetGroupsByMentor(userId);
            }
            else if (user.Role.Name == "Student")
            {
                groups =  await db.Groups.GetStudentGroups(userId);
            }
            else
            {
                groups =  db.Groups.GetAll();
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
                                         await db.Users.ExtractFullName(group.Mentor_Id)));
            }
            if (groupList.Count < 1)
            {
                return null;
            }
            return groupList;
        }

        public async ThreadTask.Task<bool> AddUsersToGroup(int[] usersId, int groupId)
        {
            Group groups = await db.Groups.Get(groupId);
            if (groups == null)
            {
                return false;
            }
            var added = false;
            foreach (var userId in usersId)
            {
                User addUser = await db.Users.Get(userId);
                if (addUser != null)
                {
                    added = await db.Groups.AddUserToGroup(userId, groupId);
                    if(added)
                    {
                        await SetUserTasksByAddingUser(userId, groupId);
                    }
                    db.Save();
                }
            }
            return added;
        }

        public async ThreadTask.Task<bool> AddPlansToGroup(int[] plansId, int groupId)
        {
            Group groups = await db.Groups.Get(groupId);
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
                    added = await db.Groups.AddPlanToGroup(planId, groupId);
                    if(added)
                    {
                      await  SetUserTasksByAddingPlan(planId, groupId);
                    }
                    db.Save();
                }
            }
            return added;
        }

        public async ThreadTask.Task<IEnumerable<UserIdentityDto>> GetUsersNotInGroup(int groupId)
        {
            Group group = await db.Groups.Get(groupId);
            if (group == null)
            {
                return null;
            }
            var usersNotInGroup = await db.Users.GetUsersNotInGroup(groupId);
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

        public async ThreadTask.Task<IEnumerable<UserIdentityDto>> SearchUserNotInGroup(string[] searchCases, int groupId)
        {
            IEnumerable<UserIdentityDto> usersNotInGroup = await GetUsersNotInGroup(groupId);
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

        public async ThreadTask.Task<IEnumerable<PlanDto>> GetPlansNotUsedInGroup(int groupId)
        {
            Group group = await db.Groups.Get(groupId);
            if (group == null)
            {
                return null;
            }
            var plansNotUsedInGroup = await db.Plans.GetPlansNotUsedInGroup(groupId);
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
                    plan.Mod_Date);

                if (!plansNotUsedInGroupList.Contains(planDto))
                {
                    plansNotUsedInGroupList.Add(planDto);
                }
            }
            return plansNotUsedInGroupList;
        }
        public async ThreadTask.Task<IEnumerable<PlanDto>> SearchPlansNotUsedInGroup(string[] searchCases, int groupId)
        {
            var plansNotInGroup = await GetPlansNotUsedInGroup(groupId);
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

        private async ThreadTask.Task RemoveMessagesForUserTask(int userTaskId)
        {
            var messages = await db.Messages.GetByUserTaskId(userTaskId);
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

        private async ThreadTask.Task DeleteUserTasksOnRemovingUser(int groupId, int userId)
        {
            Group group = await db.Groups.Get(groupId);
            var user = await db.Users.Get(userId);
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
                    UserTask userTask = await db.UserTasks.GetByPlanTaskForUser(planTask.Id, user.Id);
                    if (userTask == null)
                    {
                        continue;
                    }
                    await RemoveMessagesForUserTask(userTask.Id);
                    db.UserTasks.Remove(userTask);
                }
            }


        }

        public async ThreadTask.Task<bool> RemoveUserFromGroup(int groupId, int userIdToRemove)
        {
            var group = await db.Groups.Get(groupId);
            User userToRemove = await db.Users.Get(userIdToRemove);
            if (group == null)
            {
                return false;
            }
            if (userToRemove == null)
            {
                return false;
            }
            await DeleteUserTasksOnRemovingUser(groupId, userIdToRemove);
            group.Users.Remove(userToRemove);
            db.Save();
            return true;
        }

        private async ThreadTask.Task DeleteUserTasksOnRemovingPlan(int groupId, int planId)
        {
            Group group = await db.Groups.Get(groupId);
            var plan = await db.Plans.Get(planId);
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

                    UserTask userTask = await db.UserTasks.GetByPlanTaskForUser(planTask.Id, user.Id);
                    if (userTask == null)
                    {
                        continue;
                    }
                    await RemoveMessagesForUserTask(userTask.Id);
                    db.UserTasks.Remove(userTask);
                }
            }
        }

        public async ThreadTask.Task<bool> RemovePlanFromGroup(int groupId, int planIdToRemove)
        {
            var group = await db.Groups.Get(groupId);
            var planToRemove = await db.Plans.Get(planIdToRemove);
            if (group == null)
            {
                return false;
            }
            if (planToRemove == null)
            {
                return false;
            }
            await DeleteUserTasksOnRemovingPlan(groupId, planIdToRemove);
            group.Plans.Remove(planToRemove);
            db.Save();
            return true;
        }
    }
}
