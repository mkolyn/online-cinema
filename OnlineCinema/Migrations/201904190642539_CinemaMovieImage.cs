namespace OnlineCinema.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CinemaMovieImage : DbMigration
    {
        public override void Up()
        {
            AddColumn(
                "dbo.CinemaMovies",
                "Image",
                c => c.String()
                );
        }
        
        public override void Down()
        {
            DropColumn("dbo.CinemaMovies", "Image");
        }
    }
}
