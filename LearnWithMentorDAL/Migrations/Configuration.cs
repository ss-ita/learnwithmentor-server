namespace LearnWithMentorDAL.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<LearnWithMentorDAL.Entities.LearnWithMentorContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "LearnWithMentorDAL.Entities.LearnWithMentorContext";
        }

        protected override void Seed(LearnWithMentorDAL.Entities.LearnWithMentorContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
        }
    }
}
