namespace OnlineCinema.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MoviePrice : DbMigration
    {
        public override void Up()
        {
            AddColumn(
                "dbo.CinemaMovies",
                "Price",
                c => c.Int()
                );
        }
        
        public override void Down()
        {
            DropColumn("dbo.CinemaMovies", "Price");
        }
    }
}
