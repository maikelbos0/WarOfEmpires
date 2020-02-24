namespace WarOfEmpires.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AttacksTroops : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Attacks.Troops",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Type = c.Int(nullable: false),
                        Soldiers = c.Int(nullable: false),
                        Mercenaries = c.Int(nullable: false),
                        Player_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("Players.Players", t => t.Player_Id)
                .Index(t => t.Player_Id);
            
            DropColumn("Players.Players", "Archers_Type");
            DropColumn("Players.Players", "Archers");
            DropColumn("Players.Players", "MercenaryArchers");
            DropColumn("Players.Players", "Archers_Id");
            DropColumn("Players.Players", "Cavalry_Type");
            DropColumn("Players.Players", "Cavalry");
            DropColumn("Players.Players", "MercenaryCavalry");
            DropColumn("Players.Players", "Cavalry_Id");
            DropColumn("Players.Players", "Footmen_Type");
            DropColumn("Players.Players", "Footmen");
            DropColumn("Players.Players", "MercenaryFootmen");
            DropColumn("Players.Players", "Footmen_Id");
        }
        
        public override void Down()
        {
            AddColumn("Players.Players", "Footmen_Id", c => c.Int(nullable: false));
            AddColumn("Players.Players", "MercenaryFootmen", c => c.Int(nullable: false));
            AddColumn("Players.Players", "Footmen", c => c.Int(nullable: false));
            AddColumn("Players.Players", "Footmen_Type", c => c.Int(nullable: false));
            AddColumn("Players.Players", "Cavalry_Id", c => c.Int(nullable: false));
            AddColumn("Players.Players", "MercenaryCavalry", c => c.Int(nullable: false));
            AddColumn("Players.Players", "Cavalry", c => c.Int(nullable: false));
            AddColumn("Players.Players", "Cavalry_Type", c => c.Int(nullable: false));
            AddColumn("Players.Players", "Archers_Id", c => c.Int(nullable: false));
            AddColumn("Players.Players", "MercenaryArchers", c => c.Int(nullable: false));
            AddColumn("Players.Players", "Archers", c => c.Int(nullable: false));
            AddColumn("Players.Players", "Archers_Type", c => c.Int(nullable: false));
            DropForeignKey("Attacks.Troops", "Player_Id", "Players.Players");
            DropIndex("Attacks.Troops", new[] { "Player_Id" });
            DropTable("Attacks.Troops");
        }
    }
}
