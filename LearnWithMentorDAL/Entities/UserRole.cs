using System.ComponentModel.DataAnnotations;

namespace LearnWithMentorDAL.Entities
{
    public class UserRole
    {
        [Key]
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string RolesName { get; set; }
        public string Email { get; set; }
    }
}
