using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.ComponentModel.DataAnnotations.Schema;

namespace LearnWithMentorDAL.Entities
{
    public class Plan
    {
        [SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Plan()
        {
            PlanSuggestion = new HashSet<PlanSuggestion>();
            PlanTasks = new HashSet<PlanTask>();
            Groups = new HashSet<Group>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Published { get; set; }
        public int CreateId { get; set; }
        public int? ModId { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? ModDate { get; set; }
        public string Image { get; set; }
        public string ImageName { get; set; }
        
        public virtual User Creator { get; set; }
        public virtual User Modifier { get; set; }
        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PlanSuggestion> PlanSuggestion { get; set; }
        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PlanTask> PlanTasks { get; set; }
        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Group> Groups { get; set; }
    }
}