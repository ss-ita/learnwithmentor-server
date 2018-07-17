using System.Collections.Generic;

namespace LearnWithMentorDTO
{
    public class SectionDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<TaskDTO> Tasks { get; set; }
        public List<UserTaskDTO> UserTasks { get; set; }
    }
}