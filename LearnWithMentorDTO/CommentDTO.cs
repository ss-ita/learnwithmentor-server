using System;

namespace LearnWithMentorDTO
{
    public class CommentDTO
    {
        public CommentDTO(int id, string text, int creatorId, string creatorFullName, DateTime? createDate, DateTime? modDate)
        {
            Id = id;
            Text = text;
            CreatorId = creatorId;
            CreatorFullName = creatorFullName;
            CreateDate = createDate;
            ModDate = modDate;
        }

        public int Id { get; set; }
        public string Text { get; set; }
        public int CreatorId { get; set; }
        public string CreatorFullName { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? ModDate { get; set; }
    }
}
