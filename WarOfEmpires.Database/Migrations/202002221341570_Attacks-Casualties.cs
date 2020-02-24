namespace WarOfEmpires.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AttacksCasualties : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Attacks.Casualties",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TroopType = c.Int(nullable: false),
                        Soldiers = c.Int(nullable: false),
                        Mercenaries = c.Int(nullable: false),
                        AttackRound_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("Attacks.AttackRounds", t => t.AttackRound_Id)
                .Index(t => t.AttackRound_Id);
            
            AddColumn("Players.Players", "Archers_Type", c => c.Int(nullable: false));
            AddColumn("Players.Players", "Archers_Id", c => c.Int(nullable: false));
            AddColumn("Players.Players", "Cavalry_Type", c => c.Int(nullable: false));
            AddColumn("Players.Players", "Cavalry_Id", c => c.Int(nullable: false));
            AddColumn("Players.Players", "Footmen_Type", c => c.Int(nullable: false));
            AddColumn("Players.Players", "Footmen_Id", c => c.Int(nullable: false));
            DropColumn("Attacks.AttackRounds", "Archers");
            DropColumn("Attacks.AttackRounds", "MercenaryArchers");
            DropColumn("Attacks.AttackRounds", "Cavalry");
            DropColumn("Attacks.AttackRounds", "MercenaryCavalry");
            DropColumn("Attacks.AttackRounds", "Footmen");
            DropColumn("Attacks.AttackRounds", "MercenaryFootmen");
        }
        
        public override void Down()
        {
            AddColumn("Attacks.AttackRounds", "MercenaryFootmen", c => c.Int(nullable: false));
            AddColumn("Attacks.AttackRounds", "Footmen", c => c.Int(nullable: false));
            AddColumn("Attacks.AttackRounds", "MercenaryCavalry", c => c.Int(nullable: false));
            AddColumn("Attacks.AttackRounds", "Cavalry", c => c.Int(nullable: false));
            AddColumn("Attacks.AttackRounds", "MercenaryArchers", c => c.Int(nullable: false));
            AddColumn("Attacks.AttackRounds", "Archers", c => c.Int(nullable: false));
            DropForeignKey("Attacks.Casualties", "AttackRound_Id", "Attacks.AttackRounds");
            DropIndex("Attacks.Casualties", new[] { "AttackRound_Id" });
            DropColumn("Players.Players", "Footmen_Id");
            DropColumn("Players.Players", "Footmen_Type");
            DropColumn("Players.Players", "Cavalry_Id");
            DropColumn("Players.Players", "Cavalry_Type");
            DropColumn("Players.Players", "Archers_Id");
            DropColumn("Players.Players", "Archers_Type");
            DropTable("Attacks.Casualties");
        }
    }
}
