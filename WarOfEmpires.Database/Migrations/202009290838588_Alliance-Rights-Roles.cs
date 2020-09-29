namespace WarOfEmpires.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AllianceRightsRoles : DbMigration
    {
        public override void Up()
        {
            AddColumn("Alliances.Roles", "CanManageRoles", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("Alliances.Roles", "CanManageRoles");
        }
    }
}
