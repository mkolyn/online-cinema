namespace OnlineCinema.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserCinema : DbMigration
    {
        public override void Up()
        {
            AddColumn(
                "dbo.Users",
                "CinemaID",
                c => c.Int()
                );
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "CinemaID");
        }
    }
}
