namespace WarOfEmpires.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlliancesRequiredFields : DbMigration
    {
        public override void Up()
        {
            AlterColumn("Alliances.Alliances", "Code", c => c.String(nullable: false));
            AlterColumn("Alliances.Alliances", "Name", c => c.String(nullable: false));
            AlterColumn("Alliances.ChatMessages", "Message", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("Alliances.ChatMessages", "Message", c => c.String());
            AlterColumn("Alliances.Alliances", "Name", c => c.String());
            AlterColumn("Alliances.Alliances", "Code", c => c.String());
        }
    }
}
