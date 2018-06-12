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
        public void RemoveById(int id)
        {
            var item = context.Users.Where(u => u.Id == id);
            if (item.Any())
            {
                context.Users.RemoveRange(item);
            }
        }
        public bool UpdateById(int id, UserDTO user)
        {
            bool modified = false;
            var item = context.Users.Where(u => u.Id == id);
            if (item.Any())
            {
                User toUpdate = item.First();
                if (user.FirstName != null)
                {
                    toUpdate.FirstName = user.FirstName;
                    modified = true;
                }
                if (user.LastName != null)
                {
                    toUpdate.LastName = user.LastName;
                    modified = true;
                }
                var updatedRole = context.Roles.Where(r => r.Name == user.Role);
                if (updatedRole.Any())
                {
                    toUpdate.Role_Id = updatedRole.First().Id;
                    modified = true;
                }
                Update(toUpdate);
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
                toAdd.Role_Id = context.Roles.Where(r => r.Name == userDTO.Role).Any() ?
                    context.Roles.Where(r => r.Name == userDTO.Role).First().Id : context.Roles.Where(r => r.Name == "Student").First().Id;
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
                var found = roleId == null ? context.Users.Where(u => u.FirstName.Contains(s) || u.LastName.Contains(s)) :
                    context.Users.Where(u => u.Role_Id == roleId).Where(u => u.FirstName.Contains(s) || u.LastName.Contains(s));
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
