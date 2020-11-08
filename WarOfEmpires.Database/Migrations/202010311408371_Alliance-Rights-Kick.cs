namespace WarOfEmpires.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AllianceRightsKick : DbMigration
    {
        public override void Up()
        {
            AddColumn("Alliances.Roles", "CanKickMembers", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("Alliances.Roles", "CanKickMembers");
        }
    }
}
