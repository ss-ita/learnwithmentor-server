using System.Linq;
using System.Data.Entity;
using System.Threading.Tasks;
using LearnWithMentorDAL.Entities;
using LearnWithMentorDAL.Repositories.Interfaces;

namespace LearnWithMentorDAL.Repositories
{
    public class SectionRepository :BaseRepository<Section>, ISectionRepository
    {
        public SectionRepository(LearnWithMentorContext context) : base(context) { }

        public Task<Section> GetAsync(int id)
        {
            return Context.Sections.FirstOrDefaultAsync(t => t.Id == id);
           
        }
    }
}
