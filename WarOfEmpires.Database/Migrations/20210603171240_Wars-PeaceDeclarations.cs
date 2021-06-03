using Microsoft.EntityFrameworkCore.Migrations;

namespace WarOfEmpires.Database.Migrations
{
    public partial class WarsPeaceDeclarations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PeaceDeclarations",
                columns: table => new
                {
                    AllianceId = table.Column<int>(type: "int", nullable: false),
                    WarId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PeaceDeclarations", x => new { x.AllianceId, x.WarId });
                    table.ForeignKey(
                        name: "FK_PeaceDeclarations_Alliances_AllianceId",
                        column: x => x.AllianceId,
                        principalSchema: "Alliances",
                        principalTable: "Alliances",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PeaceDeclarations_War_WarId",
                        column: x => x.WarId,
                        principalTable: "War",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PeaceDeclarations_WarId",
                table: "PeaceDeclarations",
                column: "WarId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PeaceDeclarations");
        }
    }
}
