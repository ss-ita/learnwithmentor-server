using System.ComponentModel.DataAnnotations;
using LearnWithMentorDTO.Infrastructure;

namespace LearnWithMentorDTO
{
    public class UserWithImageDto
    {
        public UserWithImageDto(string email, int id, string firstName, string lastName, string role, bool blocked, ImageDto image)
        {
            Email = email;
            LastName = lastName;
            FirstName = firstName;
            Id = id;
            Role = role;
            Blocked = blocked;
            Image = image;
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
        public ImageDto Image { set; get; }
    }
}