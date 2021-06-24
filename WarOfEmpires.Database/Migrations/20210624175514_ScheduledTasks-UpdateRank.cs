using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WarOfEmpires.Database.Migrations
{
    public partial class ScheduledTasksUpdateRank : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                schema: "Events",
                table: "ScheduledTasks",
                keyColumn: "Id",
                keyValue: 4,
                column: "Interval",
                value: new TimeSpan(0, 0, 1, 0, 0));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                schema: "Events",
                table: "ScheduledTasks",
                keyColumn: "Id",
                keyValue: 4,
                column: "Interval",
                value: new TimeSpan(0, 0, 2, 0, 0));
        }
    }
}
