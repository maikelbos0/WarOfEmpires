namespace WarOfEmpires.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EmpiresWorkerTypes : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Empires.WorkerTypes",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateIndex("Empires.Workers", "Type");
            AddForeignKey("Empires.Workers", "Type", "Empires.WorkerTypes", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("Empires.Workers", "Type", "Empires.WorkerTypes");
            DropIndex("Empires.Workers", new[] { "Type" });
            DropTable("Empires.WorkerTypes");
        }
    }
}
