using System.ComponentModel.DataAnnotations;

namespace LearnWithMentorDTO
{
    public class UserDTO
    {
        public UserDTO(int id, string firstName, string lastName, string role, bool blocked)
        {
            LastName = lastName;
            FirstName = firstName;
            Id = id;
            Role = role;
            Blocked = blocked;
        }

        public string LastName { set; get; }

        public string FirstName { set; get; }

        public int Id { set; get; }

        public string Role { set; get; }

        public bool? Blocked { set; get; }
    }
}
