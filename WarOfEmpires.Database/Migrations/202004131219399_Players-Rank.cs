namespace WarOfEmpires.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PlayersRank : DbMigration
    {
        public override void Up()
        {
            AddColumn("Players.Players", "Rank", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("Players.Players", "Rank");
        }
    }
}
