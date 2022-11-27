using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WarOfEmpires.Database.Migrations
{
    public partial class SecurityRefreshTokenFamilies : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RefreshTokenFamilies",
                schema: "Security",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CurrentToken = table.Column<byte[]>(type: "varbinary(20)", maxLength: 20, nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshTokenFamilies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RefreshTokenFamilies_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "Security",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ExpiredRefreshTokens",
                schema: "Security",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RefreshTokenFamilyId = table.Column<int>(type: "int", nullable: false),
                    Token = table.Column<byte[]>(type: "varbinary(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExpiredRefreshTokens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExpiredRefreshTokens_RefreshTokenFamilies_RefreshTokenFamilyId",
                        column: x => x.RefreshTokenFamilyId,
                        principalSchema: "Security",
                        principalTable: "RefreshTokenFamilies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExpiredRefreshTokens_RefreshTokenFamilyId",
                schema: "Security",
                table: "ExpiredRefreshTokens",
                column: "RefreshTokenFamilyId");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokenFamilies_UserId",
                schema: "Security",
                table: "RefreshTokenFamilies",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExpiredRefreshTokens",
                schema: "Security");

            migrationBuilder.DropTable(
                name: "RefreshTokenFamilies",
                schema: "Security");
        }
    }
}
