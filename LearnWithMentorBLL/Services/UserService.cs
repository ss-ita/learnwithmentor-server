using System.Collections.Generic;
using LearnWithMentorBLL.Interfaces;
using LearnWithMentorDto;
using LearnWithMentorDAL.Entities;
using System;
using System.Linq;
using LearnWithMentorDAL.UnitOfWork;

namespace LearnWithMentorBLL.Services
{
    public class UserService : BaseService, IUserService
    {
        public UserService(IUnitOfWork db) : base(db)
        {
        }

        public UserDto Get(int id)
        {
            var user = db.Users.Get(id);
            if (user == null)
            {
                return null;
            }
            return UserToUserDTO(user);
        }

        public UserIdentityDto GetByEmail(string email)
        {
            var user = db.Users.GetByEmail(email);
            if (user == null)
            {
                return null;
            }
            return new UserIdentityDto(user.Email, user.Password, user.Id,
                user.FirstName,
                user.LastName,
                user.Roles.Name,
                user.Blocked,
                user.Email_Confirmed);
        }

        public List<UserDto> GetAllUsers()
        {
            var users = db.Users.GetAll();
            var dtos = new List<UserDto>();
            if (users != null)
            {
                foreach (var user in users)
                {
                    dtos.Add(UserToUserDTO(user));
                }
            }
            return dtos;
        }

        public PagedListDto<UserDto> GetUsers(int pageSize, int pageNumber = 0)
        {
            var query = db.Users.GetAll().AsQueryable();
            query = query.OrderBy(x => x.Id);
            return PagedList<User, UserDto>.GetDTO(query, pageNumber, pageSize, UserToUserDTO);
        }

        public bool BlockById(int id)
        {
            var item = db.Users.Get(id);
            if (item == null)
            {
                return false;
            }
            item.Blocked = true;
            db.Users.Update(item);
            db.Save();
            return true;
        }

        public bool UpdateById(int id, UserDto user)
        {
            var modified = false;
            var item = db.Users.Get(id);
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
                if (db.Roles.TryGetByName(user.Role, out var updatedRole))
                {
                    item.Role_Id = updatedRole.Id;
                    modified = true;
                }
                db.Users.Update(item);
                db.Save();
            }
            return modified;
        }
        
        public bool ConfirmEmailById(int id)
        {
            var user = db.Users.Get(id);
            if (user != null)
            {
                user.Email_Confirmed = true;
                db.Users.Update(user);
                db.Save();
                return true;
            }
            return false;
        }

        public bool Add(UserRegistrationDto userLoginDTO)
        {
            var toAdd = new User
            {
                Email = userLoginDTO.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(userLoginDTO.Password)
            };
            db.Roles.TryGetByName("Student", out var studentRole);
            toAdd.Role_Id = studentRole.Id;
            toAdd.FirstName = userLoginDTO.FirstName;
            toAdd.LastName = userLoginDTO.LastName;
            db.Users.Add(toAdd);
            db.Save();
            return true;
        }

        public bool UpdatePassword(int userId, string password)
        {
            var user = db.Users.Get(userId);
            if (user == null)
            {
                return false;
            }
            user.Password = BCrypt.Net.BCrypt.HashPassword(password);
            db.Users.Update(user);
            db.Save();
            return true;
        }

        public List<UserDto> Search(string[] str, int? roleId)
        {
            var users = db.Users.Search(str, roleId);
            var dtos = new List<UserDto>();
            foreach (var user in users)
            {
                dtos.Add(UserToUserDTO(user));
            }
            return dtos;
        }

        public PagedListDto<UserDto> Search(string[] str, int pageSize, int pageNumber, int? roleId)
        {
            var query = db.Users.Search(str, roleId).AsQueryable();
            query = query.OrderBy(x => x.Id);
            return PagedList<User, UserDto>.GetDTO(query, pageNumber, pageSize, UserToUserDTO);
        }

        public List<UserDto> GetUsersByRole(int roleId)
        {
            var users = db.Users.GetUsersByRole(roleId);
            var dtos = new List<UserDto>();
            if (users != null)
            {
                foreach (var user in users)
                {
                    dtos.Add(UserToUserDTO(user));
                }
            }

            return dtos;
        }

        public PagedListDto<UserDto> GetUsersByRole(int roleId, int pageSize, int pageNumber)
        {
            var query = db.Users.GetUsersByRole(roleId).AsQueryable();
            query = query.OrderBy(x => x.Id);
            return PagedList<User, UserDto>.GetDTO(query, pageNumber, pageSize, UserToUserDTO);
        }

        public bool SetImage(int id, byte[] image, string imageName)
        {
            var userToUpdate = db.Users.Get(id);
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

        public ImageDto GetImage(int id)
        {
            var userToGetImage = db.Users.Get(id);
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

        public bool ContainsId(int id)
        {
            return db.Users.ContainsId(id);
        }

        public List<UserDto> GetUsersByState(bool state)
        {
            var users = db.Users.GetUsersByState(state);
            var dtos = new List<UserDto>();
            if (users != null)
            {
                foreach (var user in users)
                {
                    dtos.Add(UserToUserDTO(user));
                }
            }
            return dtos;
        }

        public PagedListDto<UserDto> GetUsersByState(bool state, int pageSize, int pageNumber)
        {
            var query = db.Users.GetUsersByState(state).AsQueryable();
            query = query.OrderBy(x => x.Id);
            return PagedList<User, UserDto>.GetDTO(query, pageNumber, pageSize, UserToUserDTO);
        }

        private UserDto UserToUserDTO(User user)
        {
            return new UserDto(user.Id,
                               user.FirstName,
                               user.LastName,
                               user.Roles.Name,
                               user.Blocked,
                               user.Email_Confirmed);
        }
    }
}
