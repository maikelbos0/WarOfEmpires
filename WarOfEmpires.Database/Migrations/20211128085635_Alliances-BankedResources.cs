using Microsoft.EntityFrameworkCore.Migrations;

namespace WarOfEmpires.Database.Migrations
{
    public partial class AlliancesBankedResources : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BankTurns",
                schema: "Alliances",
                table: "Alliances",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<long>(
                name: "BankedFood",
                schema: "Alliances",
                table: "Alliances",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "BankedGold",
                schema: "Alliances",
                table: "Alliances",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "BankedOre",
                schema: "Alliances",
                table: "Alliances",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "BankedStone",
                schema: "Alliances",
                table: "Alliances",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "BankedWood",
                schema: "Alliances",
                table: "Alliances",
                type: "bigint",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BankTurns",
                schema: "Alliances",
                table: "Alliances");

            migrationBuilder.DropColumn(
                name: "BankedFood",
                schema: "Alliances",
                table: "Alliances");

            migrationBuilder.DropColumn(
                name: "BankedGold",
                schema: "Alliances",
                table: "Alliances");

            migrationBuilder.DropColumn(
                name: "BankedOre",
                schema: "Alliances",
                table: "Alliances");

            migrationBuilder.DropColumn(
                name: "BankedStone",
                schema: "Alliances",
                table: "Alliances");

            migrationBuilder.DropColumn(
                name: "BankedWood",
                schema: "Alliances",
                table: "Alliances");
        }
    }
}
