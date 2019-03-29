namespace OnlineCinema.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CinemaHall : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CinemaHalls",
                c => new
                {
                    ID = c.Int(nullable: false, identity: true),
                    CinemaID = c.Int(nullable: false),
                    Name = c.String(),
                })
                .PrimaryKey(t => t.ID);
        }
        
        public override void Down()
        {
            DropTable("dbo.CinemaHalls");
        }
    }
}
