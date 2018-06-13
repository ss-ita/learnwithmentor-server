using System.ComponentModel.DataAnnotations;

namespace LearnWithMentorDTO
{
    public class UserDTO
    {
        public UserDTO(int id, string firstName, string lastName, string email, string role, bool blocked)
        {
            LastName = lastName;
            FirstName = firstName;
            Id = id;
            Email = email;
            Role = role;
            Blocked = blocked;
        }
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

        public int Id { set; get; }

        [Required]
        [RegularExpression(ValidationRules.EMAIL_REGEX,
            ErrorMessage = "Email not valid")]
        public string Email { set; get; }

        [Required]
        public string Role { set; get; }
        public bool Blocked { set; get; }
    }
}
