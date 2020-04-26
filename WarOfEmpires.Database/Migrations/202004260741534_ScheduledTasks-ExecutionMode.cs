namespace WarOfEmpires.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ScheduledTasksExecutionMode : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Events.TaskExecutionModes",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("Events.ScheduledTasks", "ExecutionMode", c => c.Int(nullable: false));

            // Manually ensure the migration can add the foreign key
            Sql("INSERT INTO Events.TaskExecutionModes VALUES (1, 'Dummy');");
            Sql("UPDATE Events.ScheduledTasks SET ScheduledTasks.ExecutionMode = 1;");

            CreateIndex("Events.ScheduledTasks", "ExecutionMode");
            AddForeignKey("Events.ScheduledTasks", "ExecutionMode", "Events.TaskExecutionModes", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("Events.ScheduledTasks", "ExecutionMode", "Events.TaskExecutionModes");
            DropIndex("Events.ScheduledTasks", new[] { "ExecutionMode" });
            DropColumn("Events.ScheduledTasks", "ExecutionMode");
            DropTable("Events.TaskExecutionModes");
        }
    }
}
