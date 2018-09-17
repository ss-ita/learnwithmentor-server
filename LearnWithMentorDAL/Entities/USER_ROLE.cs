using System.ComponentModel.DataAnnotations;

namespace LearnWithMentorDAL.Entities
{
    public class USER_ROLE
    {
        [Key]
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Roles_Name { get; set; }
        public string Email { get; set; }
    }
}
