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

        public async Task<UserDto> GetAsync(int id)
        {
            User user = await db.Users.GetAsync(id);
            if (user == null)
            {
                return null;
            }
            return await UserToUserDTOAsync(user);
        }

        public async Task<UserIdentityDto> GetByEmailAsync(string email)
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

        public async Task<List<UserDto>> GetAllUsersAsync()
        {
            IEnumerable<User> users = await db.Users.GetAll();
            if (users == null)
            {
                return null;
            }
            var dtos = new List<UserDto>();
            foreach (var user in users)
            {
                dtos.Add(await UserToUserDTOAsync(user));
            }
            return dtos;
        }

        public async Task<PagedListDto<UserDto>> GetUsers(int pageSize, int pageNumber = 0)
        {
            //var query =  await db.Users.GetAll().AsQueryable();
            //query = query.OrderBy(x => x.Id);
            var queryLan =  await db.Users.GetAll();
            var query = queryLan.AsQueryable();
            query = query.OrderBy(x => x.Id);

            return await PagedList<User, UserDto>.GetDTO(query, pageNumber, pageSize, UserToUserDTOAsync);
        }

        public async Task<bool> BlockByIdAsync(int id)
        {
            User item = await db.Users.GetAsync(id);
            if (item == null)
            {
                return false;
            }
            item.Blocked = true;
            db.Users.UpdateAsync(item);
            db.Save();
            return true;
        }

        public async Task<bool> UpdateByIdAsync(int id, UserDto user)
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
                db.Users.UpdateAsync(item);
                db.Save();
            }
            return modified;
        }

        public async Task<bool> ConfirmEmailByIdAsync(int id)
        {
            User user = await db.Users.GetAsync(id);
            if (user != null)
            {
                user.Email_Confirmed = true;
                db.Users.UpdateAsync(user);
                db.Save();
                return true;
            }
            return false;
        }

        public async Task<bool> AddAsync(UserRegistrationDto userLoginDTO)
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
            db.Users.AddAsync(toAdd);
            db.Save();
            return true;
        }

        public async Task<bool> UpdatePasswordAsync(int userId, string password)
        {
            User user = await db.Users.GetAsync(userId);
            if (user == null)
            {
                return false;
            }
            user.Password = BCrypt.Net.BCrypt.HashPassword(password);
            await db.Users.UpdateAsync(user);
            db.Save();
            return true;
        }

        public async Task<List<UserDto>> SearchAsync(string[] str, int? roleId)
        {
            IEnumerable<User> users = await  db.Users.SearchAsync(str, roleId);
            var dtos = new List<UserDto>();
            foreach (var user in users)
            {
                dtos.Add( await UserToUserDTOAsync(user));
            }
            return dtos;
        }

        public async Task<PagedListDto<UserDto>> SearchAsync(string[] str, int pageSize, int pageNumber, int? roleId)
        {
            var query = (await db.Users.SearchAsync(str, roleId)).AsQueryable();
            query = query.OrderBy(x => x.Id);
            return await PagedList<User, UserDto>.GetDTO(query, pageNumber, pageSize, UserToUserDTOAsync);
        }

        public async Task<List<UserDto>> GetUsersByRoleAsync(int roleId)
        {
            IEnumerable<User> users = await db.Users.GetUsersByRoleAsync(roleId);
            if (users == null)
            {
                return null;
            }
            var dtos = new List<UserDto>();
            foreach (var user in users)
            {
                dtos.Add( await UserToUserDTOAsync(user));
            }
            return dtos;
        }

        public async Task<PagedListDto<UserDto>> GetUsersByRoleAsync(int roleId, int pageSize, int pageNumber)
        {
            var query = await db.Users.GetUsersByRoleAsync(roleId);
            var queryAwait = query.AsQueryable();

            queryAwait = queryAwait.OrderBy(x => x.Id);
            return await PagedList<User, UserDto>.GetDTO(queryAwait, pageNumber, pageSize, UserToUserDTOAsync);
        }

        public async Task<bool> SetImageAsync(int id, byte[] image, string imageName)
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

        public async Task<ImageDto> GetImageAsync(int id)
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

        public async Task<bool> ContainsIdAsync(int id)
        {
            return await db.Users.ContainsIdAsync(id);
        }

        public async Task<List<UserDto>> GetUsersByStateAsync(bool state)
        {
            IEnumerable<User> users = await db.Users.GetUsersByStateAsync(state);
            if (users == null)
            {
                return null;
            }
            var dtos = new List<UserDto>();
            foreach (var user in users)
            {
                  dtos.Add( await UserToUserDTOAsync(user));
            }
            return dtos;
        }

        public async Task<PagedListDto<UserDto>> GetUsersByStateAsync(bool state, int pageSize, int pageNumber)
        {
            var GetQuery = await db.Users.GetUsersByStateAsync(state);
            var query =  GetQuery.AsQueryable();
            query = query.OrderBy(x => x.Id);
            return await PagedList<User, UserDto>.GetDTO(query, pageNumber, pageSize, UserToUserDTOAsync);
        }

        private async Task<UserDto> UserToUserDTOAsync(User user)
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
