namespace WarOfEmpires.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SiegeWeapons : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Siege.SiegeWeapons",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Type = c.Int(nullable: false),
                        Count = c.Int(nullable: false),
                        Player_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("Players.Players", t => t.Player_Id)
                .ForeignKey("Siege.SiegeWeaponTypes", t => t.Type, cascadeDelete: true)
                .Index(t => t.Type)
                .Index(t => t.Player_Id);
            
            CreateTable(
                "Siege.SiegeWeaponTypes",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("Siege.SiegeWeapons", "Type", "Siege.SiegeWeaponTypes");
            DropForeignKey("Siege.SiegeWeapons", "Player_Id", "Players.Players");
            DropIndex("Siege.SiegeWeapons", new[] { "Player_Id" });
            DropIndex("Siege.SiegeWeapons", new[] { "Type" });
            DropTable("Siege.SiegeWeaponTypes");
            DropTable("Siege.SiegeWeapons");
        }
    }
}
