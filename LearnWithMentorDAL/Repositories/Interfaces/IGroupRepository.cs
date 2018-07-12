using LearnWithMentorDAL.Entities;
using System.Collections.Generic;

namespace LearnWithMentorDAL.Repositories
{
    public interface IGroupRepository: IRepository<Group>
    {
        Group Get(int id);
        int Count();
        bool GroupNameExists(string groupName);
        IEnumerable<Group> GetGroupsByMentor(int mentor_id);
        bool AddUserToGroup(int userId, int groupId);
        bool AddPlanToGroup(int planId, int groupId);
        void RemoveUserFromGroup(int groupId, int userId);
        void RemovePlanFromGroup(int groupId, int planId);
        IEnumerable<Group> GetStudentGroups(int studentId);
        IEnumerable<Group> GetGroupsByPlan(int planId);
    }
}
