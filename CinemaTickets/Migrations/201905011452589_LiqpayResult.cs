namespace CinemaTickets.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LiqpayResult : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.LiqpayResults",
                c => new
                {
                    ID = c.Int(nullable: false, identity: true),
                    LiqpayOrderId = c.String(),
                    OrderId = c.String(),
                    PaymentId = c.Int(),
                    Amount = c.Int(),
                    Description = c.String(),
                    ErrorCode = c.String(),
                    ErrorDescription = c.String(),
                    Status = c.String(),
                    Date = c.DateTime(nullable: false),
                })
                .PrimaryKey(t => t.ID);
        }
        
        public override void Down()
        {
            DropTable("dbo.LiqpayResults");
        }
    }
}
