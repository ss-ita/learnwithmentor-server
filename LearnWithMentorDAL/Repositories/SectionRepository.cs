using System.Linq;
using LearnWithMentorDAL.Entities;

namespace LearnWithMentorDAL.Repositories
{
    public class SectionRepository :BaseRepository<Section>, ISectionRepository
    {
        public SectionRepository(LearnWithMentor_DBEntities context) : base(context) { }

        public Section Get(int id)
        {
            return context.Sections.FirstOrDefault(t => t.Id == id);
        }

    }
}