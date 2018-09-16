namespace LearnWithMentorBLL.Services
{
    public static class DataBaseService
    {
        public static void DbInitialize()
        {
            LearnWithMentorDAL.EF.LearnWithMentorInitializer.Initialize();
        }
    }
}
