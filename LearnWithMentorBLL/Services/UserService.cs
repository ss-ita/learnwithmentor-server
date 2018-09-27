using System.Collections.Generic;
using LearnWithMentorBLL.Interfaces;
using LearnWithMentorDTO;
using LearnWithMentorDAL.Entities;
using System;
using System.Linq;
using LearnWithMentorDAL.UnitOfWork;
using System.Threading.Tasks;

namespace LearnWithMentorBLL.Services
{
    public class UserService : BaseService, IUserService
    {
        public UserService(IUnitOfWork db) : base(db)
        {
        }

        public async Task<UserDto> Get(int id)
        {
            User user = await db.Users.GetAsync(id);
            if (user == null)
            {
                return null;
            }
            return await UserToUserDTO(user);
        }

        public async Task<UserIdentityDto> GetByEmail(string email)
        {
            User user = await db.Users.GetByEmailAsync(email);
            if (user == null)
            {
                return null;
            }
            return new UserIdentityDto(user.Email, user.Password, user.Id,
                user.FirstName,
                user.LastName,
                user.Role.Name,
                user.Blocked,
                user.Email_Confirmed);
        }

        public async Task<List<UserDto>> GetAllUsers()
        {
            IEnumerable<User> users = db.Users.GetAll();
            if (users == null)
            {
                return null;
            }
            var dtos = new List<UserDto>();
            foreach (var user in users)
            {
                dtos.Add(await UserToUserDTO(user));
            }
            return dtos;
        }

        public  Task<PagedListDto<UserDto>> GetUsers(int pageSize, int pageNumber = 0)
        {
            var query = db.Users.GetAll().AsQueryable();
            query = query.OrderBy(x => x.Id);
            return  PagedList<User, UserDto>.GetDTO(query, pageNumber, pageSize, UserToUserDTO);
        }

        public async Task<bool> BlockById(int id)
        {
            User item = await db.Users.GetAsync(id);
            if (item == null)
            {
                return false;
            }
            item.Blocked = true;
            db.Users.Update(item);
            db.Save();
            return true;
        }

        public async Task<bool> UpdateById(int id, UserDto user)
        {
            var modified = false;
            User item = await db.Users.GetAsync(id);
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
                if (user.Blocked != null)
                {
                    item.Blocked = user.Blocked.Value;
                    modified = true;
                }
                var updatedRole = await db.Roles.TryGetByName(user.Role);
                if (updatedRole != null)
                {
                    item.Role_Id = updatedRole.Id;
                    modified = true;
                }
                db.Users.Update(item);
                db.Save();
            }
            return modified;
        }

        public async Task<bool> ConfirmEmailById(int id)
        {
            User user = await db.Users.GetAsync(id);
            if (user != null)
            {
                user.Email_Confirmed = true;
                db.Users.Update(user);
                db.Save();
                return true;
            }
            return false;
        }

        public async Task<bool> Add(UserRegistrationDto userLoginDTO)
        {
            var toAdd = new User
            {
                Email = userLoginDTO.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(userLoginDTO.Password)
            };
            var studentRole = await db.Roles.TryGetByName("Student");
            toAdd.Role_Id = studentRole.Id;
            toAdd.FirstName = userLoginDTO.FirstName;
            toAdd.LastName = userLoginDTO.LastName;
            db.Users.Add(toAdd);
            db.Save();
            return true;
        }

        public async Task<bool> UpdatePassword(int userId, string password)
        {
            User user = await db.Users.GetAsync(userId);
            if (user == null)
            {
                return false;
            }
            user.Password = BCrypt.Net.BCrypt.HashPassword(password);
            db.Users.Update(user);
            db.Save();
            return true;
        }

        public async Task<List<UserDto>> Search(string[] str, int? roleId)
        {
            IEnumerable<User> users = await  db.Users.SearchAsync(str, roleId);
            var dtos = new List<UserDto>();
            foreach (var user in users)
            {
                dtos.Add( await UserToUserDTO(user));
            }
            return dtos;
        }

        public async Task<PagedListDto<UserDto>> Search(string[] str, int pageSize, int pageNumber, int? roleId)
        {
            var query = (await db.Users.SearchAsync(str, roleId)).AsQueryable();
            query = query.OrderBy(x => x.Id);
            return await PagedList<User, UserDto>.GetDTO(query, pageNumber, pageSize, UserToUserDTO);
        }

        public async Task<List<UserDto>> GetUsersByRole(int roleId)
        {
            IEnumerable<User> users = await db.Users.GetUsersByRoleAsync(roleId);
            if (users == null)
            {
                return null;
            }
            var dtos = new List<UserDto>();
            foreach (var user in users)
            {
                dtos.Add( await UserToUserDTO(user));
            }
            return dtos;
        }

        public async Task<PagedListDto<UserDto>> GetUsersByRole(int roleId, int pageSize, int pageNumber)
        {
            var query = await db.Users.GetUsersByRoleAsync(roleId);
            var queryAwait = query.AsQueryable();

            queryAwait = queryAwait.OrderBy(x => x.Id);
            return await PagedList<User, UserDto>.GetDTO(queryAwait, pageNumber, pageSize, UserToUserDTO);
        }

        public async Task<bool> SetImage(int id, byte[] image, string imageName)
        {
            User userToUpdate = await db.Users.GetAsync(id);
            if (userToUpdate == null)
            {
                return false;
            }
            var converted = Convert.ToBase64String(image);
            userToUpdate.Image = converted;
            userToUpdate.Image_Name = imageName;
            db.Save();
            return true;
        }

        public async Task<ImageDto> GetImage(int id)
        {
            User userToGetImage = await db.Users.GetAsync(id);
            if (userToGetImage?.Image == null || userToGetImage.Image_Name == null)
            {
                return null;
            }
            return new ImageDto()
            {
                Name = userToGetImage.Image_Name,
                Base64Data = userToGetImage.Image
            };
        }

        public async Task<bool> ContainsId(int id)
        {
            return await db.Users.ContainsIdAsync(id);
        }

        public async Task<List<UserDto>> GetUsersByState(bool state)
        {
            IEnumerable<User> users = await db.Users.GetUsersByStateAsync(state);
            if (users == null)
            {
                return null;
            }
            var dtos = new List<UserDto>();
            foreach (var user in users)
            {
                  dtos.Add( await UserToUserDTO(user));
            }
            return dtos;
        }

        public async Task<PagedListDto<UserDto>> GetUsersByState(bool state, int pageSize, int pageNumber)
        {
            var GetQuery = await db.Users.GetUsersByStateAsync(state);
            var query =  GetQuery.AsQueryable();
            query = query.OrderBy(x => x.Id);
            return await PagedList<User, UserDto>.GetDTO(query, pageNumber, pageSize, UserToUserDTO);
        }

        private async Task<UserDto> UserToUserDTO(User user)
        {
            return new UserDto(user.Id,
                               user.FirstName,
                               user.LastName,
                               user.Email,
                               user.Role.Name,
                               user.Blocked,
                               user.Email_Confirmed);
        }
    }
}
