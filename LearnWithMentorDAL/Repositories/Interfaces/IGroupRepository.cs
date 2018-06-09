using LearnWithMentorDAL.Entities;

namespace LearnWithMentorDAL.Repositories
{
    public interface IGroupRepository: IRepository<Group>
    {
        Group Get(int id);
    }
}
