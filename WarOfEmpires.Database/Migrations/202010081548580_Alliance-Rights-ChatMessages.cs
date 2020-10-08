namespace WarOfEmpires.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AllianceRightsChatMessages : DbMigration
    {
        public override void Up()
        {
            AddColumn("Alliances.Roles", "CanDeleteChatMessages", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("Alliances.Roles", "CanDeleteChatMessages");
        }
    }
}
