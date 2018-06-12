using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnWithMentorDTO
{
    public static class ValidationRules
    {
        public const int MAX_LENGTH_NAME = 20;
        public const string ONLY_LETTERS_AND_NUMBERS = @"[a-zA-z0-9]*$";
        public const string EMAIL_REGEX = @"^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9-]+(?:\.[a-zA-Z0-9-]+)*$";
    }
}
