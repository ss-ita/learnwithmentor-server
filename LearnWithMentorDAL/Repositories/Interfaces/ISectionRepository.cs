using LearnWithMentorDAL.Entities;

namespace LearnWithMentorDAL.Repositories.Interfaces
{
    public interface ISectionRepository: IRepository<Section>
    {
        Section Get(int id);
    }
}