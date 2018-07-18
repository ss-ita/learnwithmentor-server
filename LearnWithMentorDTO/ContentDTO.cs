using System.Collections.Generic;

namespace LearnWithMentorDTO
{
    public class ContentDTO
    {
        public List<TaskDTO> Tasks { get; set; }
        public List<UserTaskDTO> UserTasks { get; set; }
    }
}
