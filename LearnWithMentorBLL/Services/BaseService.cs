using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LearnWithMentorBLL.Interfaces;
using LearnWithMentorDTO;
using LearnWithMentorDAL.UnitOfWork;
using LearnWithMentorDAL.Entities;

namespace LearnWithMentorBLL.Services
{
    public class BaseService
    {
        protected readonly IUnitOfWork db;

        public BaseService()
        {
            db = new UnitOfWork(new LearnWithMentor_DBEntities());
        }
        public void Dispose()
        {
            db.Dispose();
        }
    }
}
