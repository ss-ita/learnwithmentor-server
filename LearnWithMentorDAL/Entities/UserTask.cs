//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace LearnWithMentorDAL.Entities
{
    using System;
    using System.Collections.Generic;
    
    public partial class UserTask
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public UserTask()
        {
            this.Messages = new HashSet<Message>();
        }
    
        public int Id { get; set; }
        public int User_Id { get; set; }
        public int PlanTask_Id { get; set; }
        public string State { get; set; }
        public System.DateTime End_Date { get; set; }
        public string Result { get; set; }
        public Nullable<System.DateTime> Propose_End_Date { get; set; }
    
        public virtual User Users { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Message> Messages { get; set; }
        public virtual PlanTask PlanTasks { get; set; }
    }
}
