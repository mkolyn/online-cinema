namespace CinemaTickets.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MovieFields : DbMigration
    {
        public override void Up()
        {
            AddColumn(
                "dbo.Movies",
                "Country",
                c => c.String()
                );

            AddColumn(
                "dbo.Movies",
                "Year",
                c => c.Int()
                );

            AddColumn(
                "dbo.Movies",
                "Director",
                c => c.String()
                );

            AddColumn(
                "dbo.Movies",
                "Cast",
                c => c.String()
                );

            AddColumn(
                "dbo.Movies",
                "Budget",
                c => c.Int()
                );
        }
        
        public override void Down()
        {
            DropColumn("dbo.Movies", "Country");
            DropColumn("dbo.Movies", "Year");
            DropColumn("dbo.Movies", "Director");
            DropColumn("dbo.Movies", "Cast");
            DropColumn("dbo.Movies", "Budget");
        }
    }
}
