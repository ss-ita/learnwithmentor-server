using System;

namespace LearnWithMentorDTO
{
    public class UserTaskStateDTO
    {
        public UserTaskStateDTO(int planTaskId, string state)
        {
            Id = planTaskId;
            State = state;
        }

        public int Id { get; set; }
        public string State { set; get; }
    }
}
