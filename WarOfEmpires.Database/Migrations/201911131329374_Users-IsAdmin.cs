namespace WarOfEmpires.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UsersIsAdmin : DbMigration
    {
        public override void Up()
        {
            AddColumn("Security.Users", "IsAdmin", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("Security.Users", "IsAdmin");
        }
    }
}
