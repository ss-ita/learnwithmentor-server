using System;

namespace LearnWithMentorDAL.Entities
{

    public class Message
    {
        public int Id { get; set; }
        public int UserTask_Id { get; set; }
        public int User_Id { get; set; }
        public string Text { get; set; }
        public DateTime? Send_Time { get; set; }
        
        public virtual User Creator { get; set; }
        public virtual UserTask UserTask { get; set; }
    }
}
