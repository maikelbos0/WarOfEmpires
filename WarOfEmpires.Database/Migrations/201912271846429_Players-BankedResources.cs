namespace WarOfEmpires.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PlayersBankedResources : DbMigration
    {
        public override void Up()
        {
            AddColumn("Players.Players", "BankedGold", c => c.Int(nullable: false));
            AddColumn("Players.Players", "BankedFood", c => c.Int(nullable: false));
            AddColumn("Players.Players", "BankedWood", c => c.Int(nullable: false));
            AddColumn("Players.Players", "BankedStone", c => c.Int(nullable: false));
            AddColumn("Players.Players", "BankedOre", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("Players.Players", "BankedOre");
            DropColumn("Players.Players", "BankedStone");
            DropColumn("Players.Players", "BankedWood");
            DropColumn("Players.Players", "BankedFood");
            DropColumn("Players.Players", "BankedGold");
        }
    }
}
