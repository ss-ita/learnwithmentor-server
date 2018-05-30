using System.Linq;
using LearnWithMentorDAL.Entities;

namespace LearnWithMentorDAL.Repositories
{
    public class SectionRepository :BaseRepository<Section>, ISectionRepository
    {
        public SectionRepository(LearnWithMentor_DBEntities _context) : base(_context) { }

        public Section Get(int id)
        {
            return context.Sections.Where(t => t.Id == id).FirstOrDefault();
        }

    }
}