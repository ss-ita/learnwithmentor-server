namespace LearnWithMentor.Controllers
{
    public static class Constants
    {
        public static class Roles
        {
            public const string Mentor = "Mentor";
            public const string Student = "Student";
            public const string Admin = "Admin";
            public const string Blocked = "blocked";
            public const int BlockedIndex = -1;
        }

        public static class ImageRestrictions
        {
            public const int MaxSize = 1024 * 1024 * 1; // 1MB
            public static string[] Extensions = {".jpeg", ".jpg", ".png"};
        }
    }
}