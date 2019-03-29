namespace OnlineCinema.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using System.Data.Entity.Infrastructure;

    internal sealed class Configuration : DbMigrationsConfiguration<OnlineCinema.Models.GenreContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "OnlineCinema.Models.GenreContext";
            CommandTimeout = 5;
            TargetDatabase = new DbConnectionInfo("Data Source=localhost;Initial Catalog=onlinecinema;User ID=mkolyn;Password=223973;Connect Timeout=10;", "System.Data.SqlClient");
        }

        protected override void Seed(OnlineCinema.Models.GenreContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
        }
    }
}
