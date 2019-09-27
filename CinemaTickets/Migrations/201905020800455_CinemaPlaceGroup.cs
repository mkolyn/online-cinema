namespace CinemaTickets.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CinemaPlaceGroup : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CinemaPlaceGroups",
                c => new
                {
                    ID = c.Int(nullable: false, identity: true),
                    CinemaID = c.Int(),
                    Name = c.String(),
                })
                .PrimaryKey(t => t.ID);

            CreateTable(
                "dbo.CinemaHallPlaceGroups",
                c => new
                {
                    ID = c.Int(nullable: false, identity: true),
                    CinemaHallPlaceID = c.Int(),
                    CinemaPlaceGroupID = c.Int(),
                })
                .PrimaryKey(t => t.ID);

            AddForeignKey("dbo.CinemaHallPlaceGroups", "CinemaHallPlaceID", "dbo.CinemaHallPlaces", "ID");
            AddForeignKey("dbo.CinemaHallPlaceGroups", "CinemaPlaceGroupID", "dbo.CinemaPlaceGroups", "ID");
            AddForeignKey("dbo.CinemaPlaceGroups", "CinemaID", "dbo.Cinemas", "ID");
            CreateIndex("dbo.CinemaHallPlaceGroups", "CinemaHallPlaceID");
            CreateIndex("dbo.CinemaHallPlaceGroups", "CinemaPlaceGroupID");
            CreateIndex("dbo.CinemaPlaceGroups", "CinemaID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CinemaHallPlaceGroups", "CinemaHallPlaceID", "dbo.CinemaHallPlaces");
            DropForeignKey("dbo.CinemaHallPlaceGroups", "CinemaPlaceGroupID", "dbo.CinemaPlaceGroups");
            DropForeignKey("dbo.CinemaPlaceGroups", "CinemaID", "dbo.Cinemas");
            DropIndex("dbo.CinemaHallPlaceGroups", new string[] { "CinemaHallPlaceID" });
            DropIndex("dbo.CinemaHallPlaceGroups", new string[] { "CinemaPlaceGroupID" });
            DropIndex("dbo.CinemaPlaceGroups", new string[] { "CinemaID" });
            DropTable("dbo.CinemaPlaceGroups");
            DropTable("dbo.CinemaHallPlaceGroups");
        }
    }
}
