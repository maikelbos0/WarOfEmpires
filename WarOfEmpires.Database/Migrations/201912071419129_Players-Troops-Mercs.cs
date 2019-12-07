namespace WarOfEmpires.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PlayersTroopsMercs : DbMigration
    {
        public override void Up()
        {
            AddColumn("Players.Players", "MercenaryArchers", c => c.Int(nullable: false));
            AddColumn("Players.Players", "MercenaryCavalry", c => c.Int(nullable: false));
            AddColumn("Players.Players", "MercenaryFootmen", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("Players.Players", "MercenaryFootmen");
            DropColumn("Players.Players", "MercenaryCavalry");
            DropColumn("Players.Players", "MercenaryArchers");
        }
    }
}
