using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnWithMentorDTO
{
    class GroupDTO
    {
        public int ID { get; set; }
        [Required]
        [StringLength(50, ErrorMessage = "Task name too long")]
        public string Name { get; set; }
        public int MentorID { get; set; }

        public GroupDTO(int id, string name, int mentorID)
        {
            this.ID = id;
            this.Name = name;
            this.MentorID = mentorID;
        }

    }
}
