using Microsoft.EntityFrameworkCore.Migrations;

namespace WarOfEmpires.Database.Migrations
{
    public partial class EventsSeed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                schema: "Events",
                table: "TaskExecutionModes",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Execute once" },
                    { 2, "Execute all intervals" }
                });

            migrationBuilder.InsertData(
                schema: "Security",
                table: "UserStatus",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { (byte)1, "New" },
                    { (byte)2, "Active" },
                    { (byte)3, "Inactive" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "Events",
                table: "TaskExecutionModes",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                schema: "Events",
                table: "TaskExecutionModes",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                schema: "Security",
                table: "UserStatus",
                keyColumn: "Id",
                keyValue: (byte)1);

            migrationBuilder.DeleteData(
                schema: "Security",
                table: "UserStatus",
                keyColumn: "Id",
                keyValue: (byte)2);

            migrationBuilder.DeleteData(
                schema: "Security",
                table: "UserStatus",
                keyColumn: "Id",
                keyValue: (byte)3);
        }
    }
}
