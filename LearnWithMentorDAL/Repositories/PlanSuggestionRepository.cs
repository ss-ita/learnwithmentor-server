using LearnWithMentorDAL.Entities;
using LearnWithMentorDAL.Repositories.Interfaces;

namespace LearnWithMentorDAL.Repositories
{
    public class PlanSuggestionRepository:BaseRepository<PlanSuggestion>, IPlanSuggestionRepository
    {
        public PlanSuggestionRepository(LearnWithMentor_DBEntities context) : base(context)
        {
        }
    }
}