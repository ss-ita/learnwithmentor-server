using System;
using System.ComponentModel.DataAnnotations;
using LearnWithMentorDTO.Infrastructure;

namespace LearnWithMentorDTO
{
    public class MessageDto
    {
        public MessageDto(int id,
                        int senderId,
                        int userTaskId,
                        string senderName,
                        string text,
                        DateTime? sendTime,
                        bool isRead)
        {
            Id = id;
            SenderId = senderId;
            UserTaskId = userTaskId;
            Text = text;
            SendTime = sendTime;
            SenderName = senderName;
            IsRead = isRead;
        }

        public int Id { get; set; }
        public int SenderId { get; set; }
        public int UserTaskId { get; set; }
        public string SenderName { get; set; }
        [Required]
        [StringLength(ValidationRules.MAX_MESSAGE_LENGTH,
            ErrorMessage = "Message is too long")]
        public string Text { get; set; }
        public DateTime? SendTime { get; set; }
        public bool IsRead { get; set; }

    }
}
