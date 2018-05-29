namespace LearnWithMentorDAL.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("GROUPS-PLANS-TASKS")]
    public partial class GROUPS_PLANS_TASKS
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        public DateTime? Create_Date { get; set; }

        public DateTime? Mod_Date { get; set; }

        [Key]
        [Column(Order = 1)]
        public bool Published { get; set; }

        public int? Priority { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(50)]
        public string Section_Name { get; set; }

        [Key]
        [Column(Order = 3)]
        [StringLength(50)]
        public string Task_Name { get; set; }

        public string Task_Description { get; set; }

        public DateTime? Tasks_Create_Date { get; set; }

        public DateTime? Task_Mod_Date { get; set; }

        [Key]
        [Column(Order = 4)]
        public bool Private { get; set; }
    }
}
