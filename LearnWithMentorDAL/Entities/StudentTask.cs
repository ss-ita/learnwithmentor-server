using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace LearnWithMentorDAL.Entities
{
    [System.ComponentModel.DataAnnotations.Schema.Table("Tasks")]
    public class StudentTask
    {
        [SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public StudentTask()
        {
            PlanTasks = new HashSet<PlanTask>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Private { get; set; }
        public int Create_Id { get; set; }
        public int? Mod_Id { get; set; }
        public DateTime? Create_Date { get; set; }
        public DateTime? Mod_Date { get; set; }

        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PlanTask> PlanTasks { get; set; }
        public virtual User Creator { get; set; }
        public virtual User Modifier { get; set; }
    }
}
