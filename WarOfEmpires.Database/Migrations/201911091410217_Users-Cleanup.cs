namespace WarOfEmpires.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UsersCleanup : DbMigration
    {
        public override void Up()
        {
            DropColumn("Security.Users", "DisplayName");
            DropColumn("Security.Users", "Description");
            DropColumn("Security.Users", "ShowEmail");
        }
        
        public override void Down()
        {
            AddColumn("Security.Users", "ShowEmail", c => c.Boolean(nullable: false));
            AddColumn("Security.Users", "Description", c => c.String());
            AddColumn("Security.Users", "DisplayName", c => c.String(maxLength: 50));
        }
    }
}
