﻿using System;
using System.ComponentModel.DataAnnotations;

namespace LearnWithMentorDTO
{
    public class TaskDTO
    {
        public TaskDTO(int id,
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
        [Required]
        [StringLength(ValidationRules.MAX_TASK_NAME_LENGTH,
            ErrorMessage = "Task name too long")]
        public string Name { get; set; }
        [Required]
        [StringLength(ValidationRules.MAX_TASK_DESCRIPTION_LENGTH,
            ErrorMessage = "Description too long")]
        public string Description { get; set; }
        [Required]
        public bool Private { get; set; }
        [Required]
        public int CreatorId { get; set; }
        public string CreatorName { get; set; }
        [Required]
        public int? ModifierId { get; set; }
        public string ModifierName { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? ModDate { get; set; }
        public int? Priority { get; set; }
        public int? SectionId { get; set; }
        public int? PlanTaskId { get; set; }
    }
}
