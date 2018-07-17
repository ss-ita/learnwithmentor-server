namespace LearnWithMentorBLL.Services
{
    public class DataBaseService
    {
        public  DataBaseService()
        {
            DbInitialize();
        }

        public void DbInitialize()
        {
            LearnWithMentorDAL.EF.LearnWithMentorInitializer.Initialize();
        }
    }
}
