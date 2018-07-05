using System.ComponentModel.DataAnnotations;


namespace LearnWithMentorDTO
{
    public class GroupDTO
    {
        public int Id { get; set; }
        [Required]
        [StringLength(50, ErrorMessage = "Group name too long")]
        public string Name { get; set; }
        public int? MentorId { get; set; }

        public GroupDTO(int id, string name, int? mentorId)
        {
            this.Id = id;
            this.Name = name;
            this.MentorId = mentorId;
        }

    }
}
