using System.ComponentModel.DataAnnotations;
using LearnWithMentorDTO.Infrastructure;

namespace LearnWithMentorDTO
{
    public class UserLoginDto
    {
        public UserLoginDto(string email,  string password)
        {
            Email = email;
            Password = password;
        }

        [Required]
        [RegularExpression(ValidationRules.EMAIL_REGEX,
            ErrorMessage = "Email not valid")]
        public string Email { set; get; }
        [Required]
        public string Password { get; set; }
    }
}