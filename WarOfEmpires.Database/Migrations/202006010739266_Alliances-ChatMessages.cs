namespace WarOfEmpires.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlliancesChatMessages : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Alliances.ChatMessages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        Message = c.String(),
                        Player_Id = c.Int(),
                        Alliance_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("Players.Players", t => t.Player_Id)
                .ForeignKey("Alliances.Alliances", t => t.Alliance_Id, cascadeDelete: true)
                .Index(t => t.Player_Id)
                .Index(t => t.Alliance_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("Alliances.ChatMessages", "Alliance_Id", "Alliances.Alliances");
            DropForeignKey("Alliances.ChatMessages", "Player_Id", "Players.Players");
            DropIndex("Alliances.ChatMessages", new[] { "Alliance_Id" });
            DropIndex("Alliances.ChatMessages", new[] { "Player_Id" });
            DropTable("Alliances.ChatMessages");
        }
    }
}
