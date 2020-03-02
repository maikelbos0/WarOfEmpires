namespace WarOfEmpires.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EmpiresWorkers : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Empires.Workers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Type = c.Int(nullable: false),
                        Count = c.Int(nullable: false),
                        Player_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("Players.Players", t => t.Player_Id, cascadeDelete: true)
                .Index(t => t.Player_Id);
            
            DropColumn("Players.Players", "Farmers");
            DropColumn("Players.Players", "WoodWorkers");
            DropColumn("Players.Players", "StoneMasons");
            DropColumn("Players.Players", "OreMiners");
            DropColumn("Players.Players", "SiegeEngineers");
        }
        
        public override void Down()
        {
            AddColumn("Players.Players", "SiegeEngineers", c => c.Int(nullable: false));
            AddColumn("Players.Players", "OreMiners", c => c.Int(nullable: false));
            AddColumn("Players.Players", "StoneMasons", c => c.Int(nullable: false));
            AddColumn("Players.Players", "WoodWorkers", c => c.Int(nullable: false));
            AddColumn("Players.Players", "Farmers", c => c.Int(nullable: false));
            DropForeignKey("Empires.Workers", "Player_Id", "Players.Players");
            DropIndex("Empires.Workers", new[] { "Player_Id" });
            DropTable("Empires.Workers");
        }
    }
}
