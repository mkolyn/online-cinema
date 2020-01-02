namespace CinemaTickets.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MovieImage : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.CinemaMovies", "Image");
            AddColumn(
                "dbo.Movies",
                "Image",
                c => c.String()
                );
        }
        
        public override void Down()
        {
            DropColumn("dbo.Movies", "Image");
            AddColumn(
                "dbo.CinemaMovies",
                "Image",
                c => c.String()
                );
        }
    }
}
