using System;
using System.ComponentModel.DataAnnotations;
using LearnWithMentorDTO.Infrastructure;

namespace LearnWithMentorDTO
{
    public class UserTaskDTO
    {
        public UserTaskDTO() { }
        public UserTaskDTO(int id,
                int userId,
                int planTaskId,
                DateTime? endDate,
                DateTime? proposeEndDate,
                int mentorId,
                string state = "P",
                string result = "")
        {
            Id = id;
            UserId = userId;
            PlanTaskId = planTaskId;
            State = state;
            EndDate = endDate;
            Result = result;
            ProposeEndDate = proposeEndDate;
            MentorId = mentorId;
        }

        public int Id { get; set; }
        public int UserId { get; set; }
        [Required]
        public int PlanTaskId { get; set; }
        [Required]
        [RegularExpression(ValidationRules.USERTASK_STATE,
            ErrorMessage = "State could be only ['P', 'D', 'A', 'R'] letters")]
        public string State { get; set; }
        public DateTime? EndDate { get; set; }
        [Required]
        [StringLength(ValidationRules.MAX_USERTASK_RESULT_LENGTH,
            ErrorMessage = "Result too long")]
        public string Result { get; set; }
        public DateTime? ProposeEndDate { get; set; }
        public int MentorId { get; set; }
    }
}
