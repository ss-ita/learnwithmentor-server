using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnWithMentorDTO
{
    public class RoleDTO
    {
        public RoleDTO(int _id, string _name)
        {
            Id = _id;
            Name = _name;
        }
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
