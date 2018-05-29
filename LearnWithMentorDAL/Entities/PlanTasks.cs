namespace LearnWithMentorDAL.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class PlanTasks
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Plan_Id { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Task_Id { get; set; }

        public int? Priority { get; set; }

        public int? Section_Id { get; set; }

        public virtual Plans Plans { get; set; }

        public virtual Sections Sections { get; set; }

        public virtual Tasks Tasks { get; set; }
    }
}
