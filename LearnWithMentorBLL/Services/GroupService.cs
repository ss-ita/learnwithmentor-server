using System;
using System.Collections.Generic;
using LearnWithMentorBLL.Interfaces;
using LearnWithMentorDAL;
using LearnWithMentorDAL.Entities;
using LearnWithMentorDTO;

namespace LearnWithMentorBLL.Services
{
    public class GroupService : BaseService, IGroupService
    {
        public GroupService() : base()
        {
        }
        public bool AddGroup(GroupDTO group)
        {           
                if (string.IsNullOrEmpty(group.Name))
                    return false;
                var groupNew = new Group
                {
                    Name = group.Name,
                    Mentor_Id = group.MentorID
                };
                db.Groups.Add(groupNew);
                db.Save();
                return true;
            
        }

        public bool AddPlanToGroup(int planId, int groupId)
        {
            throw new NotImplementedException();
        }

        public bool AddUserToGroup(int userId, int groupId)
        {
            throw new NotImplementedException();
        }

        public GroupDTO GetGroupById(int id)
        {
            Group group = db.Groups.Get(id);
            if (group == null)
                return null;
            return new GroupDTO(group.Id,
                               group.Name,
                               group.Mentor_Id
                               );
        }
        public IEnumerable<UserDTO> GetUsers(int groupId)
        {
            var group = db.Groups.GetGroupsByMentor(groupId);
            var users = db.Users.GetAll();
            if (group == null)
                return null;
            List<UserDTO> dtos = new List<UserDTO>();
            foreach (var user in users)
            {
                dtos.Add(new UserDTO(user.Id,
                                     user.FirstName,
                                     user.LastName,
                                     user.Roles.Name,
                                     user.Blocked
                                    ));
            }
            return dtos;

        }

            public IEnumerable<GroupDTO> GetGroupsByMentor(int mentorId)
        {

           var groups = db.Groups.GetGroupsByMentor(mentorId);
            if (groups == null)
                return null;
            List<GroupDTO> dtos = new List<GroupDTO>();
            foreach (var group in groups)
            {
                dtos.Add(new GroupDTO(group.Id,
                                     group.Name,
                                     group.Mentor_Id
                                    ));
            }
            return dtos;
        }
    }
}
