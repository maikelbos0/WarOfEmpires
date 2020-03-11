namespace WarOfEmpires.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CaravansDate : DbMigration
    {
        public override void Up()
        {
            AddColumn("Markets.Caravans", "Date", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("Markets.Caravans", "Date");
        }
    }
}
