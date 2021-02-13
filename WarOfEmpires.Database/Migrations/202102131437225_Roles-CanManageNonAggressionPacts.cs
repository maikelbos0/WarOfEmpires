namespace WarOfEmpires.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RolesCanManageNonAggressionPacts : DbMigration
    {
        public override void Up()
        {
            AddColumn("Alliances.Roles", "CanManageNonAggressionPacts", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("Alliances.Roles", "CanManageNonAggressionPacts");
        }
    }
}
