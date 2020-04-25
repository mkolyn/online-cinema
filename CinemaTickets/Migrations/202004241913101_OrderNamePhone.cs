namespace CinemaTickets.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OrderNamePhone : DbMigration
    {
        public override void Up()
        {
            AddColumn(
                "dbo.Orders",
                "Name",
                c => c.String()
                );

            AddColumn(
                "dbo.Orders",
                "Phone",
                c => c.String()
                );
        }
        
        public override void Down()
        {
            DropColumn("dbo.Orders", "Name");
            DropColumn("dbo.Orders", "Phone");
        }
    }
}
