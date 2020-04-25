namespace WarOfEmpires.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PlayersTitle : DbMigration
    {
        public override void Up()
        {
            AddColumn("Players.Players", "Title", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("Players.Players", "Title");
        }
    }
}
