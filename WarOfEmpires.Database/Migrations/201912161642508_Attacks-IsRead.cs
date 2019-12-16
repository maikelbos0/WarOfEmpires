namespace WarOfEmpires.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AttacksIsRead : DbMigration
    {
        public override void Up()
        {
            AddColumn("Attacks.Attacks", "IsRead", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("Attacks.Attacks", "IsRead");
        }
    }
}
