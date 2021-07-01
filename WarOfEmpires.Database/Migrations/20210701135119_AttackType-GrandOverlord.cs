using Microsoft.EntityFrameworkCore.Migrations;

namespace WarOfEmpires.Database.Migrations
{
    public partial class AttackTypeGrandOverlord : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                schema: "Attacks",
                table: "AttackTypes",
                columns: new[] { "Id", "Name" },
                values: new object[] { 3, "Grand overlord attack" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "Attacks",
                table: "AttackTypes",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
