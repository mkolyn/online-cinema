namespace CinemaTickets.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OrderIsProcessing : DbMigration
    {
        public override void Up()
        {
            AddColumn(
                "dbo.Orders",
                "IsProcessing",
                c => c.Boolean()
                );
        }
        
        public override void Down()
        {
            DropColumn("dbo.Orders", "IsProcessing");
        }
    }
}
