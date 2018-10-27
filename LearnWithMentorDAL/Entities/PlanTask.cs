using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace LearnWithMentorDAL.Entities
{
    public class PlanTask
    {
        [SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PlanTask()
        {
            Comments = new HashSet<Comment>();
            UserTasks = new HashSet<UserTask>();
        }

        public int Id { get; set; }
        public int Plan_Id { get; set; }
        public int Task_Id { get; set; }
        public int? Priority { get; set; }
        public int? Section_Id { get; set; }

        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual Plan Plans { get; set; }
        public virtual Section Sections { get; set; }
        public virtual StudentTask Tasks { get; set; }
        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UserTask> UserTasks { get; set; }
    }
}
