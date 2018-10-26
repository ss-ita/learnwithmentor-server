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
        public int UserId { get; set; }
        public int PlanTaskId { get; set; }
        public string State { get; set; }
        public DateTime? EndDate { get; set; }
        public string Result { get; set; }
        public DateTime? ProposeEndDate { get; set; }
        public int MentorId { get; set; }
        
        public virtual User User { get; set; }
        public virtual PlanTask PlanTask { get; set; }
        public virtual User Mentor { get; set; }
        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Message> Messages { get; set; }
    }
}