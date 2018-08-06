using System.ComponentModel.DataAnnotations;

namespace LearnWithMentorDTO
{
    public class EmailDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
