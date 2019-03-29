namespace OnlineCinema.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Cinema : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Cinemas",
                c => new
                {
                    ID = c.Int(nullable: false, identity: true),
                    Name = c.String(),
                })
                .PrimaryKey(t => t.ID);
        }
        
        public override void Down()
        {
            DropTable("dbo.Cinemas");
        }
    }
}
