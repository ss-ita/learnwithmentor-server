
namespace LearnWithMentorDTO
{
    public static class ValidationRules
    {
        public const int MAX_LENGTH_NAME = 20;
        public const string ONLY_LETTERS_AND_NUMBERS = @"[a-zA-z0-9]*$";
        public const string EMAIL_REGEX = @"^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9-]+(?:\.[a-zA-Z0-9-]+)*$";
        public const int MAX_TASK_NAME_LENGTH = 50;
        public const int MAX_TASK_DESCRIPTION_LENGTH = 1000;
        public const string USERTASK_STATE = @"[P,D,A,R]$";
        public const int MAX_USERTASK_RESULT_LENGTH = 10000;
    }
}
