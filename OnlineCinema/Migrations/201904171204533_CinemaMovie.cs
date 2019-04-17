namespace OnlineCinema.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CinemaMovie : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CinemaMovies",
                c => new
                {
                    ID = c.Int(nullable: false, identity: true),
                    CinemaID = c.Int(nullable: false),
                    MovieID = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.ID);
        }
        
        public override void Down()
        {
            DropTable("dbo.CinemaMovies");
        }
    }
}
