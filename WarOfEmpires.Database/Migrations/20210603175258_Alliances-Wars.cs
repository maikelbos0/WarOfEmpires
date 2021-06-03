using Microsoft.EntityFrameworkCore.Migrations;

namespace WarOfEmpires.Database.Migrations
{
    public partial class AlliancesWars : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Wars",
                schema: "Alliances",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wars", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AllianceWars",
                schema: "Alliances",
                columns: table => new
                {
                    AllianceId = table.Column<int>(type: "int", nullable: false),
                    WarId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AllianceWars", x => new { x.AllianceId, x.WarId });
                    table.ForeignKey(
                        name: "FK_AllianceWars_Alliances_AllianceId",
                        column: x => x.AllianceId,
                        principalSchema: "Alliances",
                        principalTable: "Alliances",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AllianceWars_Wars_WarId",
                        column: x => x.WarId,
                        principalSchema: "Alliances",
                        principalTable: "Wars",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PeaceDeclarations",
                schema: "Alliances",
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
                        name: "FK_PeaceDeclarations_Wars_WarId",
                        column: x => x.WarId,
                        principalSchema: "Alliances",
                        principalTable: "Wars",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AllianceWars_WarId",
                schema: "Alliances",
                table: "AllianceWars",
                column: "WarId");

            migrationBuilder.CreateIndex(
                name: "IX_PeaceDeclarations_WarId",
                schema: "Alliances",
                table: "PeaceDeclarations",
                column: "WarId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AllianceWars",
                schema: "Alliances");

            migrationBuilder.DropTable(
                name: "PeaceDeclarations",
                schema: "Alliances");

            migrationBuilder.DropTable(
                name: "Wars",
                schema: "Alliances");
        }
    }
}
