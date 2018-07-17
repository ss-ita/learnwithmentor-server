using LearnWithMentorDAL.Entities;

namespace LearnWithMentorDAL.Repositories.Interfaces
{
    public interface IUserTaskRepository: IRepository<UserTask>
    {
        UserTask Get(int id);
        UserTask GetByPlanTaskForUser(int planTaskId, int userId);
        int GetNumberOfTasksByState(int userId, string state);
    }
}
