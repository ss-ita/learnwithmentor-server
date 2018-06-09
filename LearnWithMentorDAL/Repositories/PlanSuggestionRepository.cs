using LearnWithMentorDAL.Entities;

namespace LearnWithMentorDAL.Repositories
{
    public class PlanSuggestionRepository:BaseRepository<PlanSuggestion>, IPlanSuggestionRepository
    {
        public PlanSuggestionRepository(LearnWithMentor_DBEntities context) : base(context)
        {
        }
    }
}