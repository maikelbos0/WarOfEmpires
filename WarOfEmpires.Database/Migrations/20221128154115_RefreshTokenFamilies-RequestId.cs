using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WarOfEmpires.Database.Migrations
{
    public partial class RefreshTokenFamiliesRequestId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExpiredRefreshTokens",
                schema: "Security");

            migrationBuilder.AddColumn<Guid>(
                name: "RequestId",
                schema: "Security",
                table: "RefreshTokenFamilies",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "PreviousRefreshTokens",
                schema: "Security",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Token = table.Column<byte[]>(type: "varbinary(100)", maxLength: 100, nullable: false),
                    RefreshTokenFamilyId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PreviousRefreshTokens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PreviousRefreshTokens_RefreshTokenFamilies_RefreshTokenFamilyId",
                        column: x => x.RefreshTokenFamilyId,
                        principalSchema: "Security",
                        principalTable: "RefreshTokenFamilies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                schema: "Security",
                table: "UserEventTypes",
                columns: new[] { "Id", "Name" },
                values: new object[] { (byte)20, "Refresh token generated" });

            migrationBuilder.InsertData(
                schema: "Security",
                table: "UserEventTypes",
                columns: new[] { "Id", "Name" },
                values: new object[] { (byte)21, "Refresh token rotated" });

            migrationBuilder.InsertData(
                schema: "Security",
                table: "UserEventTypes",
                columns: new[] { "Id", "Name" },
                values: new object[] { (byte)22, "Failed refresh token rotation" });

            migrationBuilder.CreateIndex(
                name: "IX_PreviousRefreshTokens_RefreshTokenFamilyId",
                schema: "Security",
                table: "PreviousRefreshTokens",
                column: "RefreshTokenFamilyId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PreviousRefreshTokens",
                schema: "Security");

            migrationBuilder.DeleteData(
                schema: "Security",
                table: "UserEventTypes",
                keyColumn: "Id",
                keyValue: (byte)20);

            migrationBuilder.DeleteData(
                schema: "Security",
                table: "UserEventTypes",
                keyColumn: "Id",
                keyValue: (byte)21);

            migrationBuilder.DeleteData(
                schema: "Security",
                table: "UserEventTypes",
                keyColumn: "Id",
                keyValue: (byte)22);

            migrationBuilder.DropColumn(
                name: "RequestId",
                schema: "Security",
                table: "RefreshTokenFamilies");

            migrationBuilder.CreateTable(
                name: "ExpiredRefreshTokens",
                schema: "Security",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RefreshTokenFamilyId = table.Column<int>(type: "int", nullable: false),
                    Token = table.Column<byte[]>(type: "varbinary(100)", maxLength: 100, nullable: false)
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
        }
    }
}
