namespace WarOfEmpires.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PlayersPeasantsResources : DbMigration
    {
        public override void Up()
        {
            AddColumn("Players.Players", "Farmers", c => c.Int(nullable: false));
            AddColumn("Players.Players", "WoodWorkers", c => c.Int(nullable: false));
            AddColumn("Players.Players", "StoneMasons", c => c.Int(nullable: false));
            AddColumn("Players.Players", "OreMiners", c => c.Int(nullable: false));
            AddColumn("Players.Players", "Gold", c => c.Int(nullable: false));
            AddColumn("Players.Players", "Food", c => c.Int(nullable: false));
            AddColumn("Players.Players", "Wood", c => c.Int(nullable: false));
            AddColumn("Players.Players", "Stone", c => c.Int(nullable: false));
            AddColumn("Players.Players", "Ore", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("Players.Players", "Ore");
            DropColumn("Players.Players", "Stone");
            DropColumn("Players.Players", "Wood");
            DropColumn("Players.Players", "Food");
            DropColumn("Players.Players", "Gold");
            DropColumn("Players.Players", "OreMiners");
            DropColumn("Players.Players", "StoneMasons");
            DropColumn("Players.Players", "WoodWorkers");
            DropColumn("Players.Players", "Farmers");
        }
    }
}
