using System.Linq;
using LearnWithMentorDAL.Entities;
using LearnWithMentorDAL.Repositories.Interfaces;

namespace LearnWithMentorDAL.Repositories
{
    public class SectionRepository :BaseRepository<Section>, ISectionRepository
    {
        public SectionRepository(LearnWithMentor_DBEntities context) : base(context) { }

        public Section Get(int id)
        {
            return Context.Sections.FirstOrDefault(t => t.Id == id);
        }
    }
}