using System;
using System.ComponentModel.DataAnnotations;

namespace LearnWithMentorDTO
{
    public class MessageDTO
    {
        public MessageDTO(int id,
                        int senderId,
                        int taskId,
                        int planId,
                        int receiverId,
                        string text,
                        DateTime sendTime)
        {
            Id = id;
            SenderId = senderId;
            TaskId = taskId;
            PlanId = planId;
            ReceiverId = receiverId;
            Text = text;
            SendTime = sendTime;
        }

        public int Id { get; set; }
        public int SenderId { get; set; }
        public int ReceiverId { get; set; }
        public int TaskId { get; set; }
        public int PlanId { get; set; }
        public string Text { get; set; }
        public DateTime SendTime { get; set; }

    }
}
