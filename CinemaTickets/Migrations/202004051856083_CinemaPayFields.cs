namespace CinemaTickets.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CinemaPayFields : DbMigration
    {
        public override void Up()
        {
            AddColumn(
                "dbo.Cinemas",
                "LiqpayPublicKey",
                c => c.String()
                );

            AddColumn(
                "dbo.Cinemas",
                "LiqpayPrivateKey",
                c => c.String()
                );

            AddColumn(
                "dbo.Cinemas",
                "IsTestPayment",
                c => c.Boolean()
                );
        }
        
        public override void Down()
        {
            DropColumn("dbo.Cinemas", "LiqpayPublicKey");
            DropColumn("dbo.Cinemas", "LiqpayPrivateKey");
            DropColumn("dbo.Cinemas", "IsTestPayment");
        }
    }
}
