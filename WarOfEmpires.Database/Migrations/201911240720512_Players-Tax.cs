namespace WarOfEmpires.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PlayersTax : DbMigration
    {
        public override void Up()
        {
            AddColumn("Players.Players", "Tax", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("Players.Players", "Tax");
        }
    }
}
