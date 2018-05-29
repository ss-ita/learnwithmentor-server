namespace LearnWithMentorDAL.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class UserTasks
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public UserTasks()
        {
            Messages = new HashSet<Messages>();
        }

        public int Id { get; set; }

        public int User_Id { get; set; }

        public int Task_Id { get; set; }

        [Required]
        [StringLength(1)]
        public string State { get; set; }

        public DateTime End_Date { get; set; }

        [Required]
        public string Result { get; set; }

        public DateTime? Propose_End_Date { get; set; }

        public virtual Tasks Tasks { get; set; }

        public virtual Users Users { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Messages> Messages { get; set; }
    }
}
