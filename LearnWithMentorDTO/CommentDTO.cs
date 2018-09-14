using System;
using System.ComponentModel.DataAnnotations;
using LearnWithMentorDto.Infrastructure;

namespace LearnWithMentorDto
{
    public class CommentDto
    {
        public CommentDto(int id, string text, int creatorId, string creatorFullName, DateTime? createDate, DateTime? modDate)
        {
            Id = id;
            Text = text;
            CreatorId = creatorId;
            CreatorFullName = creatorFullName;
            CreateDate = createDate;
            ModDate = modDate;
        }

        public int Id { get; set; }
        [Required]
        [StringLength(ValidationRules.MAX_COMMENT_TEXT_LENGTH,
            ErrorMessage = "Comment text too long")]
        public string Text { get; set; }
        public int CreatorId { get; set; }
        public string CreatorFullName { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? ModDate { get; set; }
    }
}
