using Microsoft.EntityFrameworkCore.Migrations;

namespace WarOfEmpires.Database.Migrations
{
    public partial class UniversityScientists : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                schema: "Empires",
                table: "BuildingTypes",
                columns: new[] { "Id", "Name" },
                values: new object[] { 20, "University" });

            migrationBuilder.InsertData(
                schema: "Empires",
                table: "WorkerTypes",
                columns: new[] { "Id", "Name" },
                values: new object[] { 7, "Scientists" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "Empires",
                table: "BuildingTypes",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                schema: "Empires",
                table: "WorkerTypes",
                keyColumn: "Id",
                keyValue: 7);
        }
    }
}
