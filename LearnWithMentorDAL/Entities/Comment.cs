using System;

namespace LearnWithMentorDAL.Entities
{
    public class Comment
    {
        public int Id { get; set; }
        public int PlanTaskId { get; set; }
        public string Text { get; set; }
        public int CreateId { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? ModDate { get; set; }
        
        public virtual PlanTask PlanTask { get; set; }
        public virtual User Creator { get; set; }
    }
}
