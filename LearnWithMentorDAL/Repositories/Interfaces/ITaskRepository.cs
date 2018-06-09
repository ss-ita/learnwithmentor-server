namespace LearnWithMentorDAL.Repositories
{
    public interface ITaskRepository: IRepository<Entities.Task>
    {
        Entities.Task Get(int id);
    }
}
