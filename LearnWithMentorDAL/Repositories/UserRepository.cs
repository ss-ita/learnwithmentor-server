using System.Collections.Generic;
using System.Linq;
using LearnWithMentorDAL.Entities;

namespace LearnWithMentorDAL.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(LearnWithMentor_DBEntities context) : base(context)
        {
        }

        public User Get(int id)
        {
            return context.Users.FirstOrDefault(u => u.Id == id);
        }
        public User GetByEmail(string email)
        {
            return context.Users.FirstOrDefault(u => u.Email == email);
        }
        public IEnumerable<User> GetUsersByGroup(int groupId)
        {
            return context.Groups.FirstOrDefault(g => g.Id == groupId)?.Users;
        }

        public IEnumerable<User> Search(string[] str, int? roleId)
        {
            List<User> result = new List<User>();
            IQueryable<User> usersWithCriteria;
            string firstWord = str.Length == 1 ? str[0] : "";
            string secondWord = str.Length == 2 ? str[1] : "";
            if (roleId == null)
            {
                usersWithCriteria = context.Users;
            }
            else if (roleId == -1)
            {
                usersWithCriteria = context.Users.Where(u => u.Blocked);
            }
            else
            {
                usersWithCriteria = context.Users.Where(u => u.Role_Id == roleId);
            }
            usersWithCriteria = usersWithCriteria.Where(u =>
                (u.FirstName.Contains(firstWord) && u.LastName.Contains(secondWord))
                || (u.FirstName.Contains(secondWord) && u.LastName.Contains(firstWord)));
            foreach (var user in usersWithCriteria)
            {
                if (!result.Contains(user))
                {
                    result.Add(user);
                }
            }
            return result;
        }

        public string GetImageBase64(int userId)
        {
            return context.Users.FirstOrDefault(u => u.Id == userId)?.Image;
        }

        public bool ContainsId(int id)
        {
            return context.Users.Any(u => u.Id == id);
        }

        public IEnumerable<User> GetUsersByRole(int role_id)
        {
            return context.Users.Where(u => u.Role_Id == role_id);
        }

        public IEnumerable<User> GetUsersByState(bool state)
        {
            return context.Users.Where(u => u.Blocked == state);
        }

        public string ExtractFullName(int? id)
        {
            if (id == null)
                return null;
            User currentUser = context.Users.FirstOrDefault(u => u.Id == id.Value);
            string fullName = null;
            if (currentUser != null)
                fullName = string.Concat(currentUser.FirstName, " ", currentUser.LastName);
            return fullName;
        }

        public IEnumerable<User> GetUsersNotInGroup(int groupId)
        {
            return context.Users.Where(u => !u.Groups.Select(g => g.Id).Contains(groupId));
        }
    }
}
