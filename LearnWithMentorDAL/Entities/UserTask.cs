using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace LearnWithMentorDAL.Entities
{
    public class UserTask
    {
        [SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public UserTask()
        {
            Messages = new HashSet<Message>();
        }

        public int Id { get; set; }
        public int User_Id { get; set; }
        public int PlanTask_Id { get; set; }
        public string State { get; set; }
        public DateTime? End_Date { get; set; }
        public string Result { get; set; }
        public DateTime? Propose_End_Date { get; set; }
        public int Mentor_Id { get; set; }
        
        public virtual GroupUser User { get; set; }
        public virtual PlanTask PlanTask { get; set; }
        public virtual GroupUser Mentor { get; set; }
        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Message> Messages { get; set; }
    }
}