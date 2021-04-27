using Microsoft.EntityFrameworkCore.Migrations;

namespace WarOfEmpires.Database.Migrations
{
    public partial class AttacksSeed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                schema: "Attacks",
                table: "AttackResults",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Undefined" },
                    { 2, "Won" },
                    { 3, "Defended" },
                    { 4, "Surrendered" },
                    { 5, "Fatigued" }
                });

            migrationBuilder.InsertData(
                schema: "Attacks",
                table: "AttackTypes",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Raid" },
                    { 2, "Assault" }
                });

            migrationBuilder.InsertData(
                schema: "Attacks",
                table: "TroopTypes",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Archers" },
                    { 2, "Cavalry" },
                    { 3, "Footmen" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "Attacks",
                table: "AttackResults",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                schema: "Attacks",
                table: "AttackResults",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                schema: "Attacks",
                table: "AttackResults",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                schema: "Attacks",
                table: "AttackResults",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                schema: "Attacks",
                table: "AttackResults",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                schema: "Attacks",
                table: "AttackTypes",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                schema: "Attacks",
                table: "AttackTypes",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                schema: "Attacks",
                table: "TroopTypes",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                schema: "Attacks",
                table: "TroopTypes",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                schema: "Attacks",
                table: "TroopTypes",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
