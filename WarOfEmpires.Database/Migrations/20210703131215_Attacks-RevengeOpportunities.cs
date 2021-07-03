using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WarOfEmpires.Database.Migrations
{
    public partial class AttacksRevengeOpportunities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RevengeOpportunities",
                schema: "Attacks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TargetId = table.Column<int>(type: "int", nullable: false),
                    ExpirationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PlayerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RevengeOpportunities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RevengeOpportunities_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalSchema: "Players",
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RevengeOpportunities_Players_TargetId",
                        column: x => x.TargetId,
                        principalSchema: "Players",
                        principalTable: "Players",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_RevengeOpportunities_PlayerId",
                schema: "Attacks",
                table: "RevengeOpportunities",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_RevengeOpportunities_TargetId",
                schema: "Attacks",
                table: "RevengeOpportunities",
                column: "TargetId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RevengeOpportunities",
                schema: "Attacks");
        }
    }
}
