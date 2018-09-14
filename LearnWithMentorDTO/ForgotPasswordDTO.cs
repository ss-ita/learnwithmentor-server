using System.ComponentModel.DataAnnotations;

namespace LearnWithMentorDto
{
    public class EmailDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
