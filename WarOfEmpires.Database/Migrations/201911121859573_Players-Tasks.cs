namespace WarOfEmpires.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PlayersTasks : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Events.ScheduledTasks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Interval = c.Time(nullable: false, precision: 7),
                        EventType = c.String(nullable: false),
                        IsPaused = c.Boolean(nullable: false),
                        LastExecutionDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("Players.Players", "Peasants", c => c.Int(nullable: false));
            AddColumn("Players.Players", "RecruitsPerDay", c => c.Int(nullable: false));
            AddColumn("Players.Players", "CurrentRecruitingEffort", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("Players.Players", "CurrentRecruitingEffort");
            DropColumn("Players.Players", "RecruitsPerDay");
            DropColumn("Players.Players", "Peasants");
            DropTable("Events.ScheduledTasks");
        }
    }
}
