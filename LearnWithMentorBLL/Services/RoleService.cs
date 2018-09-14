﻿using System.Collections.Generic;
using LearnWithMentorBLL.Interfaces;
using LearnWithMentorDto;
using LearnWithMentorDAL.UnitOfWork;

namespace LearnWithMentorBLL.Services
{
    public class RoleService : BaseService, IRoleService
    {
        public RoleService(IUnitOfWork db) : base(db)
        {
        }
        public RoleDto Get(int id)
        {
            var role = db.Roles.Get(id);
            return role == null ? null :
                new RoleDto(role.Id, role.Name);
        }
        public List<RoleDto> GetAllRoles()
        {
            var roles = db.Roles.GetAll();
            var dtos = new List<RoleDto>();
            if (roles != null)
            {
                foreach (var role in roles)
                {
                    dtos.Add(new RoleDto(role.Id, role.Name));
                }
            }
            return dtos;
        }
        public RoleDto GetByName(string name)
        {
            if (!db.Roles.TryGetByName(name, out var role))
            {
                return null;
            }
            return new RoleDto(role.Id, role.Name);
        }
    }
}
