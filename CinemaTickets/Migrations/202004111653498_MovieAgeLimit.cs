namespace CinemaTickets.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MovieAgeLimit : DbMigration
    {
        public override void Up()
        {
            AddColumn(
                "dbo.Movies",
                "AgeLimit",
                c => c.Int()
                );
        }
        
        public override void Down()
        {
            DropColumn("dbo.Movies", "AgeLimit");
        }
    }
}
