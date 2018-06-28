using System.ComponentModel.DataAnnotations;


namespace LearnWithMentorDTO
{
    public class GroupDTO
    {
        public int ID { get; set; }
        [Required]
        [StringLength(50, ErrorMessage = "Group name too long")]
        public string Name { get; set; }
        public int? MentorID { get; set; }

        public GroupDTO(int id, string name, int? mentorID)
        {
            this.ID = id;
            this.Name = name;
            this.MentorID = mentorID;
        }

    }
}
