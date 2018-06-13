using System.Collections.Generic;
using System.Linq;
using LearnWithMentorDAL.Entities;
using LearnWithMentorDTO;

namespace LearnWithMentorDAL.Repositories
{
    public class UserRepository: BaseRepository<User>, IUserRepository
    {
        public UserRepository(LearnWithMentor_DBEntities context) : base(context)
        {
        }
        public User Get(int id)
        {
            return context.Users.FirstOrDefault(u => u.Id == id);
        }
        public bool BlockById(int id)
        {
            var item = Get(id);
            if (item != null)
            {
                item.Blocked = true;
                Update(item);
                return true;
            }
            return false;
        }
        public bool UpdateById(int id, UserDTO user)
        {
            bool modified = false;
            var item = Get(id);
            if (item != null)
            {
                if (user.FirstName != null)
                {
                    item.FirstName = user.FirstName;
                    modified = true;
                }
                if (user.LastName != null)
                {
                    item.LastName = user.LastName;
                    modified = true;
                }
                var updatedRole = context.Roles.FirstOrDefault(r => r.Name == user.Role);
                if (updatedRole != null)
                {
                    item.Role_Id = updatedRole.Id;
                    modified = true;
                }
                Update(item);
            }
            return modified;
        }
        public bool Add(UserDTO userDTO, string password)
        {
            if (userDTO.Email == null || userDTO.FirstName == null || userDTO.LastName == null || password == null)
            {
                return false;
            }
            else
            {
                User toAdd = new User();
                toAdd.Email = userDTO.Email;
                //add hashing
                toAdd.Password = password;
                toAdd.Role_Id = context.Roles.FirstOrDefault(r => r.Name == userDTO.Role) != null ?
                    context.Roles.First(r => r.Name == userDTO.Role).Id : context.Roles.First(r => r.Name == "Student").Id;
                toAdd.FirstName = userDTO.FirstName;
                toAdd.LastName = userDTO.LastName;
                context.Users.Add(toAdd);
                return true;
            }
        }
        public IEnumerable<User> Search(string[] str, int? roleId)
        {
            List<User> result = new List<User>();
            foreach (var s in str)
            {
                IQueryable<User> found;
                if (roleId == null)
                {
                    found = context.Users.Where(u => u.FirstName.Contains(s) || u.LastName.Contains(s));
                }
                else if (roleId == -1)
                {
                    found = context.Users.Where(u => (u.FirstName.Contains(s) || u.LastName.Contains(s)) && (u.Blocked));
                }
                else
                {
                    found = context.Users.Where(u => u.Role_Id == roleId).Where(u => u.FirstName.Contains(s) || u.LastName.Contains(s));
                }
                foreach (var f in found)
                {
                    if (!result.Contains(f))
                    {
                        result.Add(f);
                    }
                }
            }
            return result;
        }

        public string ExtractFullName(int? id)
        {
            if (id == null)
                return null;
            User currentUser = context.Users.FirstOrDefault(u => u.Id == id);
            string fullName = null;
            if (currentUser!=null)
                fullName=string.Concat(currentUser.FirstName, " ", currentUser.LastName);
            return fullName;
        }
    }
}
