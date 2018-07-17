using System.Linq;
using LearnWithMentorDAL.Entities;
using System.Collections.Generic;
using LearnWithMentorDAL.Repositories.Interfaces;

namespace LearnWithMentorDAL.Repositories
{
    public class GroupRepository : BaseRepository<Group>, IGroupRepository
    {
        public GroupRepository(LearnWithMentor_DBEntities context) : base(context)
        {
        }
        public Group Get(int id)
        {
            return Context.Groups.FirstOrDefault(group => group.Id == id);
        }

        public bool GroupNameExists(string groupName)
        {
            return Context.Groups.Any(g => g.Name.Equals(groupName));
        }

        public int Count()
        {
            return Context.Groups.Count();
        }

        public IEnumerable<Group> GetGroupsByMentor(int mentorId)
        {
            return Context.Groups.Where(group => group.Mentor_Id == mentorId);
        }
        public IEnumerable<Group> GetStudentGroups(int studentId)
        {
            return Context.Users.FirstOrDefault(u => u.Id == studentId)?.Groups;
        }
        public IEnumerable<Group> GetGroupsByPlan(int planId)
        {
            return Context.Groups.Where(g => g.Plans.Any(p => p.Id == planId));
        }

        public bool AddPlanToGroup(int planId, int groupId)
        {
            var planAdd = Context.Plans.FirstOrDefault(plan => plan.Id == planId);
            Context.Groups.FirstOrDefault(group => group.Id == groupId)?.Plans.Add(planAdd);
            return true;
        }
        public bool AddUserToGroup(int userId, int groupId)
        {
            var userAdd = Context.Users.FirstOrDefault(user => user.Id == userId);   
            Context.Groups.FirstOrDefault(group => group.Id == groupId)?.Users.Add(userAdd);
            return true;
        }

        public void RemoveUserFromGroup(int groupId, int userId)
        {
            var group = Get(groupId);
            var userToRemove = Context.Users.FirstOrDefault(user => user.Id == userId);
            group.Users.Remove(userToRemove);
        }

        public void RemovePlanFromGroup(int groupId, int planId)
        {
            var group = Get(groupId);
            var planToRemove = Context.Plans.FirstOrDefault(plan => plan.Id == planId);
            group.Plans.Remove(planToRemove);
        }
    }
}
