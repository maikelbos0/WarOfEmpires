namespace WarOfEmpires.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Players : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Players.Players",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        DisplayName = c.String(nullable: false, maxLength: 25),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("Security.Users", t => t.Id)
                .Index(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("Players.Players", "Id", "Security.Users");
            DropIndex("Players.Players", new[] { "Id" });
            DropTable("Players.Players");
        }
    }
}
