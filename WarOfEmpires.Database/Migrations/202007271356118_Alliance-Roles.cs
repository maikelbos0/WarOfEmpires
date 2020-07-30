namespace WarOfEmpires.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AllianceRoles : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Alliances.Roles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Alliance_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("Alliances.Alliances", t => t.Alliance_Id)
                .Index(t => t.Alliance_Id);
            
            AddColumn("Players.Players", "AllianceRole_Id", c => c.Int());
            CreateIndex("Players.Players", "AllianceRole_Id");
            AddForeignKey("Players.Players", "AllianceRole_Id", "Alliances.Roles", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("Players.Players", "AllianceRole_Id", "Alliances.Roles");
            DropForeignKey("Alliances.Roles", "Alliance_Id", "Alliances.Alliances");
            DropIndex("Alliances.Roles", new[] { "Alliance_Id" });
            DropIndex("Players.Players", new[] { "AllianceRole_Id" });
            DropColumn("Players.Players", "AllianceRole_Id");
            DropTable("Alliances.Roles");
        }
    }
}
