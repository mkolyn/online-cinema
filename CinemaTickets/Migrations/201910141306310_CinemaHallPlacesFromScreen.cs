namespace CinemaTickets.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CinemaHallPlacesFromScreen : DbMigration
    {
        public override void Up()
        {
            AddColumn(
                "dbo.CinemaHalls",
                "IsPlacesFromScreen",
                c => c.Boolean()
                );
        }
        
        public override void Down()
        {
            DropColumn("dbo.CinemaHalls", "IsPlacesFromScreen");
        }
    }
}
