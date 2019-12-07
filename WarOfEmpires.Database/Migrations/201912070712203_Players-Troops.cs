namespace WarOfEmpires.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PlayersTroops : DbMigration
    {
        public override void Up()
        {
            AddColumn("Players.Players", "Archers", c => c.Int(nullable: false));
            AddColumn("Players.Players", "Cavalry", c => c.Int(nullable: false));
            AddColumn("Players.Players", "Footmen", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("Players.Players", "Footmen");
            DropColumn("Players.Players", "Cavalry");
            DropColumn("Players.Players", "Archers");
        }
    }
}
