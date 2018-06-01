using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnWithMentorDTO
{
    public class UserDTO
    {
        public UserDTO(int _id, string _firstName, string _lastName, string _email, string _role)
        {
            LastName = _lastName;
            FirstName = _firstName;
            Id = _id;
            Email = _email;
            Role = _role;
        }
        public string LastName { set; get; }
        public string FirstName { set; get; }
        public int Id { set; get; }
        public string Email { set; get; }
        public string Role { set; get; }
    }
}
