namespace WarOfEmpires.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PlayersDynamicRecruiting : DbMigration
    {
        public override void Up()
        {
            DropColumn("Players.Players", "RecruitsPerDay");
        }
        
        public override void Down()
        {
            AddColumn("Players.Players", "RecruitsPerDay", c => c.Int(nullable: false));
        }
    }
}
