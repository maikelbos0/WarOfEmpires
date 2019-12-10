namespace WarOfEmpires.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PlayersStamina : DbMigration
    {
        public override void Up()
        {
            AddColumn("Players.Players", "Stamina", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("Players.Players", "Stamina");
        }
    }
}
