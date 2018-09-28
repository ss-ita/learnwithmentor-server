using System.ComponentModel.DataAnnotations;
using LearnWithMentorDTO.Infrastructure;

namespace LearnWithMentorDTO
{
    public class UserRegistrationDto
    {
        public UserRegistrationDto(string email, string lastName, string firstName, string password)
        {
            Email = email;
            LastName = lastName;
            FirstName = firstName;
            Password = password;
        }

        [Required]
        [RegularExpression(ValidationRules.EMAIL_REGEX,
        ErrorMessage = "Email not valid")]
        public string Email { set; get; }
        [Required]
        [StringLength(ValidationRules.MAX_LENGTH_NAME,
            ErrorMessage = "LastName too long")]
        [RegularExpression(ValidationRules.ONLY_LETTERS_AND_NUMBERS,
            ErrorMessage = "LastName not valid")]
        public string LastName { set; get; }
        [Required]
        [StringLength(ValidationRules.MAX_LENGTH_NAME,
            ErrorMessage = "FirstName too long")]
        [RegularExpression(ValidationRules.ONLY_LETTERS_AND_NUMBERS,
            ErrorMessage = "FirstName not valid")]
        public string FirstName { set; get; }
        [Required]
        public string Password { get; set; }
    }
}
