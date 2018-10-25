using System;

namespace LearnWithMentorDAL.Entities
{
    public class Comment
    {
        public int Id { get; set; }
        public int PlanTask_Id { get; set; }
        public string Text { get; set; }
        public int Create_Id { get; set; }
        public DateTime? Create_Date { get; set; }
        public DateTime? Mod_Date { get; set; }
        
        public virtual PlanTask PlanTask { get; set; }
        public virtual User Creator { get; set; }
    }
}
