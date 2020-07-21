namespace WarOfEmpires.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveTransactionsIsRead : DbMigration
    {
        public override void Up()
        {
            DropColumn("Markets.Transactions", "IsRead");
        }
        
        public override void Down()
        {
            AddColumn("Markets.Transactions", "IsRead", c => c.Boolean(nullable: false));
        }
    }
}
