using LearnWithMentorBLL.Interfaces;

namespace LearnWithMentorBLL.Services
{
    public class DataBaseService : BaseService, IDataBaseService
    {
        public DataBaseService()
        {

        }

        public void DbInitialize()
        {
            LearnWithMentorDAL.EF.LearnWithMentorInitializer.Initialize();
        }
    }
}
