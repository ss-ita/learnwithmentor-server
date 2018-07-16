using LearnWithMentorBLL.Interfaces;
using LearnWithMentorDAL.UnitOfWork;
using LearnWithMentorDAL.Entities;

namespace LearnWithMentorBLL.Services
{
    public class BaseService: IDisposableService
    {
        protected readonly IUnitOfWork db;
        public BaseService(IUnitOfWork db)
        {
            this.db = db;
        }
        public void Dispose()
        {
            db.Dispose();
        }

    }
}
