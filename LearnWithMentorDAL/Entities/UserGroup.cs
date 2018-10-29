using System.ComponentModel.DataAnnotations.Schema;

namespace LearnWithMentorDAL.Entities
{
    public class UserGroup
    {
        public int Id { get; set; }
        [ForeignKey(nameof(User))]
        public int UserId { get; set; }
        [ForeignKey(nameof(Group))]
        public int GroupId { get; set; }

        public virtual User User { get; set; }
        public virtual Group Group { get; set; }
    }
}
