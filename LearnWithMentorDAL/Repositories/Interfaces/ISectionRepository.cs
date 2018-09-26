using LearnWithMentorDAL.Entities;
using System.Threading.Tasks;

namespace LearnWithMentorDAL.Repositories.Interfaces
{
    public interface ISectionRepository: IRepository<Section>
    {
        Task<Section> Get(int id);
    }
}