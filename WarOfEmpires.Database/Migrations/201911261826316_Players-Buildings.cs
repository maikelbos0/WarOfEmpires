namespace WarOfEmpires.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PlayersBuildings : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Empires.Buildings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Type = c.Int(nullable: false),
                        Level = c.Int(nullable: false),
                        Player_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("Players.Players", t => t.Player_Id, cascadeDelete: true)
                .ForeignKey("Empires.BuildingTypes", t => t.Type, cascadeDelete: true)
                .Index(t => t.Type)
                .Index(t => t.Player_Id);
            
            CreateTable(
                "Empires.BuildingTypes",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("Empires.Buildings", "Type", "Empires.BuildingTypes");
            DropForeignKey("Empires.Buildings", "Player_Id", "Players.Players");
            DropIndex("Empires.Buildings", new[] { "Player_Id" });
            DropIndex("Empires.Buildings", new[] { "Type" });
            DropTable("Empires.BuildingTypes");
            DropTable("Empires.Buildings");
        }
    }
}
