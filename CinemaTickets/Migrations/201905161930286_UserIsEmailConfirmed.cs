namespace CinemaTickets.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserIsEmailConfirmed : DbMigration
    {
        public override void Up()
        {
            AddColumn(
                "dbo.Users",
                "IsEmailConfirmed",
                c => c.Boolean()
                );
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "IsEmailConfirmed");
        }
    }
}
