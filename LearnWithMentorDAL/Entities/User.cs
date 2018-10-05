using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.ComponentModel.DataAnnotations;

namespace LearnWithMentorDAL.Entities
{
    public class User
    {
        [SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public User()
        {
            Comments = new HashSet<Comment>();
            GroupMentor = new HashSet<Group>();
            PlansCreated = new HashSet<Plan>();
            PlansModified = new HashSet<Plan>();
            TasksCreated = new HashSet<Task>();
            TasksModified = new HashSet<Task>();
            PlanSuggestionsStudent = new HashSet<PlanSuggestion>();
            PlanSuggestionsMentor = new HashSet<PlanSuggestion>();
            UserTasks = new HashSet<UserTask>();
            Groups = new HashSet<Group>();
            UserTaskMentor = new HashSet<UserTask>();
            Messages = new HashSet<Message>();
        }

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool Blocked { get; set; }
        public string Image { get; set; }
        public string Image_Name { get; set; }
        public bool Email_Confirmed { get; set; }
        public int Role_Id { get; set; }

        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Comment> Comments { get; set; }
        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Group> GroupMentor { get; set; }
        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Plan> PlansCreated { get; set; }
        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Plan> PlansModified { get; set; }
        public virtual Role Role { get; set; }
        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Task> TasksCreated { get; set; }
        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Task> TasksModified { get; set; }
        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PlanSuggestion> PlanSuggestionsStudent { get; set; }
        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PlanSuggestion> PlanSuggestionsMentor { get; set; }
        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UserTask> UserTasks { get; set; }
        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Group> Groups { get; set; }
        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UserTask> UserTaskMentor { get; set; }
        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Message> Messages { get; set; }
    }
}
