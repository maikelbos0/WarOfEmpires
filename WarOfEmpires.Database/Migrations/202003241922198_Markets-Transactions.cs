namespace WarOfEmpires.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MarketsTransactions : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Markets.Transactions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Type = c.Int(nullable: false),
                        Quantity = c.Int(nullable: false),
                        Price = c.Int(nullable: false),
                        IsRead = c.Boolean(nullable: false),
                        Buyer_Id = c.Int(nullable: false),
                        Seller_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("Players.Players", t => t.Buyer_Id, cascadeDelete: true)
                .ForeignKey("Players.Players", t => t.Seller_Id)
                .Index(t => t.Buyer_Id)
                .Index(t => t.Seller_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("Markets.Transactions", "Seller_Id", "Players.Players");
            DropForeignKey("Markets.Transactions", "Buyer_Id", "Players.Players");
            DropIndex("Markets.Transactions", new[] { "Seller_Id" });
            DropIndex("Markets.Transactions", new[] { "Buyer_Id" });
            DropTable("Markets.Transactions");
        }
    }
}
