using System;
using System.ComponentModel.DataAnnotations;

namespace LearnWithMentorDTO
{
    public class ForgotPasswordDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
