namespace WarOfEmpires.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PlayersSiegeEngineers : DbMigration
    {
        public override void Up()
        {
            AddColumn("Players.Players", "SiegeEngineers", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("Players.Players", "SiegeEngineers");
        }
    }
}
