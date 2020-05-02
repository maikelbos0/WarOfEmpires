namespace WarOfEmpires.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlliancesAlliances : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Alliances.Alliances",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IsActive = c.Boolean(nullable: false),
                        Code = c.String(),
                        Name = c.String(),
                        Leader_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("Players.Players", t => t.Leader_Id, cascadeDelete: true)
                .Index(t => t.Leader_Id);
            
            AddColumn("Players.Players", "Alliance_Id", c => c.Int());
            CreateIndex("Players.Players", "Alliance_Id");
            AddForeignKey("Players.Players", "Alliance_Id", "Alliances.Alliances", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("Players.Players", "Alliance_Id", "Alliances.Alliances");
            DropForeignKey("Alliances.Alliances", "Leader_Id", "Players.Players");
            DropIndex("Players.Players", new[] { "Alliance_Id" });
            DropIndex("Alliances.Alliances", new[] { "Leader_Id" });
            DropColumn("Players.Players", "Alliance_Id");
            DropTable("Alliances.Alliances");
        }
    }
}
