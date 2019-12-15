namespace WarOfEmpires.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AttacksDate : DbMigration
    {
        public override void Up()
        {
            AddColumn("Attacks.Attacks", "Date", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("Attacks.Attacks", "Date");
        }
    }
}
