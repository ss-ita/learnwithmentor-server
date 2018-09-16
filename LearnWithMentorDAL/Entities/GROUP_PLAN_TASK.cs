using System;
using System.ComponentModel.DataAnnotations;

namespace LearnWithMentorDAL.Entities
{
    public class GROUP_PLAN_TASK
    {
        [Key]
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime? Create_Date { get; set; }
        public DateTime? Mod_Date { get; set; }
        public bool Published { get; set; }
        public int? Priority { get; set; }
        public string Section_Name { get; set; }
        public string Task_Name { get; set; }
        public string Task_Description { get; set; }
        public DateTime? Tasks_Create_Date { get; set; }
        public DateTime? Task_Mod_Date { get; set; }
        public bool Private { get; set; }
    }
}
