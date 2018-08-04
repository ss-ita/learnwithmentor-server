using System.Collections.Generic;

namespace LearnWithMentorDTO
{
    public class ContentDTO
    {
        public List<TaskDTO> Tasks { get; set; }
        public List<ListUserTasksDTO> UserTasks { get; set; }
    }
}
