namespace WarOfEmpires.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AttacksType : DbMigration
    {
        public override void Up()
        {
            AddColumn("Attacks.Attacks", "Type", c => c.Int(nullable: false));
            AddColumn("Attacks.Attacks", "Discriminator", c => c.String(nullable: false, maxLength: 128));
        }
        
        public override void Down()
        {
            DropColumn("Attacks.Attacks", "Discriminator");
            DropColumn("Attacks.Attacks", "Type");
        }
    }
}
