namespace WarOfEmpires.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MerchandiseTypesTransactions : DbMigration
    {
        public override void Up()
        {
            CreateIndex("Markets.Transactions", "Type");
            AddForeignKey("Markets.Transactions", "Type", "Markets.MerchandiseTypes", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("Markets.Transactions", "Type", "Markets.MerchandiseTypes");
            DropIndex("Markets.Transactions", new[] { "Type" });
        }
    }
}
