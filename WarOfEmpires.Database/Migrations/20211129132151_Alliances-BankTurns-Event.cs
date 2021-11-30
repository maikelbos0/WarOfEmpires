using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WarOfEmpires.Database.Migrations
{
    public partial class AlliancesBankTurnsEvent : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                schema: "Events",
                table: "ScheduledTasks",
                columns: new[] { "Id", "EventType", "ExecutionMode", "Interval", "IsPaused", "LastExecutionDate" },
                values: new object[] { 5, "WarOfEmpires.Domain.Alliances.BankTurnTaskTriggeredEvent, WarOfEmpires.Domain, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", 2, new TimeSpan(0, 2, 0, 0, 0), true, null });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "Events",
                table: "ScheduledTasks",
                keyColumn: "Id",
                keyValue: 5);
        }
    }
}
