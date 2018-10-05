using System.ComponentModel.DataAnnotations;

namespace LearnWithMentorDTO
{
    public class EmailDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
