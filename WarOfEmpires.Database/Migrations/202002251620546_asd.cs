namespace WarOfEmpires.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class asd : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("Siege.SiegeWeapons", "Player_Id", "Players.Players");
            DropForeignKey("Attacks.Troops", "Player_Id", "Players.Players");
            DropIndex("Siege.SiegeWeapons", new[] { "Player_Id" });
            DropIndex("Attacks.Troops", new[] { "Player_Id" });
            AlterColumn("Siege.SiegeWeapons", "Player_Id", c => c.Int(nullable: false));
            AlterColumn("Attacks.Troops", "Player_Id", c => c.Int(nullable: false));
            CreateIndex("Siege.SiegeWeapons", "Player_Id");
            CreateIndex("Attacks.Troops", "Player_Id");
            AddForeignKey("Siege.SiegeWeapons", "Player_Id", "Players.Players", "Id", cascadeDelete: true);
            AddForeignKey("Attacks.Troops", "Player_Id", "Players.Players", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("Attacks.Troops", "Player_Id", "Players.Players");
            DropForeignKey("Siege.SiegeWeapons", "Player_Id", "Players.Players");
            DropIndex("Attacks.Troops", new[] { "Player_Id" });
            DropIndex("Siege.SiegeWeapons", new[] { "Player_Id" });
            AlterColumn("Attacks.Troops", "Player_Id", c => c.Int());
            AlterColumn("Siege.SiegeWeapons", "Player_Id", c => c.Int());
            CreateIndex("Attacks.Troops", "Player_Id");
            CreateIndex("Siege.SiegeWeapons", "Player_Id");
            AddForeignKey("Attacks.Troops", "Player_Id", "Players.Players", "Id");
            AddForeignKey("Siege.SiegeWeapons", "Player_Id", "Players.Players", "Id");
        }
    }
}
