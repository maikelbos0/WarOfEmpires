namespace WarOfEmpires.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PlayersHasNewMarketSales : DbMigration
    {
        public override void Up()
        {
            AddColumn("Players.Players", "HasNewMarketSales", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("Players.Players", "HasNewMarketSales");
        }
    }
}
