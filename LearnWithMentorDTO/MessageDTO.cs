using System;
using System.ComponentModel.DataAnnotations;
using LearnWithMentorDTO.Infrastructure;

namespace LearnWithMentorDTO
{
    public class MessageDTO
    {
        public MessageDTO(int id,
                        int senderId,
                        int userTaskId,
                        string senderName,
                        string text,
                        DateTime? sendTime)
        {
            Id = id;
            SenderId = senderId;
            UserTaskId = userTaskId;
            Text = text;
            SendTime = sendTime;
            SenderName = senderName;
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
    }
}
