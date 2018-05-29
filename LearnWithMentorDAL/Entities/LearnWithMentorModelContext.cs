namespace LearnWithMentorDAL.Entities
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class LearnWithMentorModelContext : DbContext
    {
        public LearnWithMentorModelContext()
            : base("name=LearnWithMentorModelContext")
        {
        }

        public virtual DbSet<Comments> Comments { get; set; }
        public virtual DbSet<Groups> Groups { get; set; }
        public virtual DbSet<Messages> Messages { get; set; }
        public virtual DbSet<Plans> Plans { get; set; }
        public virtual DbSet<Roles> Roles { get; set; }
        public virtual DbSet<Sections> Sections { get; set; }
        public virtual DbSet<Tasks> Tasks { get; set; }
        public virtual DbSet<Users> Users { get; set; }
        public virtual DbSet<UserTasks> UserTasks { get; set; }
        public virtual DbSet<PlanSuggestion> PlanSuggestion { get; set; }
        public virtual DbSet<PlanTasks> PlanTasks { get; set; }
        public virtual DbSet<GROUPS_PLANS_TASKS> GROUPS_PLANS_TASKS { get; set; }
        public virtual DbSet<UERS_ROLES> UERS_ROLES { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Groups>()
                .HasMany(e => e.Plans)
                .WithMany(e => e.Groups)
                .Map(m => m.ToTable("GroupPlans").MapLeftKey("Group_Id").MapRightKey("Plan_Id"));

            modelBuilder.Entity<Groups>()
                .HasMany(e => e.Users1)
                .WithMany(e => e.Groups1)
                .Map(m => m.ToTable("UserGroups").MapLeftKey("Group_Id").MapRightKey("User_Id"));

            modelBuilder.Entity<Messages>()
                .HasMany(e => e.UserTasks)
                .WithMany(e => e.Messages)
                .Map(m => m.ToTable("UserTasksMessages").MapLeftKey("Message_Id").MapRightKey("Usertask_Id"));

            modelBuilder.Entity<Plans>()
                .HasMany(e => e.PlanSuggestion)
                .WithRequired(e => e.Plans)
                .HasForeignKey(e => e.Plan_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Plans>()
                .HasMany(e => e.PlanTasks)
                .WithRequired(e => e.Plans)
                .HasForeignKey(e => e.Plan_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Roles>()
                .HasMany(e => e.Users)
                .WithRequired(e => e.Roles)
                .HasForeignKey(e => e.Role_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Sections>()
                .HasMany(e => e.PlanTasks)
                .WithOptional(e => e.Sections)
                .HasForeignKey(e => e.Section_Id);

            modelBuilder.Entity<Tasks>()
                .HasMany(e => e.Comments)
                .WithRequired(e => e.Tasks)
                .HasForeignKey(e => e.Task_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Tasks>()
                .HasMany(e => e.PlanTasks)
                .WithRequired(e => e.Tasks)
                .HasForeignKey(e => e.Task_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Tasks>()
                .HasMany(e => e.UserTasks)
                .WithRequired(e => e.Tasks)
                .HasForeignKey(e => e.Task_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Users>()
                .HasMany(e => e.Comments)
                .WithRequired(e => e.Users)
                .HasForeignKey(e => e.Create_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Users>()
                .HasMany(e => e.Groups)
                .WithOptional(e => e.Users)
                .HasForeignKey(e => e.Mentor_Id);

            modelBuilder.Entity<Users>()
                .HasMany(e => e.Messages)
                .WithRequired(e => e.Users)
                .HasForeignKey(e => e.User_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Users>()
                .HasMany(e => e.Plans)
                .WithRequired(e => e.Users)
                .HasForeignKey(e => e.Create_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Users>()
                .HasMany(e => e.Plans1)
                .WithOptional(e => e.Users1)
                .HasForeignKey(e => e.Mod_Id);

            modelBuilder.Entity<Users>()
                .HasMany(e => e.Tasks)
                .WithRequired(e => e.Users)
                .HasForeignKey(e => e.Create_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Users>()
                .HasMany(e => e.Tasks1)
                .WithOptional(e => e.Users1)
                .HasForeignKey(e => e.Mod_Id);

            modelBuilder.Entity<Users>()
                .HasMany(e => e.PlanSuggestion)
                .WithRequired(e => e.Users)
                .HasForeignKey(e => e.User_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Users>()
                .HasMany(e => e.PlanSuggestion1)
                .WithRequired(e => e.Users1)
                .HasForeignKey(e => e.Mentor_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Users>()
                .HasMany(e => e.UserTasks)
                .WithRequired(e => e.Users)
                .HasForeignKey(e => e.User_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<UserTasks>()
                .Property(e => e.State)
                .IsFixedLength();
        }
    }
}
