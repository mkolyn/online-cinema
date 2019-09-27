namespace CinemaTickets.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CinemaHallMovie : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CinemaHallMovies",
                c => new
                {
                    ID = c.Int(nullable: false, identity: true),
                    CinemaHallID = c.Int(nullable: false),
                    MovieId = c.Int(nullable: false),
                    Date = c.DateTime(nullable: false),
                })
                .PrimaryKey(t => t.ID);
        }
        
        public override void Down()
        {
            DropTable("dbo.CinemaHallMovies");
        }
    }
}
