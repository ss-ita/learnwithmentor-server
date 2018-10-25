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
            CreateUserGroupReferences(modelBuilder);

            CreateManyToManyReferences(modelBuilder);
        }

        public virtual DbSet<Comment> Comments { get; set; }
        public virtual DbSet<Group> Groups { get; set; }
        public virtual DbSet<Plan> Plans { get; set; }
        public virtual DbSet<PlanTask> PlanTasks { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<Section> Sections { get; set; }
        public virtual DbSet<Task> Tasks { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserTask> UserTasks { get; set; }
        public virtual DbSet<PlanSuggestion> PlanSuggestion { get; set; }
        public virtual DbSet<GROUP_PLAN_TASK> GROUPS_PLANS_TASKS { get; set; }
        public virtual DbSet<USER_ROLE> USERS_ROLES { get; set; }
        public virtual DbSet<Message> Messages { get; set; }
        public virtual DbSet<UserGroup> UserGroups { get; set; }

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
                .HasForeignKey(user => user.Role_Id)
                .WillCascadeOnDelete(false);
        }

        private void CreateCommentReferences(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Comment>()
                .HasKey(comment => comment.Id)
                .HasRequired(comment => comment.Creator)
                .WithMany(user => user.Comments)
                .HasForeignKey(comment => comment.Create_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Comment>()
                .HasRequired(comment => comment.PlanTask)
                .WithMany(planTask => planTask.Comments)
                .HasForeignKey(comment => comment.PlanTask_Id)
                .WillCascadeOnDelete(false);
        }

        private void CreateGroupReferences(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Group>()
                .HasKey(group => group.Id)
                .HasRequired(group => group.Mentor)
                .WithMany(mentor => mentor.GroupMentor)
                .HasForeignKey(group => group.Mentor_Id)
                .WillCascadeOnDelete(false);
        }

        private void CreateMessageReferences(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Message>()
                .HasKey(message => message.Id)
                .HasRequired(message => message.Creator)
                .WithMany(user => user.Messages)
                .HasForeignKey(message => message.User_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Message>()
                .HasRequired(message => message.UserTask)
                .WithMany(userTask => userTask.Messages)
                .HasForeignKey(message => message.UserTask_Id)
                .WillCascadeOnDelete(false);
        }

        private void CreatePlanReferences(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Plan>()
                .HasKey(plan => plan.Id)
                .HasRequired(plan => plan.Creator)
                .WithMany(creator => creator.PlansCreated)
                .HasForeignKey(plan => plan.Create_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Plan>()
                .HasOptional(plan => plan.Modifier)
                .WithMany(modifier => modifier.PlansModified)
                .HasForeignKey(plans => plans.Mod_Id)
                .WillCascadeOnDelete(false);
        }

        private void CreatePlanSuggestionReferences(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PlanSuggestion>()
                .HasKey(planSug => planSug.Id)
                .HasRequired(planSug => planSug.Mentor)
                .WithMany(user => user.PlanSuggestionsMentor)
                .HasForeignKey(planSug => planSug.Mentor_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<PlanSuggestion>()
                .HasRequired(planSug => planSug.User)
                .WithMany(user => user.PlanSuggestionsStudent)
                .HasForeignKey(planSug => planSug.User_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<PlanSuggestion>()
                .HasRequired(planSug => planSug.Plan)
                .WithMany(plan => plan.PlanSuggestion)
                .HasForeignKey(planSug => planSug.Plan_Id)
                .WillCascadeOnDelete(false);
        }

        private void CreatePlanTaskReferences(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PlanTask>()
                .HasKey(planTask => planTask.Id)
                .HasRequired(planTask => planTask.Plans)
                .WithMany(plan => plan.PlanTasks)
                .HasForeignKey(planTask => planTask.Plan_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<PlanTask>()
                .HasRequired(planTask => planTask.Tasks)
                .WithMany(task => task.PlanTasks)
                .HasForeignKey(planTask => planTask.Task_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<PlanTask>()
                .HasOptional(planTask => planTask.Sections)
                .WithMany(section => section.PlanTasks)
                .HasForeignKey(planTask => planTask.Section_Id)
                .WillCascadeOnDelete(false);
        }


        private void CreateUserGroupReferences(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserGroup>()
                .HasKey(userGroup => userGroup.Id)
                .HasRequired(userGroup => userGroup.User)
                .WithMany(plan => plan.UserGroups)
                .HasForeignKey(planTask => planTask.UserId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<UserGroup>()
                .HasRequired(planTask => planTask.Group)
                .WithMany(task => task.UserGroups)
                .HasForeignKey(planTask => planTask.GroupId)
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
            modelBuilder.Entity<Task>()
                .HasKey(task => task.Id)
                .HasRequired(task => task.Creator)
                .WithMany(user => user.TasksCreated)
                .HasForeignKey(task => task.Create_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Task>()
                .HasOptional(task => task.Modifier)
                .WithMany(user => user.TasksModified)
                .HasForeignKey(task => task.Mod_Id)
                .WillCascadeOnDelete(false);
        }

        private void CreateUserTaskReferences(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserTask>()
                .HasKey(userTask => userTask.Id)
                .HasRequired(userTask => userTask.User)
                .WithMany(user => user.UserTasks)
                .HasForeignKey(userTask => userTask.User_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<UserTask>()
                .HasRequired(userTask => userTask.Mentor)
                .WithMany(user => user.UserTaskMentor)
                .HasForeignKey(userTask => userTask.Mentor_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<UserTask>()
                .HasRequired(userTask => userTask.PlanTask)
                .WithMany(planTask => planTask.UserTasks)
                .HasForeignKey(userTask => userTask.PlanTask_Id)
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