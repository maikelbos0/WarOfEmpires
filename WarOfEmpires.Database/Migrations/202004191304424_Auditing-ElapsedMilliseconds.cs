namespace WarOfEmpires.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AuditingElapsedMilliseconds : DbMigration
    {
        public override void Up()
        {
            AddColumn("Auditing.CommandExecutions", "ElapsedMilliseconds", c => c.Double(nullable: false));
            AddColumn("Auditing.QueryExecutions", "ElapsedMilliseconds", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("Auditing.QueryExecutions", "ElapsedMilliseconds");
            DropColumn("Auditing.CommandExecutions", "ElapsedMilliseconds");
        }
    }
}
