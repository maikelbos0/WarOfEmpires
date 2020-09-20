namespace WarOfEmpires.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RoleAllianceRequired : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("Alliances.Roles", "Alliance_Id", "Alliances.Alliances");
            DropIndex("Alliances.Roles", new[] { "Alliance_Id" });
            AlterColumn("Alliances.Roles", "Alliance_Id", c => c.Int(nullable: false));
            CreateIndex("Alliances.Roles", "Alliance_Id");
            AddForeignKey("Alliances.Roles", "Alliance_Id", "Alliances.Alliances", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("Alliances.Roles", "Alliance_Id", "Alliances.Alliances");
            DropIndex("Alliances.Roles", new[] { "Alliance_Id" });
            AlterColumn("Alliances.Roles", "Alliance_Id", c => c.Int());
            CreateIndex("Alliances.Roles", "Alliance_Id");
            AddForeignKey("Alliances.Roles", "Alliance_Id", "Alliances.Alliances", "Id");
        }
    }
}
