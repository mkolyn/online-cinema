namespace CinemaTickets.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CinemaCity : DbMigration
    {
        public override void Up()
        {
            AddColumn(
                "dbo.Cinemas",
                "CityID",
                c => c.Int()
                );
        }
        
        public override void Down()
        {
            DropColumn("dbo.Cinemas", "CityID");
        }
    }
}
