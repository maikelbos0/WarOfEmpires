using Microsoft.EntityFrameworkCore.Migrations;

namespace WarOfEmpires.Database.Migrations
{
    public partial class PlayersBlocks : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PlayerBlocks",
                schema: "Players",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BlockedPlayerId = table.Column<int>(type: "int", nullable: false),
                    PlayerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerBlocks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlayerBlocks_Players_BlockedPlayerId",
                        column: x => x.BlockedPlayerId,
                        principalSchema: "Players",
                        principalTable: "Players",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PlayerBlocks_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalSchema: "Players",
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PlayerBlocks_BlockedPlayerId",
                schema: "Players",
                table: "PlayerBlocks",
                column: "BlockedPlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerBlocks_PlayerId",
                schema: "Players",
                table: "PlayerBlocks",
                column: "PlayerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PlayerBlocks",
                schema: "Players");
        }
    }
}
