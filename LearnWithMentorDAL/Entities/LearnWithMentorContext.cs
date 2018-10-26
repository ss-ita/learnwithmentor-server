using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Core.Objects;

namespace LearnWithMentorDAL.Entities
{
    public partial class LearnWithMentorContext: DbContext
    {
        public LearnWithMentorContext()
            : base("name=LearnWithMentorContext")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            CreateUserReferences(modelBuilder);
            CreateCommentReferences(modelBuilder);
            CreateGroupReferences(modelBuilder);
            CreateMessageReferences(modelBuilder);
            CreatePlanReferences(modelBuilder);
            CreatePlanSuggestionReferences(modelBuilder);
            CreatePlanTaskReferences(modelBuilder);
            CreateRoleReferences(modelBuilder);
            CreateSectionReferences(modelBuilder);
            CreateTaskReferences(modelBuilder);
            CreateUserTaskReferences(modelBuilder);
            CreateManyToManyReferences(modelBuilder);
        }

        public virtual DbSet<Comment> Comments { get; set; }
        public virtual DbSet<Group> Groups { get; set; }
        public virtual DbSet<Plan> Plans { get; set; }
        public virtual DbSet<PlanTask> PlanTasks { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<Section> Sections { get; set; }
        public virtual DbSet<StudentTask> Tasks { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserTask> UserTasks { get; set; }
        public virtual DbSet<PlanSuggestion> PlanSuggestion { get; set; }
        public virtual DbSet<GroupPlanTask> GroupsPlansTasks { get; set; }
        public virtual DbSet<UserRole> UsersRoles { get; set; }
        public virtual DbSet<Message> Messages { get; set; }

        public virtual int sp_Total_Ammount_of_Users(ObjectParameter total)
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("sp_Total_Ammount_of_Users", total);
        }

        private void CreateUserReferences(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasKey(user => user.Id)
                .HasRequired(user => user.Role)
                .WithMany(role => role.Users)
                .HasForeignKey(user => user.RoleId)
                .WillCascadeOnDelete(false);
        }

        private void CreateCommentReferences(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Comment>()
                .HasKey(comment => comment.Id)
                .HasRequired(comment => comment.Creator)
                .WithMany(user => user.Comments)
                .HasForeignKey(comment => comment.CreateId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Comment>()
                .HasRequired(comment => comment.PlanTask)
                .WithMany(planTask => planTask.Comments)
                .HasForeignKey(comment => comment.PlanTaskId)
                .WillCascadeOnDelete(false);
        }

        private void CreateGroupReferences(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Group>()
                .HasKey(group => group.Id)
                .HasRequired(group => group.Mentor)
                .WithMany(mentor => mentor.GroupMentor)
                .HasForeignKey(group => group.MentorId)
                .WillCascadeOnDelete(false);
        }

        private void CreateMessageReferences(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Message>()
                .HasKey(message => message.Id)
                .HasRequired(message => message.Creator)
                .WithMany(user => user.Messages)
                .HasForeignKey(message => message.UserId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Message>()
                .HasRequired(message => message.UserTask)
                .WithMany(userTask => userTask.Messages)
                .HasForeignKey(message => message.UserTaskId)
                .WillCascadeOnDelete(false);
        }

        private void CreatePlanReferences(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Plan>()
                .HasKey(plan => plan.Id)
                .HasRequired(plan => plan.Creator)
                .WithMany(creator => creator.PlansCreated)
                .HasForeignKey(plan => plan.CreateId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Plan>()
                .HasOptional(plan => plan.Modifier)
                .WithMany(modifier => modifier.PlansModified)
                .HasForeignKey(plans => plans.ModId)
                .WillCascadeOnDelete(false);
        }

        private void CreatePlanSuggestionReferences(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PlanSuggestion>()
                .HasKey(planSug => planSug.Id)
                .HasRequired(planSug => planSug.Mentor)
                .WithMany(user => user.PlanSuggestionsMentor)
                .HasForeignKey(planSug => planSug.MentorId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<PlanSuggestion>()
                .HasRequired(planSug => planSug.User)
                .WithMany(user => user.PlanSuggestionsStudent)
                .HasForeignKey(planSug => planSug.UserId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<PlanSuggestion>()
                .HasRequired(planSug => planSug.Plan)
                .WithMany(plan => plan.PlanSuggestion)
                .HasForeignKey(planSug => planSug.PlanId)
                .WillCascadeOnDelete(false);
        }

        private void CreatePlanTaskReferences(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PlanTask>()
                .HasKey(planTask => planTask.Id)
                .HasRequired(planTask => planTask.Plans)
                .WithMany(plan => plan.PlanTasks)
                .HasForeignKey(planTask => planTask.PlanId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<PlanTask>()
                .HasRequired(planTask => planTask.Tasks)
                .WithMany(task => task.PlanTasks)
                .HasForeignKey(planTask => planTask.TaskId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<PlanTask>()
                .HasOptional(planTask => planTask.Sections)
                .WithMany(section => section.PlanTasks)
                .HasForeignKey(planTask => planTask.SectionId)
                .WillCascadeOnDelete(false);
        }

        private void CreateRoleReferences(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Role>()
                .HasKey(role => role.Id);
        }

        private void CreateSectionReferences(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Section>()
                .HasKey(section => section.Id);
        }

        private void CreateTaskReferences(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<StudentTask>()
                .HasKey(task => task.Id)
                .HasRequired(task => task.Creator)
                .WithMany(user => user.TasksCreated)
                .HasForeignKey(task => task.CreateId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<StudentTask>()
                .HasOptional(task => task.Modifier)
                .WithMany(user => user.TasksModified)
                .HasForeignKey(task => task.ModId)
                .WillCascadeOnDelete(false);
        }

        private void CreateUserTaskReferences(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserTask>()
                .HasKey(userTask => userTask.Id)
                .HasRequired(userTask => userTask.User)
                .WithMany(user => user.UserTasks)
                .HasForeignKey(userTask => userTask.UserId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<UserTask>()
                .HasRequired(userTask => userTask.Mentor)
                .WithMany(user => user.UserTaskMentor)
                .HasForeignKey(userTask => userTask.MentorId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<UserTask>()
                .HasRequired(userTask => userTask.PlanTask)
                .WithMany(planTask => planTask.UserTasks)
                .HasForeignKey(userTask => userTask.PlanTaskId)
                .WillCascadeOnDelete(false);
        }

        private void CreateManyToManyReferences(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasMany(user => user.Groups)
                .WithMany(group => group.Users)
                .Map(userGroups =>
                {
                    userGroups.MapLeftKey("UserId");
                    userGroups.MapRightKey("GroupId");
                    userGroups.ToTable("UserGroups");
                });

            modelBuilder.Entity<Group>()
                .HasMany(group => group.Plans)
                .WithMany(plan => plan.Groups)
                .Map(groupPlans =>
                {
                    groupPlans.MapLeftKey("GroupId");
                    groupPlans.MapRightKey("PlanId");
                    groupPlans.ToTable("GroupPlans");
                });
        }
    }
}