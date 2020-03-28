namespace CinemaTickets.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CinemaMovieIs3D : DbMigration
    {
        public override void Up()
        {
            AddColumn(
                "dbo.CinemaMovies",
                "Is3D",
                c => c.Boolean()
                );
        }
        
        public override void Down()
        {
            DropColumn("dbo.CinemaMovies", "Is3D");
        }
    }
}
