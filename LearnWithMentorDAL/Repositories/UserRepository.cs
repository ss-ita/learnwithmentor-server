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
        public void UpdateById(int id, UserDTO user)
        {
            var item = context.Users.Where(u => u.Id == id);
            if (item.Any())
            {
                User toUpdate = item.First();
                toUpdate.FirstName = user.FirstName;
                toUpdate.LastName = user.LastName;
                if (context.Roles.Any(r => r.Name == user.Role))
                {
                    toUpdate.Role_Id = context.Roles.FirstOrDefault(r => r.Name == user.Role).Id;
                }
                Update(toUpdate);
            }
        }
        public void Add(UserDTO userDTO, string password)
        {
            User toAdd = new User();
            toAdd.Email = userDTO.Email;
            //add hashing
            toAdd.Password = password;
            toAdd.Role_Id = context.Roles.Where(r => r.Name == userDTO.Role) != null ?
                context.Roles.Where(r => r.Name == userDTO.Role).First().Id : context.Roles.Where(r => r.Name == "Student").FirstOrDefault().Id;
            toAdd.FirstName = userDTO.FirstName;
            toAdd.LastName = userDTO.LastName;
            context.Users.Add(toAdd);
        }
        public IEnumerable<User> Search(string[] str, int? role_id)
        {
            List<User> ret = new List<User>();
            foreach (var s in str)
            {
                var found = role_id == null ? context.Users.Where(u => u.FirstName.Contains(s) || u.LastName.Contains(s)) :
                    context.Users.Where(u => u.Role_Id == role_id).Where(u => u.FirstName.Contains(s) || u.LastName.Contains(s));
                foreach (var f in found)
                {
                    if (!ret.Contains(f))
                    {
                        ret.Add(f);
                    }
                }
            }
            return ret;
        }
    }
}
