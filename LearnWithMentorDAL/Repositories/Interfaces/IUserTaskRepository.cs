using LearnWithMentorDAL.Entities;

namespace LearnWithMentorDAL.Repositories
{
    public interface IUserTaskRepository: IRepository<UserTask>
    {
        UserTask Get(int id);
        UserTask Get(int planTaskId, int userId);
        UserTask GetByPlanTaskForUser(int planTaskId, int userId);
    }
}
