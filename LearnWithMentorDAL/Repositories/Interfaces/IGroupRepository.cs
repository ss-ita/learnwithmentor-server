using System.Collections.Generic;
using System.Threading.Tasks;
using LearnWithMentorDAL.Entities;
using Task = System.Threading.Tasks.Task;

namespace LearnWithMentorDAL.Repositories.Interfaces
{
    public interface IGroupRepository: IRepository<Group>
    {
        Task<Group> Get(int id);
        Task<int> Count();
        Task<bool> GroupNameExists(string groupName);
        Task<IEnumerable<Group>> GetGroupsByMentor(int mentorId);
        Task<bool> AddUserToGroup(int userId, int groupId);
        Task<bool> AddPlanToGroup(int planId, int groupId);
        Task RemoveUserFromGroup(int groupId, int userId);
        Task RemovePlanFromGroup(int groupId, int planId);
        Task<IEnumerable<Group>> GetStudentGroups(int studentId);
        Task<IEnumerable<Group>> GetGroupsByPlan(int planId);
    }
}
