namespace OnlineCinema.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CinemaHallPlaces : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CinemaHallPlaces",
                c => new
                {
                    ID = c.Int(nullable: false, identity: true),
                    CinemaHallID = c.Int(nullable: false),
                    row = c.Int(nullable: false),
                    cell = c.Int(nullable: false),
                    rows = c.Int(),
                    cells = c.Int(),
                })
                .PrimaryKey(t => t.ID);
        }
        
        public override void Down()
        {
            DropTable("dbo.CinemaHallPlaces");
        }
    }
}
