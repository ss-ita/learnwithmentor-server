namespace LearnWithMentorDAL.Entities
{
    public class PlanSuggestion
    {
        public int Id { get; set; }
        public int Plan_Id { get; set; }
        public int User_Id { get; set; }
        public int Mentor_Id { get; set; }
        public string Text { get; set; }
        
        public virtual Plan Plan { get; set; }
        public virtual User User { get; set; }
        public virtual User Mentor { get; set; }
    }
}
