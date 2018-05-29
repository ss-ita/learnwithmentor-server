namespace LearnWithMentorDAL.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Comments
    {
        public int Id { get; set; }

        public int Task_Id { get; set; }

        [Required]
        [StringLength(2000)]
        public string Text { get; set; }

        public int Create_Id { get; set; }

        public DateTime? Create_Date { get; set; }

        public DateTime? Mod_Date { get; set; }

        public virtual Tasks Tasks { get; set; }

        public virtual Users Users { get; set; }
    }
}
