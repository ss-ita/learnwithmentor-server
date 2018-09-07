using System.Linq;
using System.Data.Entity;
using System.Threading.Tasks;
using LearnWithMentorDAL.Entities;
using LearnWithMentorDAL.Repositories.Interfaces;

namespace LearnWithMentorDAL.Repositories
{
    public class SectionRepository :BaseRepository<Section>, ISectionRepository
    {
        public SectionRepository(LearnWithMentor_DBEntities context) : base(context) { }

        public Section Get(int id)
        {
            Task<Section> findSection = Context.Sections.FirstOrDefaultAsync(t => t.Id == id);
            return findSection.GetAwaiter().GetResult();
        }
    }
}
