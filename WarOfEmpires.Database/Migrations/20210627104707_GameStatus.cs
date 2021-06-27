using Microsoft.EntityFrameworkCore.Migrations;

namespace WarOfEmpires.Database.Migrations
{
    public partial class GameStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Game");

            migrationBuilder.CreateTable(
                name: "GamePhases",
                schema: "Game",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GamePhases", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GameStatus",
                schema: "Game",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    CurrentGrandOverlordId = table.Column<int>(type: "int", nullable: true),
                    Phase = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameStatus", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GameStatus_GamePhases_Phase",
                        column: x => x.Phase,
                        principalSchema: "Game",
                        principalTable: "GamePhases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GameStatus_Players_CurrentGrandOverlordId",
                        column: x => x.CurrentGrandOverlordId,
                        principalSchema: "Players",
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                schema: "Game",
                table: "GamePhases",
                columns: new[] { "Id", "Name" },
                values: new object[] { 0, "Truce" });

            migrationBuilder.InsertData(
                schema: "Game",
                table: "GamePhases",
                columns: new[] { "Id", "Name" },
                values: new object[] { 1, "Active" });

            migrationBuilder.InsertData(
                schema: "Game",
                table: "GamePhases",
                columns: new[] { "Id", "Name" },
                values: new object[] { 2, "Finished" });

            migrationBuilder.InsertData(
                schema: "Game",
                table: "GameStatus",
                columns: new[] { "Id", "CurrentGrandOverlordId", "Phase" },
                values: new object[] { 1, null, 0 });

            migrationBuilder.CreateIndex(
                name: "IX_GameStatus_CurrentGrandOverlordId",
                schema: "Game",
                table: "GameStatus",
                column: "CurrentGrandOverlordId");

            migrationBuilder.CreateIndex(
                name: "IX_GameStatus_Phase",
                schema: "Game",
                table: "GameStatus",
                column: "Phase");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GameStatus",
                schema: "Game");

            migrationBuilder.DropTable(
                name: "GamePhases",
                schema: "Game");
        }
    }
}
