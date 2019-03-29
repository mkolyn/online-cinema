namespace OnlineCinema.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Movie : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Movies",
                c => new
                {
                    ID = c.Int(nullable: false, identity: true),
                    GenreID = c.Int(nullable: false),
                    Name = c.String(),
                    Duration = c.Int(nullable: false),
                    Description = c.String(),
                })
                .PrimaryKey(t => t.ID);
        }
        
        public override void Down()
        {
            DropTable("dbo.Movies");
        }
    }
}
