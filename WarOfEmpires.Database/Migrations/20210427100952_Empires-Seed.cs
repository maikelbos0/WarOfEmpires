using Microsoft.EntityFrameworkCore.Migrations;

namespace WarOfEmpires.Database.Migrations
{
    public partial class EmpiresSeed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                schema: "Empires",
                table: "BuildingTypes",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Farm" },
                    { 19, "Market" },
                    { 18, "Siege factory" },
                    { 17, "Ore bank" },
                    { 15, "Wood bank" },
                    { 14, "Food bank" },
                    { 13, "Gold bank" },
                    { 12, "Barracks" },
                    { 11, "Huts" },
                    { 16, "Stone bank" },
                    { 9, "Footman range" },
                    { 8, "Cavalry range" },
                    { 7, "Archery range" },
                    { 6, "Armoury" },
                    { 5, "Forge" },
                    { 4, "Mine" },
                    { 3, "Quarry" },
                    { 2, "Lumberyard" },
                    { 10, "Defences" }
                });

            migrationBuilder.InsertData(
                schema: "Empires",
                table: "WorkerTypes",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 4, "Ore miners" },
                    { 3, "Stone masons" },
                    { 2, "Wood workers" },
                    { 1, "Farmers" },
                    { 6, "Merchants" },
                    { 5, "Siege engineers" }
                });

            migrationBuilder.InsertData(
                schema: "Markets",
                table: "MerchandiseTypes",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 4, "Ore" },
                    { 3, "Stone" },
                    { 1, "Food" },
                    { 2, "Wood" }
                });

            migrationBuilder.InsertData(
                schema: "Siege",
                table: "SiegeWeaponTypes",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 2, "Battering rams" },
                    { 3, "Scaling ladders" },
                    { 1, "Fire arrows" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "Empires",
                table: "BuildingTypes",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                schema: "Empires",
                table: "BuildingTypes",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                schema: "Empires",
                table: "BuildingTypes",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                schema: "Empires",
                table: "BuildingTypes",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                schema: "Empires",
                table: "BuildingTypes",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                schema: "Empires",
                table: "BuildingTypes",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                schema: "Empires",
                table: "BuildingTypes",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                schema: "Empires",
                table: "BuildingTypes",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                schema: "Empires",
                table: "BuildingTypes",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                schema: "Empires",
                table: "BuildingTypes",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                schema: "Empires",
                table: "BuildingTypes",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                schema: "Empires",
                table: "BuildingTypes",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                schema: "Empires",
                table: "BuildingTypes",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                schema: "Empires",
                table: "BuildingTypes",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                schema: "Empires",
                table: "BuildingTypes",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                schema: "Empires",
                table: "BuildingTypes",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                schema: "Empires",
                table: "BuildingTypes",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                schema: "Empires",
                table: "BuildingTypes",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                schema: "Empires",
                table: "BuildingTypes",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                schema: "Empires",
                table: "WorkerTypes",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                schema: "Empires",
                table: "WorkerTypes",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                schema: "Empires",
                table: "WorkerTypes",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                schema: "Empires",
                table: "WorkerTypes",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                schema: "Empires",
                table: "WorkerTypes",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                schema: "Empires",
                table: "WorkerTypes",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                schema: "Markets",
                table: "MerchandiseTypes",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                schema: "Markets",
                table: "MerchandiseTypes",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                schema: "Markets",
                table: "MerchandiseTypes",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                schema: "Markets",
                table: "MerchandiseTypes",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                schema: "Siege",
                table: "SiegeWeaponTypes",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                schema: "Siege",
                table: "SiegeWeaponTypes",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                schema: "Siege",
                table: "SiegeWeaponTypes",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
