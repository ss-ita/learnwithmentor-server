using System.Collections.Generic;

namespace LearnWithMentorDTO
{
    public class GroupUsersTaskState
    {
        public int UserId { get; set; }
        public List<UserTaskStateDTO> Tasks { get; set; }
    }
}