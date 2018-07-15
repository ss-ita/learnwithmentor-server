using System.Collections.Generic;

namespace LearnWithMentorDTO
{
    public class GroupUsersTaskState
    {
        public int UserId { get; set; }
        public List<UserTaskDTO> Tasks { get; set; }
    }
}