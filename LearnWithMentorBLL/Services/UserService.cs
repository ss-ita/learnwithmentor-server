using System.Collections.Generic;
using System.Linq;
using LearnWithMentorBLL.Interfaces;
using LearnWithMentorDTO;
using LearnWithMentorDAL.Entities;
using System;
using LearnWithMentorDAL.UnitOfWork;

namespace LearnWithMentorBLL.Services
{
    public class UserService : BaseService, IUserService
    {
        public UserService(IUnitOfWork db) : base(db)
        {
        }
        public UserDTO Get(int id)
        {
            User user = db.Users.Get(id);
            if (user == null)
                return null;
            return new UserDTO(user.Id,
                               user.FirstName,
                               user.LastName,
                               user.Roles.Name,
                               user.Blocked);
        }
        public UserIdentityDTO GetByEmail(string email)
        {
            User user = db.Users.GetByEmail(email);
            if (user == null)
                return null;
            return new UserIdentityDTO(user.Email, user.Password, user.Id,
                user.FirstName,
                user.LastName,
                user.Roles.Name,
                user.Blocked);
        }

        public List<UserDTO> GetAllUsers()
        {
            var users = db.Users.GetAll();
            if (!users.Any())
                return null;
            List<UserDTO> dtos = new List<UserDTO>();
            foreach (var user in users)
            {
                dtos.Add(new UserDTO(user.Id,
                                     user.FirstName,
                                     user.LastName,
                                     user.Roles.Name,
                                     user.Blocked));
            }
            return dtos;
        }
        public bool BlockById(int id)
        {
            var item = db.Users.Get(id);
            if (item != null)
            {
                item.Blocked = true;
                db.Users.Update(item);
                db.Save();
                return true;
            }
            return false;
        }
        public bool UpdateById(int id, UserDTO user)
        {
            bool modified = false;
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
                Role updatedRole;
                if (db.Roles.TryGetByName(user.Role, out updatedRole))
                {
                    item.Role_Id = updatedRole.Id;
                    modified = true;
                }
                db.Users.Update(item);
                db.Save();
            }
            return modified;
        }
        public bool Add(UserRegistrationDTO userLoginDTO)
        {
            User toAdd = new User();
            toAdd.Email = userLoginDTO.Email;
            toAdd.Password = BCrypt.Net.BCrypt.HashPassword(userLoginDTO.Password);
            Role studentRole;
            db.Roles.TryGetByName("Student", out studentRole);
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
            if(user == null)
                return false;
            user.Password = BCrypt.Net.BCrypt.HashPassword(password);
            db.Users.Update(user);
            db.Save();
            return true;
        }
        public List<UserDTO> Search(string[] str, int? roleId)
        {
            var users = db.Users.Search(str, roleId);
            List<UserDTO> dtos = new List<UserDTO>();
            foreach (var user in users)
            {
                dtos.Add(new UserDTO(user.Id,
                                     user.FirstName,
                                     user.LastName,
                                     user.Roles.Name,
                                     user.Blocked));
            }
            return dtos;
        }
        public List<UserDTO> GetUsersByRole(int role_id)
        {
            var users = db.Users.GetUsersByRole(role_id);
            if (users == null)
                return null;
            List<UserDTO> dtos = new List<UserDTO>();
            foreach (var user in users)
            {
                dtos.Add(new UserDTO(user.Id,
                                     user.FirstName,
                                     user.LastName,
                                     user.Roles.Name,
                                     user.Blocked));
            }
            return dtos;
        }

        public bool SetImage(int id, byte[] image, string imageName)
        {
            var userToUpdate = db.Users.Get(id);
            if (userToUpdate == null)
                return false;
            string converted = Convert.ToBase64String(image);
            userToUpdate.Image = converted;
            userToUpdate.Image_Name = imageName;
            db.Save();
            return true;
        }

        public ImageDTO GetImage(int id)
        {
            var userToGetImage = db.Users.Get(id);
            if (userToGetImage == null || userToGetImage.Image == null || userToGetImage.Image_Name == null)
                return null;
            return new ImageDTO()
            {
                Name = userToGetImage.Image_Name,
                Base64Data = userToGetImage.Image
            };
        }

        public bool ContainsId(int id)
        {
            return db.Users.ContainsId(id);
        }

        public List<UserDTO> GetUsersByState(bool state)
        {
            var users = db.Users.GetUsersByState(state);
            if (users == null)
                return null;
            List<UserDTO> dtos = new List<UserDTO>();
            foreach (var user in users)
            {
                dtos.Add(new UserDTO(user.Id,
                                     user.FirstName,
                                     user.LastName,
                                     user.Roles.Name,
                                     user.Blocked));
            }
            return dtos;
        }

        public PagedListDTO<UserDTO> GetUsers(int pageSize, int pageNumber = 1)
        {
            IQueryable<User> query = db.Users.GetAll().AsQueryable();
            List<UserDTO> users = new List<UserDTO>();
            foreach (var user in query)
            {
                users.Add(UserToUserDTO(user));
            }
            return PagedList<UserDTO>.GetDTO(users, pageNumber, pageSize);
        }
        private UserDTO UserToUserDTO(User user)
        {
            return new UserDTO(user.Id,
                                     user.FirstName,
                                     user.LastName,
                                     user.Roles.Name,
                                     user.Blocked);
        }
    }
}
