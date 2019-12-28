namespace WarOfEmpires.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PlayersBankTurns : DbMigration
    {
        public override void Up()
        {
            AddColumn("Players.Players", "BankTurns", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("Players.Players", "BankTurns");
        }
    }
}
