using System.Linq;
using LearnWithMentorDAL.Entities;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Collections.Generic;
using LearnWithMentorDAL.Repositories.Interfaces;
using ThreadTask = System.Threading.Tasks;

namespace LearnWithMentorDAL.Repositories
{
    public class GroupRepository : BaseRepository<Group>, IGroupRepository
    {
        public GroupRepository(LearnWithMentorContext context) : base(context)
        {
        }

        public async Task<Group> Get(int id)
        {
            return await Context.Groups.FirstOrDefaultAsync(group => group.Id == id);
        }

        public async Task<bool> GroupNameExists(string groupName)
        {
            return await Context.Groups.AnyAsync(g => g.Name.Equals(groupName));
        }

        public async Task<int> Count()
        {
            return await Context.Groups.CountAsync();
        }

        public async Task<IEnumerable<Group>> GetGroupsByMentor(int mentorId)
        {
            return await Context.Groups.Where(group => group.Mentor_Id == mentorId).ToListAsync();
        }

        public async Task<IEnumerable<Group>> GetStudentGroups(int studentId)
        {
            User findStudent = await Context.Users.FirstOrDefaultAsync(u => u.Id == studentId);
            return findStudent?.Groups;
        }

        public async Task<IEnumerable<Group>> GetGroupsByPlan(int planId)
        {
            return await Context.Groups.Where(g => g.Plans.Any(p => p.Id == planId)).ToListAsync();
        }

        public async Task<bool> AddPlanToGroup(int planId, int groupId)
        {
            Plan findPlan = await Context.Plans.FirstOrDefaultAsync(plan => plan.Id == planId);
            Group findGroup = await Context.Groups.FirstOrDefaultAsync(group => group.Id == groupId);
             findGroup?.Plans.Add(findPlan);
            return true;
        }

        public async Task<bool> AddUserToGroup(int userId, int groupId)
        {
            User findUser = await Context.Users.FirstOrDefaultAsync(user => user.Id == userId);
            Group findGroup = await Context.Groups.FirstOrDefaultAsync(group => group.Id == groupId);
            findGroup?.Users.Add(findUser);
            return true;
        }

        public async ThreadTask.Task RemoveUserFromGroup(int groupId, int userId)
        {
            Group group = await Get(groupId); 
            User findUser = await Context.Users.FirstOrDefaultAsync(user => user.Id == userId);
            group.Users.Remove(findUser);
        }

        public async ThreadTask.Task RemovePlanFromGroup(int groupId, int planId)
        {
            Group group = await Get(groupId);
            Plan findPlan = await Context.Plans.FirstOrDefaultAsync(plan => plan.Id == planId);
            group.Plans.Remove(findPlan);
        }
    }
}
