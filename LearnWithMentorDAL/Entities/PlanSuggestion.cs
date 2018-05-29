namespace LearnWithMentorDAL.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PlanSuggestion")]
    public partial class PlanSuggestion
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Plan_Id { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int User_Id { get; set; }

        [Key]
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Mentor_Id { get; set; }

        [Key]
        [Column(Order = 3)]
        [StringLength(1000)]
        public string Text { get; set; }

        public virtual Plans Plans { get; set; }

        public virtual Users Users { get; set; }

        public virtual Users Users1 { get; set; }
    }
}
