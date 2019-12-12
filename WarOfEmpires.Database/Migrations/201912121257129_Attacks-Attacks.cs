namespace WarOfEmpires.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AttacksAttacks : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Attacks.Attacks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Result = c.Int(nullable: false),
                        Turns = c.Int(nullable: false),
                        Attacker_Id = c.Int(nullable: false),
                        Defender_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("Players.Players", t => t.Attacker_Id)
                .ForeignKey("Players.Players", t => t.Defender_Id)
                .ForeignKey("Attacks.AttackResults", t => t.Result, cascadeDelete: true)
                .Index(t => t.Result)
                .Index(t => t.Attacker_Id)
                .Index(t => t.Defender_Id);
            
            CreateTable(
                "Attacks.AttackRounds",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TroopType = c.Int(nullable: false),
                        IsAggressor = c.Boolean(nullable: false),
                        Troops = c.Int(nullable: false),
                        Damage = c.Long(nullable: false),
                        Archers = c.Int(nullable: false),
                        MercenaryArchers = c.Int(nullable: false),
                        Cavalry = c.Int(nullable: false),
                        MercenaryCavalry = c.Int(nullable: false),
                        Footmen = c.Int(nullable: false),
                        MercenaryFootmen = c.Int(nullable: false),
                        Attack_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("Attacks.Attacks", t => t.Attack_Id, cascadeDelete: true)
                .ForeignKey("Attacks.TroopTypes", t => t.TroopType, cascadeDelete: true)
                .Index(t => t.TroopType)
                .Index(t => t.Attack_Id);
            
            CreateTable(
                "Attacks.AttackResults",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "Attacks.TroopTypes",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("Attacks.AttackRounds", "TroopType", "Attacks.TroopTypes");
            DropForeignKey("Attacks.Attacks", "Result", "Attacks.AttackResults");
            DropForeignKey("Attacks.Attacks", "Defender_Id", "Players.Players");
            DropForeignKey("Attacks.Attacks", "Attacker_Id", "Players.Players");
            DropForeignKey("Attacks.AttackRounds", "Attack_Id", "Attacks.Attacks");
            DropIndex("Attacks.AttackRounds", new[] { "Attack_Id" });
            DropIndex("Attacks.AttackRounds", new[] { "TroopType" });
            DropIndex("Attacks.Attacks", new[] { "Defender_Id" });
            DropIndex("Attacks.Attacks", new[] { "Attacker_Id" });
            DropIndex("Attacks.Attacks", new[] { "Result" });
            DropTable("Attacks.TroopTypes");
            DropTable("Attacks.AttackResults");
            DropTable("Attacks.AttackRounds");
            DropTable("Attacks.Attacks");
        }
    }
}
