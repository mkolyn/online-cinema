namespace CinemaTickets.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LiqpayResultChangeAmountType : DbMigration
    {
        public override void Up()
        {
            AlterColumn(
                "dbo.LiqpayResults",
                "Amount",
                c => c.Double()
                );
        }
        
        public override void Down()
        {
            AlterColumn(
                "dbo.LiqpayResults",
                "Amount",
                c => c.Int()
                );
        }
    }
}
