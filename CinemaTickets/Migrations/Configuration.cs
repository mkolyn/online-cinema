namespace CinemaTickets.Migrations
{
    using System.Data.Entity.Migrations;
    using System.Data.Entity.Infrastructure;
    using CinemaTickets.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<CinemaTickets.Models.GenreContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "OnlineCinema.Models.GenreContext";
            CommandTimeout = 5;
            string connectionString = "Data Source=" + Config.migrationDbDataSource + ";";
            connectionString += "Initial Catalog=" + Config.migrationDbName + ";";
            connectionString += "User ID=" + Config.migrationDbUserId + ";";
            connectionString += "Password=" + Config.migrationDbUserPassword + ";";
            connectionString += "Connect Timeout=" + Config.migrationDbConnectTimeout + ";";
            TargetDatabase = new DbConnectionInfo(connectionString, "System.Data.SqlClient");
        }

        protected override void Seed(CinemaTickets.Models.GenreContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
        }
    }
}
