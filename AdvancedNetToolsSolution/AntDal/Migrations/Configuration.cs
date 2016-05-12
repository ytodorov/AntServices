namespace AntDal.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    public sealed class Configuration : DbMigrationsConfiguration<AntDal.AntDbContext>
    {
        public Configuration()
        {
            // Тези двете обезмислят класовете от Migrations папката
            AutomaticMigrationDataLossAllowed = true;
            AutomaticMigrationsEnabled = true;

            ContextKey = "AntDal.AntDbContext";
        }

        protected override void Seed(AntDal.AntDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
        }
    }
}
