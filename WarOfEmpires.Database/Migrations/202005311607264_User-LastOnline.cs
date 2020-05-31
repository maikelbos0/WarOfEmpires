namespace WarOfEmpires.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserLastOnline : DbMigration
    {
        public override void Up()
        {
            AddColumn("Security.Users", "LastOnline", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("Security.Users", "LastOnline");
        }
    }
}
