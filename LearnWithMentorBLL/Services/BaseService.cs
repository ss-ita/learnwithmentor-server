using LearnWithMentorBLL.Interfaces;
using LearnWithMentorDAL.UnitOfWork;
using LearnWithMentorDAL.Entities;

namespace LearnWithMentorBLL.Services
{
    public class BaseService: IDisposableService
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
