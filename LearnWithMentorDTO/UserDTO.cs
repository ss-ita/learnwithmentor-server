using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnWithMentorDTO
{
    public class UserDTO
    {
        public UserDTO(int id, string firstName, string lastName, string email, string role)
        {
            LastName = lastName;
            FirstName = firstName;
            Id = id;
            Email = email;
            Role = role;
        }
        public string LastName { set; get; }
        public string FirstName { set; get; }
        public int Id { set; get; }
        public string Email { set; get; }
        public string Role { set; get; }
    }
}
