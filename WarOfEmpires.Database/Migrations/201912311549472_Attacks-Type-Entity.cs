namespace WarOfEmpires.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AttacksTypeEntity : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Attacks.AttackTypes",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateIndex("Attacks.Attacks", "Type");
            AddForeignKey("Attacks.Attacks", "Type", "Attacks.AttackTypes", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("Attacks.Attacks", "Type", "Attacks.AttackTypes");
            DropIndex("Attacks.Attacks", new[] { "Type" });
            DropTable("Attacks.AttackTypes");
        }
    }
}
