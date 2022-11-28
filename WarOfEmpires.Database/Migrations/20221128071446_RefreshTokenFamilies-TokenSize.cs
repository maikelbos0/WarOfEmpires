using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WarOfEmpires.Database.Migrations
{
    public partial class RefreshTokenFamiliesTokenSize : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<byte[]>(
                name: "CurrentToken",
                schema: "Security",
                table: "RefreshTokenFamilies",
                type: "varbinary(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(byte[]),
                oldType: "varbinary(20)",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<byte[]>(
                name: "Token",
                schema: "Security",
                table: "ExpiredRefreshTokens",
                type: "varbinary(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(byte[]),
                oldType: "varbinary(20)",
                oldMaxLength: 20);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<byte[]>(
                name: "CurrentToken",
                schema: "Security",
                table: "RefreshTokenFamilies",
                type: "varbinary(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(byte[]),
                oldType: "varbinary(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<byte[]>(
                name: "Token",
                schema: "Security",
                table: "ExpiredRefreshTokens",
                type: "varbinary(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(byte[]),
                oldType: "varbinary(100)",
                oldMaxLength: 100);
        }
    }
}
