using Microsoft.EntityFrameworkCore.Migrations;

namespace WarOfEmpires.Database.Migrations
{
    public partial class ResearchSafeStorage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                schema: "Empires",
                table: "ResearchTypes",
                columns: new[] { "Id", "Name" },
                values: new object[] { 5, "Safe storage" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "Empires",
                table: "ResearchTypes",
                keyColumn: "Id",
                keyValue: 5);
        }
    }
}
