using System.Linq;
using LearnWithMentorDAL.Entities;
using System.Collections.Generic;

namespace LearnWithMentorDAL.Repositories
{
    public class GroupRepository : BaseRepository<Group>, IGroupRepository
    {
        public GroupRepository(LearnWithMentor_DBEntities context) : base(context)
        {
        }
        public Group Get(int id)
        {
            return context.Groups.FirstOrDefault(group => group.Id == id);
        }

        public bool GroupNameExists(string groupName)
        {
            return context.Groups.Any(g => g.Name.Equals(groupName));
        }

        public int Count()
        {
            return context.Groups.Count();
        }

        public IEnumerable<Group> GetGroupsByMentor(int mentorId)
        {
            return context.Groups.Where(group => group.Mentor_Id == mentorId);
        }
        public IEnumerable<Group> GetStudentGroups(int studentId)
        {
            return context.Users.FirstOrDefault(u => u.Id == studentId).Groups;
        }

        public bool AddPlanToGroup(int planId, int groupId)
        {
            var planAdd = context.Plans.FirstOrDefault(plan => plan.Id == planId);
            context.Groups.FirstOrDefault(group => group.Id == groupId).Plans.Add(planAdd);
            return true;
        }
        public bool AddUserToGroup(int userId, int groupId)
        {
            var userAdd = context.Users.FirstOrDefault(user => user.Id == userId);
           
            context.Groups.FirstOrDefault(group => group.Id == groupId).Users.Add(userAdd);
            return true;
        }

        public void RemoveUserFromGroup(int groupId, int userId)
        {
            var group = Get(groupId);
            var userToRemove = context.Users.FirstOrDefault(user => user.Id == userId);
            group.Users.Remove(userToRemove);
        }

        public void RemovePlanFromGroup(int groupId, int planId)
        {
            var group = Get(groupId);
            var planToRemove = context.Plans.FirstOrDefault(plan => plan.Id == planId);
            group.Plans.Remove(planToRemove);
        }
    }
}
