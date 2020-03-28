namespace WarOfEmpires.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AttacksCascadePaths : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("Attacks.Attacks", "Defender_Id", "Players.Players");
            AddForeignKey("Attacks.Attacks", "Defender_Id", "Players.Players", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("Attacks.Attacks", "Defender_Id", "Players.Players");
            AddForeignKey("Attacks.Attacks", "Defender_Id", "Players.Players", "Id");
        }
    }
}
