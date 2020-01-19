namespace CinemaTickets.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MovieYoutubeUrl : DbMigration
    {
        public override void Up()
        {
            AddColumn(
                "dbo.Movies",
                "YoutubeUrl",
                c => c.String()
                );
        }
        
        public override void Down()
        {
            DropColumn("dbo.Movies", "YoutubeUrl");
        }
    }
}
