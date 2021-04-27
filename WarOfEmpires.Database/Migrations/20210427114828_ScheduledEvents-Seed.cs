using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WarOfEmpires.Database.Migrations
{
    public partial class ScheduledEventsSeed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                schema: "Events",
                table: "ScheduledTasks",
                columns: new[] { "Id", "EventType", "ExecutionMode", "Interval", "IsPaused", "LastExecutionDate" },
                values: new object[,]
                {
                    { 1, "WarOfEmpires.Domain.Empires.RecruitTaskTriggeredEvent, WarOfEmpires.Domain, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", 2, new TimeSpan(0, 1, 0, 0, 0), true, null },
                    { 2, "WarOfEmpires.Domain.Empires.TurnTaskTriggeredEvent, WarOfEmpires.Domain, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", 2, new TimeSpan(0, 0, 10, 0, 0), true, null },
                    { 3, "WarOfEmpires.Domain.Empires.BankTurnTaskTriggeredEvent, WarOfEmpires.Domain, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", 2, new TimeSpan(0, 4, 0, 0, 0), true, null },
                    { 4, "WarOfEmpires.Domain.Empires.UpdateRankTaskTriggeredEvent, WarOfEmpires.Domain, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", 1, new TimeSpan(0, 0, 2, 0, 0), true, null }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "Events",
                table: "ScheduledTasks",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                schema: "Events",
                table: "ScheduledTasks",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                schema: "Events",
                table: "ScheduledTasks",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                schema: "Events",
                table: "ScheduledTasks",
                keyColumn: "Id",
                keyValue: 4);
        }
    }
}
