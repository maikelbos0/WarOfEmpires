namespace WarOfEmpires.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ResourcesLong : DbMigration
    {
        public override void Up()
        {
            AlterColumn("Players.Players", "Gold", c => c.Long(nullable: false));
            AlterColumn("Players.Players", "Food", c => c.Long(nullable: false));
            AlterColumn("Players.Players", "Wood", c => c.Long(nullable: false));
            AlterColumn("Players.Players", "Stone", c => c.Long(nullable: false));
            AlterColumn("Players.Players", "Ore", c => c.Long(nullable: false));
            AlterColumn("Players.Players", "BankedGold", c => c.Long(nullable: false));
            AlterColumn("Players.Players", "BankedFood", c => c.Long(nullable: false));
            AlterColumn("Players.Players", "BankedWood", c => c.Long(nullable: false));
            AlterColumn("Players.Players", "BankedStone", c => c.Long(nullable: false));
            AlterColumn("Players.Players", "BankedOre", c => c.Long(nullable: false));
            AlterColumn("Attacks.Attacks", "Gold", c => c.Long(nullable: false));
            AlterColumn("Attacks.Attacks", "Food", c => c.Long(nullable: false));
            AlterColumn("Attacks.Attacks", "Wood", c => c.Long(nullable: false));
            AlterColumn("Attacks.Attacks", "Stone", c => c.Long(nullable: false));
            AlterColumn("Attacks.Attacks", "Ore", c => c.Long(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("Attacks.Attacks", "Ore", c => c.Int(nullable: false));
            AlterColumn("Attacks.Attacks", "Stone", c => c.Int(nullable: false));
            AlterColumn("Attacks.Attacks", "Wood", c => c.Int(nullable: false));
            AlterColumn("Attacks.Attacks", "Food", c => c.Int(nullable: false));
            AlterColumn("Attacks.Attacks", "Gold", c => c.Int(nullable: false));
            AlterColumn("Players.Players", "BankedOre", c => c.Int(nullable: false));
            AlterColumn("Players.Players", "BankedStone", c => c.Int(nullable: false));
            AlterColumn("Players.Players", "BankedWood", c => c.Int(nullable: false));
            AlterColumn("Players.Players", "BankedFood", c => c.Int(nullable: false));
            AlterColumn("Players.Players", "BankedGold", c => c.Int(nullable: false));
            AlterColumn("Players.Players", "Ore", c => c.Int(nullable: false));
            AlterColumn("Players.Players", "Stone", c => c.Int(nullable: false));
            AlterColumn("Players.Players", "Wood", c => c.Int(nullable: false));
            AlterColumn("Players.Players", "Food", c => c.Int(nullable: false));
            AlterColumn("Players.Players", "Gold", c => c.Int(nullable: false));
        }
    }
}
