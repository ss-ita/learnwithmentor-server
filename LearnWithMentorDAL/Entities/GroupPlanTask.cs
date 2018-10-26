using System;
using System.ComponentModel.DataAnnotations;

namespace LearnWithMentorDAL.Entities
{
    public class GroupPlanTask
    {
        [Key]
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? ModDate { get; set; }
        public bool Published { get; set; }
        public int? Priority { get; set; }
        public string SectionName { get; set; }
        public string TaskName { get; set; }
        public string TaskDescription { get; set; }
        public DateTime? TasksCreateDate { get; set; }
        public DateTime? TaskModDate { get; set; }
        public bool Private { get; set; }
    }
}
