namespace WarOfEmpires.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PlayersAttackTurns : DbMigration
    {
        public override void Up()
        {
            AddColumn("Players.Players", "AttackTurns", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("Players.Players", "AttackTurns");
        }
    }
}
