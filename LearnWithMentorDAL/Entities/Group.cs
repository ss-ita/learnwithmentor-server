using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace LearnWithMentorDAL.Entities
{
    public class Group
    {
        [SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Group()
        {
            Plans = new HashSet<Plan>();
            Users = new HashSet<User>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int? MentorId { get; set; }
        
        public virtual User Mentor { get; set; }
        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Plan> Plans { get; set; }
        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<User> Users { get; set; }
    }
}