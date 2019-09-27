namespace CinemaTickets.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Order : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Orders",
                c => new
                {
                    ID = c.Int(nullable: false, identity: true),
                    Date = c.DateTime(nullable: false),
                    IsPaid = c.Boolean(),
                })
                .PrimaryKey(t => t.ID);

            CreateTable(
                "dbo.OrderItems",
                c => new
                {
                    ID = c.Int(nullable: false, identity: true),
                    OrderID = c.Int(nullable: false),
                    CinemaHallMovieID = c.Int(nullable: false),
                    CinemaHallPlaceID = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.ID);
        }
        
        public override void Down()
        {
            DropTable("dbo.Orders");
            DropTable("dbo.OrderItems");
        }
    }
}
