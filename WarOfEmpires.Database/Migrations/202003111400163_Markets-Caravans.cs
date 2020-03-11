namespace WarOfEmpires.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MarketsCaravans : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Markets.Caravans",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Player_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("Players.Players", t => t.Player_Id, cascadeDelete: true)
                .Index(t => t.Player_Id);
            
            CreateTable(
                "Markets.Merchandise",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Type = c.Int(nullable: false),
                        Quantity = c.Int(nullable: false),
                        Price = c.Int(nullable: false),
                        Caravan_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("Markets.Caravans", t => t.Caravan_Id, cascadeDelete: true)
                .ForeignKey("Markets.MerchandiseTypes", t => t.Type, cascadeDelete: true)
                .Index(t => t.Type)
                .Index(t => t.Caravan_Id);
            
            CreateTable(
                "Markets.MerchandiseTypes",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("Markets.Merchandise", "Type", "Markets.MerchandiseTypes");
            DropForeignKey("Markets.Caravans", "Player_Id", "Players.Players");
            DropForeignKey("Markets.Merchandise", "Caravan_Id", "Markets.Caravans");
            DropIndex("Markets.Merchandise", new[] { "Caravan_Id" });
            DropIndex("Markets.Merchandise", new[] { "Type" });
            DropIndex("Markets.Caravans", new[] { "Player_Id" });
            DropTable("Markets.MerchandiseTypes");
            DropTable("Markets.Merchandise");
            DropTable("Markets.Caravans");
        }
    }
}
