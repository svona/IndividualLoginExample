namespace IndividualLoginExample.Migrations
{
    using System.Data.Entity.Migrations;
    using BizObjects.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<IndividualLoginExample.BizObjects.MyDBContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(IndividualLoginExample.BizObjects.MyDBContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //context.People.AddOrUpdate(
            //  p => p.FullName,
            //  new Person { FullName = "Andrew Peters" },
            //  new Person { FullName = "Brice Lambson" },
            //  new Person { FullName = "Rowan Miller" }
            //);
            //

            context.Add(new BizObject { Name = "User" });
            context.Add(new BizObject { Name = "Role" });
            context.Add(new BizObject { Name = "UserRole" });
            context.Add(new BizObject { Name = "BizObject" });
            context.SaveChanges();
        }
    }
}
