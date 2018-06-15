using System;
using System.Collections.Generic;
using LearnWithMentorDAL;
using LearnWithMentorDAL.Entities;
using LearnWithMentorDTO;

namespace LearnWithMentorBLL.Interfaces
{
    public interface IUserService
    {
        UserDTO Get(int id);
        bool BlockById(int id);
        bool UpdateById(int id, UserDTO user);
        bool Add(UserLoginDTO dto);
        IEnumerable<UserDTO> Search(string[] str, int? role_id);
        IEnumerable<UserDTO> GetUsersByRole(int roleId);
        string ExtractFullName(int? id);
    }
}
