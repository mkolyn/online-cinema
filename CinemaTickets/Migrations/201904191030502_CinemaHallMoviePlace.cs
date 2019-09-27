namespace CinemaTickets.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CinemaHallMoviePlace : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CinemaHallMoviePlaces",
                c => new
                {
                    ID = c.Int(nullable: false, identity: true),
                    CinemaHallMovieID = c.Int(nullable: false),
                    CinemaHallPlaceID = c.Int(nullable: false),
                    Status = c.Int(),
                })
                .PrimaryKey(t => t.ID);
        }
        
        public override void Down()
        {
            DropTable("dbo.CinemaHallMoviePlaces");
        }
    }
}
