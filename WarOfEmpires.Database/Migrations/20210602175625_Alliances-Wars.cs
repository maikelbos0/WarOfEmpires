using Microsoft.EntityFrameworkCore.Migrations;

namespace WarOfEmpires.Database.Migrations
{
    public partial class AlliancesWars : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "War",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_War", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Wars",
                schema: "Alliances",
                columns: table => new
                {
                    AllianceId = table.Column<int>(type: "int", nullable: false),
                    WarId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wars", x => new { x.AllianceId, x.WarId });
                    table.ForeignKey(
                        name: "FK_Wars_Alliances_AllianceId",
                        column: x => x.AllianceId,
                        principalSchema: "Alliances",
                        principalTable: "Alliances",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Wars_War_WarId",
                        column: x => x.WarId,
                        principalTable: "War",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Wars_WarId",
                schema: "Alliances",
                table: "Wars",
                column: "WarId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Wars",
                schema: "Alliances");

            migrationBuilder.DropTable(
                name: "War");
        }
    }
}
