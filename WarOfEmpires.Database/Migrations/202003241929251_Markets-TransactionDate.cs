namespace WarOfEmpires.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MarketsTransactionDate : DbMigration
    {
        public override void Up()
        {
            AddColumn("Markets.Transactions", "Date", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("Markets.Transactions", "Date");
        }
    }
}
