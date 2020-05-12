namespace WarOfEmpires.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlliancesIsActiveRemoved : DbMigration
    {
        public override void Up()
        {
            DropColumn("Alliances.Alliances", "IsActive");
        }
        
        public override void Down()
        {
            AddColumn("Alliances.Alliances", "IsActive", c => c.Boolean(nullable: false));
        }
    }
}
