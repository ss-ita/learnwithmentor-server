using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnWithMentorDTO
{
    public class SectionUserTaskDTO
    {
        public int Id { get; set; }
        public List<UserTaskDTO> UserTasks { get; set; }
    }
}
