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
        public UserRepository(LearnWithMentorContext context) : base(context)
        {
        }

        public Task<User> GetAsync(int id)
        {
         return  Context.Users.FirstOrDefaultAsync(user => user.Id == id);
        }

        public Task<User> GetByEmailAsync(string email)
        {
            return Context.Users.FirstOrDefaultAsync(user => user.Email == email);
           
        }

        public async Task<IEnumerable<User>> GetUsersByGroupAsync(int groupId)
        {
            Group findGroup = await Context.Groups.FirstOrDefaultAsync(group => group.Id == groupId);
            return findGroup?.Users;
        }
        
        public async  Task<IEnumerable<User>> SearchAsync(string[] searchString, int? roleId)
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
                usersWithCriteria =  Context.Users.Where(user => user.RoleId == roleId);
            }
           var users = await usersWithCriteria.Where(user =>
                (user.FirstName.Contains(firstWord) && user.LastName.Contains(secondWord))
                || (user.FirstName.Contains(secondWord) && user.LastName.Contains(firstWord))).ToListAsync();

            foreach (var user in users)
            {
                if (!result.Contains(user))
                {
                    result.Add(user);
                }
            }
            return result;
        }

        public async Task<string> GetImageBase64Async(int userId)
        {
            User findUser = await Context.Users.FirstOrDefaultAsync(user => user.Id == userId);
            return findUser?.Image;
        }

        public async Task<bool> ContainsIdAsync(int id)
        {
            return await Context.Users.AnyAsync(user => user.Id == id);
            
        }

        public async Task<IEnumerable<User>> GetUsersByRoleAsync(int roleId)
        {
            return await Context.Users.Where(user => user.RoleId == roleId).ToListAsync();
        }

        public async Task<IEnumerable<User>> GetUsersByStateAsync(bool state)
        {
            return await Context.Users.Where(user => user.Blocked == state).ToListAsync();
        }

        public async Task<string> ExtractFullNameAsync(int? id)
        {
            if (id == null)
            {
                return null;
            }

            User findUser  =  await Context.Users.FirstOrDefaultAsync(user => user.Id == id.Value);
            string fullName = null;
            if (findUser != null)
            {
                fullName = string.Concat(findUser.FirstName, " ", findUser.LastName);
            }

            return fullName;
        }

        public async Task<IEnumerable<User>> GetUsersNotInGroupAsync(int groupId)
        {
            return await Context.Users.Where(user => !user.Groups.Select(group => group.Id).Contains(groupId))
                .Where(user => !user.Blocked && user.Role.Name == "Student").ToListAsync();
        }
    }
}
