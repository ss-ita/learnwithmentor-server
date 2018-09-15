using System.Linq;
using LearnWithMentorDAL.Entities;
using System.Threading.Tasks;
using System.Data.Entity;
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
            Task<Group> findGroup = Context.Groups.FirstOrDefaultAsync(group => group.Id == id);
            return findGroup.GetAwaiter().GetResult();
        }

        public bool GroupNameExists(string groupName)
        {
            Task<bool> checkNameExisting = Context.Groups.AnyAsync(g => g.Name.Equals(groupName));
            return checkNameExisting.GetAwaiter().GetResult();
        }

        public int Count()
        {
            Task<int> countGroups = Context.Groups.CountAsync();
            return countGroups.GetAwaiter().GetResult();
        }

        public IEnumerable<Group> GetGroupsByMentor(int mentorId)
        {
            return Context.Groups.Where(group => group.Mentor_Id == mentorId);
        }

        public IEnumerable<Group> GetStudentGroups(int studentId)
        {
            Task<User> findStudent = Context.Users.FirstOrDefaultAsync(u => u.Id == studentId);
            return findStudent.GetAwaiter().GetResult()?.Groups;
        }

        public IEnumerable<Group> GetGroupsByPlan(int planId)
        {
            return Context.Groups.Where(g => g.Plans.Any(p => p.Id == planId));
        }

        public bool AddPlanToGroup(int planId, int groupId)
        {
            Task<Plan> findPlan = Context.Plans.FirstOrDefaultAsync(plan => plan.Id == planId);
            Task<Group> findGroup = Context.Groups.FirstOrDefaultAsync(group => group.Id == groupId);
            findGroup.GetAwaiter().GetResult()?.Plans.Add(findPlan.GetAwaiter().GetResult());
            return true;
        }

        public bool AddUserToGroup(int userId, int groupId)
        {
            Task<User> findUser = Context.Users.FirstOrDefaultAsync(user => user.Id == userId);
            Task<Group> findGroup = Context.Groups.FirstOrDefaultAsync(group => group.Id == groupId);
            findGroup.GetAwaiter().GetResult()?.Users.Add(findUser.GetAwaiter().GetResult());
            return true;
        }

        public void RemoveUserFromGroup(int groupId, int userId)
        {
            Group group = Get(groupId); 
            Task<User> findUser = Context.Users.FirstOrDefaultAsync(user => user.Id == userId);
            group.Users.Remove(findUser.GetAwaiter().GetResult());
        }

        public void RemovePlanFromGroup(int groupId, int planId)
        {
            Group group = Get(groupId);
            Task <Plan> findPlan = Context.Plans.FirstOrDefaultAsync(plan => plan.Id == planId);
            group.Plans.Remove(findPlan.GetAwaiter().GetResult());
        }
    }
}
