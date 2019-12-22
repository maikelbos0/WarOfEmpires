namespace WarOfEmpires.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PlayersUpkeep : DbMigration
    {
        public override void Up()
        {
            AddColumn("Players.Players", "HasUpkeepRunOut", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("Players.Players", "HasUpkeepRunOut");
        }
    }
}
