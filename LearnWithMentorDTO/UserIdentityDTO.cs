using System.ComponentModel.DataAnnotations;
using LearnWithMentorDTO.Infrastructure;

namespace LearnWithMentorDTO
{
    public class UserIdentityDTO
    {
        public UserIdentityDTO(string email, string password, int id, string firstName, string lastName, string role, bool blocked, bool emailConfirmed)
        {
            Email = email;
            Password = password;
            LastName = lastName;
            FirstName = firstName;
            Id = id;
            Role = role;
            Blocked = blocked;
            EmailConfirmed = emailConfirmed;
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
        public int Id { set; get; }
        public string Role { set; get; }
        public bool? Blocked { set; get; }
        [Required]
        public string Password { get; set; }
        public bool EmailConfirmed { set; get; }
    }
}