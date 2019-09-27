namespace CinemaTickets.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using System.Data.Entity.Infrastructure;

    internal sealed class Configuration : DbMigrationsConfiguration<CinemaTickets.Models.GenreContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "OnlineCinema.Models.GenreContext";
            CommandTimeout = 5;
            //TargetDatabase = new DbConnectionInfo("Data Source=MKOLYN\\MSSQLSERVER3;Initial Catalog=onlinecinema;User ID=mkolyn;Password=111111;Connect Timeout=10;", "System.Data.SqlClient");
            TargetDatabase = new DbConnectionInfo("Data Source=DESKTOP-NVEBCLL;Initial Catalog=CinemaTickets;User ID=mkolyn_admin;Password=223973;Connect Timeout=10;", "System.Data.SqlClient");
        }

        protected override void Seed(CinemaTickets.Models.GenreContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
        }
    }
}
