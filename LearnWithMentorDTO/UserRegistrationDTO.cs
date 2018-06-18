using System.ComponentModel.DataAnnotations;

namespace LearnWithMentorDTO
{
    public class UserRegistrationDTO
    {
        public UserRegistrationDTO(string email, string lastName, string firstName, string password, string role)
        {
            Email = email;
            LastName = lastName;
            FirstName = firstName;
            Password = password;
            Role = role;
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

        [Required]
        public string Role { set; get; }
    }
}
