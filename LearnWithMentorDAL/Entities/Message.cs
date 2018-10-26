using System;

namespace LearnWithMentorDAL.Entities
{

    public class Message
    {
        public int Id { get; set; }
        public int UserTaskId { get; set; }
        public int UserId { get; set; }
        public string Text { get; set; }
        public DateTime? SendTime { get; set; }
        public bool IsRead { get; set; }

        public virtual User Creator { get; set; }
        public virtual UserTask UserTask { get; set; }
        
    }
}
