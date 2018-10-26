namespace LearnWithMentorDAL.Entities
{
    public class PlanSuggestion
    {
        public int Id { get; set; }
        public int PlanId { get; set; }
        public int UserId { get; set; }
        public int MentorId { get; set; }
        public string Text { get; set; }
        
        public virtual Plan Plan { get; set; }
        public virtual User User { get; set; }
        public virtual User Mentor { get; set; }
    }
}
