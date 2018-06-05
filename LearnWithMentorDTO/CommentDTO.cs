using System;

namespace LearnWithMentorDTO
{
    public class CommentDTO
    {
        public CommentDTO(int id, string text, int creatorId, string creatorFirstName, string creatorLastName, DateTime? createDate, DateTime? modDate)
        {
            Id = id;
            Text = text;
            CreatorId = creatorId;
            CreatorFirstName = creatorFirstName;
            CreatorLastName = creatorLastName;
            CreateDate = createDate;
            ModDate = modDate;
        }

        public int Id { get; set; }
        public string Text { get; set; }
        public int CreatorId { get; set; }
        public string CreatorFirstName { get; set; }
        public string CreatorLastName { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? ModDate { get; set; }
    }
}
