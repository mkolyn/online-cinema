namespace OnlineCinema.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OrderItemPrice : DbMigration
    {
        public override void Up()
        {
            AddColumn(
                "dbo.OrderItems",
                "Price",
                c => c.Int()
                );
        }
        
        public override void Down()
        {
            DropColumn("dbo.OrderItems", "Price");
        }
    }
}
