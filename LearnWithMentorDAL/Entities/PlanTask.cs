//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace LearnWithMentorDAL.Entities
{
    using System;
    using System.Collections.Generic;
    
    public partial class PlanTask
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PlanTask()
        {
            this.Comments = new HashSet<Comment>();
        }
    
        public int Id { get; set; }
        public int Plan_Id { get; set; }
        public int Task_Id { get; set; }
        public Nullable<int> Priority { get; set; }
        public Nullable<int> Section_Id { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual Plan Plans { get; set; }
        public virtual Section Sections { get; set; }
        public virtual Task Tasks { get; set; }
    }
}
