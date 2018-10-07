namespace LearnWithMentorDAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Comments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PlanTask_Id = c.Int(nullable: false),
                        Text = c.String(),
                        Create_Id = c.Int(nullable: false),
                        Create_Date = c.DateTime(),
                        Mod_Date = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.Create_Id)
                .ForeignKey("dbo.PlanTasks", t => t.PlanTask_Id)
                .Index(t => t.PlanTask_Id)
                .Index(t => t.Create_Id);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FirstName = c.String(),
                        LastName = c.String(),
                        Email = c.String(),
                        Password = c.String(),
                        Blocked = c.Boolean(nullable: false),
                        Image = c.String(),
                        Image_Name = c.String(),
                        Email_Confirmed = c.Boolean(nullable: false),
                        Role_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Roles", t => t.Role_Id)
                .Index(t => t.Role_Id);
            
            CreateTable(
                "dbo.Groups",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Mentor_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.Mentor_Id)
                .Index(t => t.Mentor_Id);
            
            CreateTable(
                "dbo.Plans",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        Published = c.Boolean(nullable: false),
                        Create_Id = c.Int(nullable: false),
                        Mod_Id = c.Int(),
                        Create_Date = c.DateTime(),
                        Mod_Date = c.DateTime(),
                        Image = c.String(),
                        Image_Name = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.Create_Id)
                .ForeignKey("dbo.Users", t => t.Mod_Id)
                .Index(t => t.Create_Id)
                .Index(t => t.Mod_Id);
            
            CreateTable(
                "dbo.PlanSuggestions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Plan_Id = c.Int(nullable: false),
                        User_Id = c.Int(nullable: false),
                        Mentor_Id = c.Int(nullable: false),
                        Text = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.Mentor_Id)
                .ForeignKey("dbo.Plans", t => t.Plan_Id)
                .ForeignKey("dbo.Users", t => t.User_Id)
                .Index(t => t.Plan_Id)
                .Index(t => t.User_Id)
                .Index(t => t.Mentor_Id);
            
            CreateTable(
                "dbo.PlanTasks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Plan_Id = c.Int(nullable: false),
                        Task_Id = c.Int(nullable: false),
                        Priority = c.Int(),
                        Section_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Plans", t => t.Plan_Id)
                .ForeignKey("dbo.Sections", t => t.Section_Id)
                .ForeignKey("dbo.Tasks", t => t.Task_Id)
                .Index(t => t.Plan_Id)
                .Index(t => t.Task_Id)
                .Index(t => t.Section_Id);
            
            CreateTable(
                "dbo.Sections",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Tasks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        Private = c.Boolean(nullable: false),
                        Create_Id = c.Int(nullable: false),
                        Mod_Id = c.Int(),
                        Create_Date = c.DateTime(),
                        Mod_Date = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.Create_Id)
                .ForeignKey("dbo.Users", t => t.Mod_Id)
                .Index(t => t.Create_Id)
                .Index(t => t.Mod_Id);
            
            CreateTable(
                "dbo.UserTasks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        User_Id = c.Int(nullable: false),
                        PlanTask_Id = c.Int(nullable: false),
                        State = c.String(),
                        End_Date = c.DateTime(),
                        Result = c.String(),
                        Propose_End_Date = c.DateTime(),
                        Mentor_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.Mentor_Id)
                .ForeignKey("dbo.PlanTasks", t => t.PlanTask_Id)
                .ForeignKey("dbo.Users", t => t.User_Id)
                .Index(t => t.User_Id)
                .Index(t => t.PlanTask_Id)
                .Index(t => t.Mentor_Id);
            
            CreateTable(
                "dbo.Messages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserTask_Id = c.Int(nullable: false),
                        User_Id = c.Int(nullable: false),
                        Text = c.String(),
                        Send_Time = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.User_Id)
                .ForeignKey("dbo.UserTasks", t => t.UserTask_Id)
                .Index(t => t.UserTask_Id)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.GROUP_PLAN_TASK",
                c => new
                    {
                        Name = c.String(nullable: false, maxLength: 128),
                        Description = c.String(),
                        Create_Date = c.DateTime(),
                        Mod_Date = c.DateTime(),
                        Published = c.Boolean(nullable: false),
                        Priority = c.Int(),
                        Section_Name = c.String(),
                        Task_Name = c.String(),
                        Task_Description = c.String(),
                        Tasks_Create_Date = c.DateTime(),
                        Task_Mod_Date = c.DateTime(),
                        Private = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Name);
            
            CreateTable(
                "dbo.USER_ROLE",
                c => new
                    {
                        FirstName = c.String(nullable: false, maxLength: 128),
                        LastName = c.String(),
                        Roles_Name = c.String(),
                        Email = c.String(),
                    })
                .PrimaryKey(t => t.FirstName);
            
            CreateTable(
                "dbo.GroupPlan",
                c => new
                    {
                        GroupId = c.Int(nullable: false),
                        PlanId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.GroupId, t.PlanId })
                .ForeignKey("dbo.Groups", t => t.GroupId, cascadeDelete: true)
                .ForeignKey("dbo.Plans", t => t.PlanId, cascadeDelete: true)
                .Index(t => t.GroupId)
                .Index(t => t.PlanId);
            
            CreateTable(
                "dbo.UserGroup",
                c => new
                    {
                        UserId = c.Int(nullable: false),
                        GroupId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserId, t.GroupId })
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.Groups", t => t.GroupId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.GroupId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Comments", "PlanTask_Id", "dbo.PlanTasks");
            DropForeignKey("dbo.Comments", "Create_Id", "dbo.Users");
            DropForeignKey("dbo.Users", "Role_Id", "dbo.Roles");
            DropForeignKey("dbo.UserGroup", "GroupId", "dbo.Groups");
            DropForeignKey("dbo.UserGroup", "UserId", "dbo.Users");
            DropForeignKey("dbo.GroupPlan", "PlanId", "dbo.Plans");
            DropForeignKey("dbo.GroupPlan", "GroupId", "dbo.Groups");
            DropForeignKey("dbo.UserTasks", "User_Id", "dbo.Users");
            DropForeignKey("dbo.UserTasks", "PlanTask_Id", "dbo.PlanTasks");
            DropForeignKey("dbo.Messages", "UserTask_Id", "dbo.UserTasks");
            DropForeignKey("dbo.Messages", "User_Id", "dbo.Users");
            DropForeignKey("dbo.UserTasks", "Mentor_Id", "dbo.Users");
            DropForeignKey("dbo.PlanTasks", "Task_Id", "dbo.Tasks");
            DropForeignKey("dbo.Tasks", "Mod_Id", "dbo.Users");
            DropForeignKey("dbo.Tasks", "Create_Id", "dbo.Users");
            DropForeignKey("dbo.PlanTasks", "Section_Id", "dbo.Sections");
            DropForeignKey("dbo.PlanTasks", "Plan_Id", "dbo.Plans");
            DropForeignKey("dbo.PlanSuggestions", "User_Id", "dbo.Users");
            DropForeignKey("dbo.PlanSuggestions", "Plan_Id", "dbo.Plans");
            DropForeignKey("dbo.PlanSuggestions", "Mentor_Id", "dbo.Users");
            DropForeignKey("dbo.Plans", "Mod_Id", "dbo.Users");
            DropForeignKey("dbo.Plans", "Create_Id", "dbo.Users");
            DropForeignKey("dbo.Groups", "Mentor_Id", "dbo.Users");
            DropIndex("dbo.UserGroup", new[] { "GroupId" });
            DropIndex("dbo.UserGroup", new[] { "UserId" });
            DropIndex("dbo.GroupPlan", new[] { "PlanId" });
            DropIndex("dbo.GroupPlan", new[] { "GroupId" });
            DropIndex("dbo.Messages", new[] { "User_Id" });
            DropIndex("dbo.Messages", new[] { "UserTask_Id" });
            DropIndex("dbo.UserTasks", new[] { "Mentor_Id" });
            DropIndex("dbo.UserTasks", new[] { "PlanTask_Id" });
            DropIndex("dbo.UserTasks", new[] { "User_Id" });
            DropIndex("dbo.Tasks", new[] { "Mod_Id" });
            DropIndex("dbo.Tasks", new[] { "Create_Id" });
            DropIndex("dbo.PlanTasks", new[] { "Section_Id" });
            DropIndex("dbo.PlanTasks", new[] { "Task_Id" });
            DropIndex("dbo.PlanTasks", new[] { "Plan_Id" });
            DropIndex("dbo.PlanSuggestions", new[] { "Mentor_Id" });
            DropIndex("dbo.PlanSuggestions", new[] { "User_Id" });
            DropIndex("dbo.PlanSuggestions", new[] { "Plan_Id" });
            DropIndex("dbo.Plans", new[] { "Mod_Id" });
            DropIndex("dbo.Plans", new[] { "Create_Id" });
            DropIndex("dbo.Groups", new[] { "Mentor_Id" });
            DropIndex("dbo.Users", new[] { "Role_Id" });
            DropIndex("dbo.Comments", new[] { "Create_Id" });
            DropIndex("dbo.Comments", new[] { "PlanTask_Id" });
            DropTable("dbo.UserGroup");
            DropTable("dbo.GroupPlan");
            DropTable("dbo.USER_ROLE");
            DropTable("dbo.GROUP_PLAN_TASK");
            DropTable("dbo.Roles");
            DropTable("dbo.Messages");
            DropTable("dbo.UserTasks");
            DropTable("dbo.Tasks");
            DropTable("dbo.Sections");
            DropTable("dbo.PlanTasks");
            DropTable("dbo.PlanSuggestions");
            DropTable("dbo.Plans");
            DropTable("dbo.Groups");
            DropTable("dbo.Users");
            DropTable("dbo.Comments");
        }
    }
}
