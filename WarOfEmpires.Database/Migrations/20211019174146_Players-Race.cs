using Microsoft.EntityFrameworkCore.Migrations;

namespace WarOfEmpires.Database.Migrations
{
    public partial class PlayersRace : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Race",
                schema: "Players",
                table: "Players",
                type: "int",
                nullable: false,
                defaultValue: 3);

            migrationBuilder.CreateTable(
                name: "Races",
                schema: "Players",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Races", x => x.Id);
                });

            migrationBuilder.InsertData(
                schema: "Players",
                table: "Races",
                columns: new[] { "Id", "Name" },
                values: new object[] { 1, "Humans" });

            migrationBuilder.InsertData(
                schema: "Players",
                table: "Races",
                columns: new[] { "Id", "Name" },
                values: new object[] { 2, "Dwarves" });

            migrationBuilder.InsertData(
                schema: "Players",
                table: "Races",
                columns: new[] { "Id", "Name" },
                values: new object[] { 3, "Elves" });

            migrationBuilder.CreateIndex(
                name: "IX_Players_Race",
                schema: "Players",
                table: "Players",
                column: "Race");

            migrationBuilder.AddForeignKey(
                name: "FK_Players_Races_Race",
                schema: "Players",
                table: "Players",
                column: "Race",
                principalSchema: "Players",
                principalTable: "Races",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Players_Races_Race",
                schema: "Players",
                table: "Players");

            migrationBuilder.DropTable(
                name: "Races",
                schema: "Players");

            migrationBuilder.DropIndex(
                name: "IX_Players_Race",
                schema: "Players",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "Race",
                schema: "Players",
                table: "Players");
        }
    }
}
