namespace WarOfEmpires.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlliancesNonAggressionPacts : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Alliances.NonAggressionPacts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "Alliances.NonAggressionPactRequests",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Recipient_Id = c.Int(nullable: false),
                        Sender_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("Alliances.Alliances", t => t.Recipient_Id, cascadeDelete: true)
                .ForeignKey("Alliances.Alliances", t => t.Sender_Id)
                .Index(t => t.Recipient_Id)
                .Index(t => t.Sender_Id);
            
            CreateTable(
                "Alliances.AllianceNonAggressionPacts",
                c => new
                    {
                        Alliance_Id = c.Int(nullable: false),
                        NonAggressionPact_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Alliance_Id, t.NonAggressionPact_Id })
                .ForeignKey("Alliances.Alliances", t => t.Alliance_Id, cascadeDelete: true)
                .ForeignKey("Alliances.NonAggressionPacts", t => t.NonAggressionPact_Id, cascadeDelete: true)
                .Index(t => t.Alliance_Id)
                .Index(t => t.NonAggressionPact_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("Alliances.NonAggressionPactRequests", "Sender_Id", "Alliances.Alliances");
            DropForeignKey("Alliances.NonAggressionPactRequests", "Recipient_Id", "Alliances.Alliances");
            DropForeignKey("Alliances.AllianceNonAggressionPacts", "NonAggressionPact_Id", "Alliances.NonAggressionPacts");
            DropForeignKey("Alliances.AllianceNonAggressionPacts", "Alliance_Id", "Alliances.Alliances");
            DropIndex("Alliances.AllianceNonAggressionPacts", new[] { "NonAggressionPact_Id" });
            DropIndex("Alliances.AllianceNonAggressionPacts", new[] { "Alliance_Id" });
            DropIndex("Alliances.NonAggressionPactRequests", new[] { "Sender_Id" });
            DropIndex("Alliances.NonAggressionPactRequests", new[] { "Recipient_Id" });
            DropTable("Alliances.AllianceNonAggressionPacts");
            DropTable("Alliances.NonAggressionPactRequests");
            DropTable("Alliances.NonAggressionPacts");
        }
    }
}
