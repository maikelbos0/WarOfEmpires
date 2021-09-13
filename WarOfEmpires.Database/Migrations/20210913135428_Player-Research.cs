using Microsoft.EntityFrameworkCore.Migrations;

namespace WarOfEmpires.Database.Migrations
{
    public partial class PlayerResearch : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ResearchTypes",
                schema: "Empires",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResearchTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "QueuedResearch",
                schema: "Empires",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PlayerId = table.Column<int>(type: "int", nullable: false),
                    Priority = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    CompletedResearchTime = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QueuedResearch", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QueuedResearch_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalSchema: "Players",
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_QueuedResearch_ResearchTypes_Type",
                        column: x => x.Type,
                        principalSchema: "Empires",
                        principalTable: "ResearchTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Research",
                schema: "Empires",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Level = table.Column<int>(type: "int", nullable: false),
                    PlayerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Research", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Research_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalSchema: "Players",
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Research_ResearchTypes_Type",
                        column: x => x.Type,
                        principalSchema: "Empires",
                        principalTable: "ResearchTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                schema: "Empires",
                table: "ResearchTypes",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Efficiency" },
                    { 2, "Commerce" },
                    { 3, "Tactics" },
                    { 4, "Combat medicine" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_QueuedResearch_PlayerId",
                schema: "Empires",
                table: "QueuedResearch",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_QueuedResearch_Type",
                schema: "Empires",
                table: "QueuedResearch",
                column: "Type");

            migrationBuilder.CreateIndex(
                name: "IX_Research_PlayerId",
                schema: "Empires",
                table: "Research",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_Research_Type",
                schema: "Empires",
                table: "Research",
                column: "Type");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "QueuedResearch",
                schema: "Empires");

            migrationBuilder.DropTable(
                name: "Research",
                schema: "Empires");

            migrationBuilder.DropTable(
                name: "ResearchTypes",
                schema: "Empires");
        }
    }
}
