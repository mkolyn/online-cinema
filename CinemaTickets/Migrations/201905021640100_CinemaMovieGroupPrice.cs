namespace CinemaTickets.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CinemaMovieGroupPrice : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CinemaMovieGroupPrices",
                c => new
                {
                    ID = c.Int(nullable: false, identity: true),
                    CinemaMovieID = c.Int(),
                    CinemaPlaceGroupID = c.Int(),
                    Price = c.Int(),
                })
                .PrimaryKey(t => t.ID);

            AddForeignKey("dbo.CinemaMovieGroupPrices", "CinemaMovieID", "dbo.CinemaMovies", "ID");
            AddForeignKey("dbo.CinemaMovieGroupPrices", "CinemaPlaceGroupID", "dbo.CinemaPlaceGroups", "ID");
            CreateIndex("dbo.CinemaMovieGroupPrices", "CinemaMovieID");
            CreateIndex("dbo.CinemaMovieGroupPrices", "CinemaPlaceGroupID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CinemaMovieGroupPrices", "CinemaMovieID", "dbo.CinemaMovies");
            DropForeignKey("dbo.CinemaMovieGroupPrices", "CinemaPlaceGroupID", "dbo.CinemaPlaceGroups");
            DropIndex("dbo.CinemaMovieGroupPrices", new string[] { "CinemaMovieID" });
            DropIndex("dbo.CinemaMovieGroupPrices", new string[] { "CinemaPlaceGroupID" });
            DropTable("dbo.CinemaMovieGroupPrices");
        }
    }
}
