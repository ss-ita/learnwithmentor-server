using System;
using System.ComponentModel.DataAnnotations;

namespace LearnWithMentorDTO.Models
{
    class UserTaskDTO
    {
        public UserTaskDTO(int id,
                string name,
                string description,
                bool privateness,
                int createId,
                string creatorName,
                int? modId,
                string modifierName,
                DateTime? createDate,
                DateTime? modDate,
                int? priority,
                int? sectionId)
        {
            Id = id;
            Name = name;
            Description = description;
            Private = privateness;
            CreateDate = createDate;
            ModDate = modDate;
            CreatorId = createId;
            CreatorName = creatorName;
            ModifierId = modId;
            ModifierName = modifierName;
            Priority = priority;
            SectionId = sectionId;
        }

        public int Id { get; set; }
        public int User_Id { get; set; }
        public int PlanTask_Id { get; set; }
        public string State { get; set; }
        public DateTime? End_Date { get; set; }
        public string Result { get; set; }
        public DateTime? Propose_End_Date { get; set; }
        public int Mentor_Id { get; set; }
    }
}
