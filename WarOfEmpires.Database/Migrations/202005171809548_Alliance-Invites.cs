namespace WarOfEmpires.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AllianceInvites : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Alliances.Invites",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        IsRead = c.Boolean(nullable: false),
                        Message = c.String(),
                        Player_Id = c.Int(nullable: false),
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
            DropForeignKey("Alliances.Invites", "Alliance_Id", "Alliances.Alliances");
            DropForeignKey("Alliances.Invites", "Player_Id", "Players.Players");
            DropIndex("Alliances.Invites", new[] { "Alliance_Id" });
            DropIndex("Alliances.Invites", new[] { "Player_Id" });
            DropTable("Alliances.Invites");
        }
    }
}
