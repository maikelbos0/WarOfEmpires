namespace WarOfEmpires.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AllianceInviteSubjectBody : DbMigration
    {
        public override void Up()
        {
            AddColumn("Alliances.Invites", "Subject", c => c.String(nullable: false, maxLength: 100));
            AddColumn("Alliances.Invites", "Body", c => c.String());
            DropColumn("Alliances.Invites", "Message");
        }
        
        public override void Down()
        {
            AddColumn("Alliances.Invites", "Message", c => c.String());
            DropColumn("Alliances.Invites", "Body");
            DropColumn("Alliances.Invites", "Subject");
        }
    }
}
