using System;
using System.ComponentModel.DataAnnotations;

namespace LearnWithMentorDTO
{
    //todo
    public class UserTaskDTO
    {
        public UserTaskDTO(int id,
                int userId,
                int planId,
                int taskId,
                string state,
                DateTime? endDate,
                string result,
                DateTime? proposeEndDate,
                int mentorId)
        {
            Id = id;
            UserId = userId;
            PlanId = planId;
            TaskId = taskId;
            State = state;
            EndDate= endDate;
            Result = result;
            ProposeEndDate= proposeEndDate;
            MentorId = mentorId;
        }

        public int Id { get; set; }
        public int UserId { get; set; }
        public int PlanId { get; set; }
        public int TaskId { get; set; }
        public string State { get; set; }
        public DateTime? EndDate { get; set; }
        public string Result { get; set; }
        public DateTime? ProposeEndDate { get; set; }
        public int MentorId { get; set; }
    }
}
