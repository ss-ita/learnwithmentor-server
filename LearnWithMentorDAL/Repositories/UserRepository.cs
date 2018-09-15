using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.Entity;
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
            Task<User> findUser = Context.Users.FirstOrDefaultAsync(user => user.Id == id);
            return findUser.GetAwaiter().GetResult();
        }

        public User GetByEmail(string email)
        {
            Task<User> findUser = Context.Users.FirstOrDefaultAsync(user => user.Email == email);
            return findUser.GetAwaiter().GetResult();
        }

        public IEnumerable<User> GetUsersByGroup(int groupId)
        {
            Task<Group> findGroup = Context.Groups.FirstOrDefaultAsync(group => group.Id == groupId);
            return findGroup.GetAwaiter().GetResult()?.Users;
        }

        public IEnumerable<User> Search(string[] searchString, int? roleId)
        {
            List<User> result = new List<User>();
            IQueryable<User> usersWithCriteria;
            string firstWord = searchString.Length >= 1 ? searchString[0] : "";
            string secondWord = searchString.Length == 2 ? searchString[1] : "";
            if (roleId == null)
            {
                usersWithCriteria = Context.Users;
            }
            else if (roleId == -1)
            {
                usersWithCriteria = Context.Users.Where(user => user.Blocked);
            }
            else
            {
                usersWithCriteria = Context.Users.Where(user => user.Role_Id == roleId);
            }
            usersWithCriteria = usersWithCriteria.Where(user =>
                (user.FirstName.Contains(firstWord) && user.LastName.Contains(secondWord))
                || (user.FirstName.Contains(secondWord) && user.LastName.Contains(firstWord)));

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
            Task<User> findUser = Context.Users.FirstOrDefaultAsync(user => user.Id == userId);
            return findUser.GetAwaiter().GetResult()?.Image;
        }

        public bool ContainsId(int id)
        {
            Task<bool> checkIdExistion = Context.Users.AnyAsync(user => user.Id == id);
            return checkIdExistion.GetAwaiter().GetResult();
        }

        public IEnumerable<User> GetUsersByRole(int roleId)
        {
            return Context.Users.Where(user => user.Role_Id == roleId);
        }

        public IEnumerable<User> GetUsersByState(bool state)
        {
            return Context.Users.Where(user => user.Blocked == state);
        }

        public string ExtractFullName(int? id)
        {
            if (id == null)
            {
                return null;
            }

            Task<User> findUser = Context.Users.FirstOrDefaultAsync(user => user.Id == id.Value);
            string fullName = null;
            if (findUser.GetAwaiter().GetResult() != null)
            {
                fullName = string.Concat(findUser.Result.FirstName, " ", findUser.Result.LastName);
            }

            return fullName;
        }

        public IEnumerable<User> GetUsersNotInGroup(int groupId)
        {
            return Context.Users.Where(user => !user.Groups.Select(group => group.Id).Contains(groupId))
                .Where(user => !user.Blocked && user.Roles.Name == "Student");
        }
    }
}
