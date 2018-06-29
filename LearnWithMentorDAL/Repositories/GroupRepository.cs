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

        public IEnumerable<Group> GetGroupsByMentor(int mentor_id)
        {
            return context.Groups.Where(group => group.Mentor_Id == mentor_id);
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

        public void DeleteUserFromGroup(int groupId, int userId)
        {
            var group = Get(groupId);
            var userToDelete = context.Users.FirstOrDefault(user => user.Id == userId);
            group.Users.Remove(userToDelete);
        }

        public void DeletePlanFromGroup(int groupId, int planId)
        {
            var group = Get(groupId);
            var planToDelete = context.Plans.FirstOrDefault(plan => plan.Id == planId);
            group.Plans.Remove(planToDelete);
        }
    }
}
