using System.Collections.Generic;
using System.Linq;
using LearnWithMentorDAL.Entities;
using LearnWithMentorDAL.Repositories.Interfaces;

namespace LearnWithMentorDAL.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(LearnWithMentor_DBEntities context) : base(context)
        {
        }

        public User Get(int id)
        {
            return Context.Users.FirstOrDefault(u => u.Id == id);
        }

        public User GetByEmail(string email)
        {
            return Context.Users.FirstOrDefault(u => u.Email == email);
        }

        public IEnumerable<User> GetUsersByGroup(int groupId)
        {
            return Context.Groups.FirstOrDefault(g => g.Id == groupId)?.Users;
        }

        public IEnumerable<User> Search(string[] searchString, int? roleId)
        {
            var result = new List<User>();
            IQueryable<User> usersWithCriteria;
            var firstWord = searchString.Length >= 1 ? searchString[0] : "";
            var secondWord = searchString.Length == 2 ? searchString[1] : "";
            if (roleId == null)
            {
                usersWithCriteria = Context.Users;
            }
            else if (roleId == -1)
            {
                usersWithCriteria = Context.Users.Where(u => u.Blocked);
            }
            else
            {
                usersWithCriteria = Context.Users.Where(u => u.Role_Id == roleId);
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
            return Context.Users.FirstOrDefault(u => u.Id == userId)?.Image;
        }

        public bool ContainsId(int id)
        {
            return Context.Users.Any(u => u.Id == id);
        }
        
        public IEnumerable<User> GetUsersByRole(int roleId)
        {
            return Context.Users.Where(u => u.Role_Id == roleId);
        }

        public IEnumerable<User> GetUsersByState(bool state)
        {
            return Context.Users.Where(u => u.Blocked == state);
        }

        public string ExtractFullName(int? id)
        {
            if (id == null)
                return null;
            var currentUser = Context.Users.FirstOrDefault(u => u.Id == id.Value);
            string fullName = null;
            if (currentUser != null)
                fullName = string.Concat(currentUser.FirstName, " ", currentUser.LastName);
            return fullName;
        }

        public IEnumerable<User> GetUsersNotInGroup(int groupId)
        {
            return Context.Users.Where(u => !u.Groups.Select(g => g.Id).Contains(groupId))
                .Where(u => !u.Blocked && u.Roles.Name == "Student");
        }
    }
}
