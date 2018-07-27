using System.ComponentModel.DataAnnotations;
using LearnWithMentorDTO.Infrastructure;

namespace LearnWithMentorDTO
{
    public class PlanSuggestionDTO
    {
        public PlanSuggestionDTO(int id, int planId, int mentorId, string text)
        {
            Id = id;
            PlanId = planId;
            MentorId = mentorId;
            Text = text;
        }
        public int Id { get; set; }
        public int PlanId { get; set; }
        public int MentorId { get; set; }
        [Required]
        [StringLength(ValidationRules.MAX_COMMENT_TEXT_LENGTH,
            ErrorMessage = "PlanSuggestion text too long")]
        public string Text { get; set; }
    }
}
