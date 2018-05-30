using LearnWithMentorDAL.Entities;

namespace LearnWithMentorDAL.Repositories
{
    public interface ISectionRepository: IRepository<Section>
    {
        Section Get(int id);
    }
}