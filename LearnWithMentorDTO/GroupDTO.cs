using System.ComponentModel.DataAnnotations;

namespace LearnWithMentorDTO
{
    public class GroupDto
    {
        public int Id { get; set; }
        [Required]
        [StringLength(50, ErrorMessage = "Group name too long")]
        public string Name { get; set; }
        public int? MentorId { get; set; }
        public string MentorName { get; set; }

        public GroupDto() { }

        public GroupDto(int id, string name, int? mentorId, string mentorName)
        {
            Id = id;
            Name = name;
            MentorId = mentorId;
            MentorName = mentorName;
        }

    }
}
