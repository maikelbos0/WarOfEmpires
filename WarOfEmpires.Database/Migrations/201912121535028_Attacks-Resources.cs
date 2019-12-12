namespace WarOfEmpires.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AttacksResources : DbMigration
    {
        public override void Up()
        {
            AddColumn("Attacks.Attacks", "Gold", c => c.Int(nullable: false));
            AddColumn("Attacks.Attacks", "Food", c => c.Int(nullable: false));
            AddColumn("Attacks.Attacks", "Wood", c => c.Int(nullable: false));
            AddColumn("Attacks.Attacks", "Stone", c => c.Int(nullable: false));
            AddColumn("Attacks.Attacks", "Ore", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("Attacks.Attacks", "Ore");
            DropColumn("Attacks.Attacks", "Stone");
            DropColumn("Attacks.Attacks", "Wood");
            DropColumn("Attacks.Attacks", "Food");
            DropColumn("Attacks.Attacks", "Gold");
        }
    }
}
