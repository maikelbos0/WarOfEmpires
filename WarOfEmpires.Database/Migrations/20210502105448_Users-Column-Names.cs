using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WarOfEmpires.Database.Migrations
{
    public partial class UsersColumnNames : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Password_Salt",
                schema: "Security",
                table: "Users",
                newName: "PasswordSalt");

            migrationBuilder.RenameColumn(
                name: "Password_HashIterations",
                schema: "Security",
                table: "Users",
                newName: "PasswordHashIterations");

            migrationBuilder.RenameColumn(
                name: "Password_Hash",
                schema: "Security",
                table: "Users",
                newName: "PasswordHash");

            migrationBuilder.RenameColumn(
                name: "PasswordResetToken_Salt",
                schema: "Security",
                table: "Users",
                newName: "PasswordResetTokenSalt");

            migrationBuilder.RenameColumn(
                name: "PasswordResetToken_HashIterations",
                schema: "Security",
                table: "Users",
                newName: "PasswordResetTokenHashIterations");

            migrationBuilder.RenameColumn(
                name: "PasswordResetToken_Hash",
                schema: "Security",
                table: "Users",
                newName: "PasswordResetTokenHash");

            migrationBuilder.RenameColumn(
                name: "PasswordResetToken_ExpiryDate",
                schema: "Security",
                table: "Users",
                newName: "PasswordResetTokenExpiryDate");

            migrationBuilder.AlterColumn<byte[]>(
                name: "PasswordSalt",
                schema: "Security",
                table: "Users",
                type: "varbinary(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(byte[]),
                oldType: "varbinary(20)",
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "PasswordHashIterations",
                schema: "Security",
                table: "Users",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<byte[]>(
                name: "PasswordHash",
                schema: "Security",
                table: "Users",
                type: "varbinary(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(byte[]),
                oldType: "varbinary(20)",
                oldMaxLength: 20,
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PasswordSalt",
                schema: "Security",
                table: "Users",
                newName: "Password_Salt");

            migrationBuilder.RenameColumn(
                name: "PasswordResetTokenSalt",
                schema: "Security",
                table: "Users",
                newName: "PasswordResetToken_Salt");

            migrationBuilder.RenameColumn(
                name: "PasswordResetTokenHashIterations",
                schema: "Security",
                table: "Users",
                newName: "PasswordResetToken_HashIterations");

            migrationBuilder.RenameColumn(
                name: "PasswordResetTokenHash",
                schema: "Security",
                table: "Users",
                newName: "PasswordResetToken_Hash");

            migrationBuilder.RenameColumn(
                name: "PasswordResetTokenExpiryDate",
                schema: "Security",
                table: "Users",
                newName: "PasswordResetToken_ExpiryDate");

            migrationBuilder.RenameColumn(
                name: "PasswordHashIterations",
                schema: "Security",
                table: "Users",
                newName: "Password_HashIterations");

            migrationBuilder.RenameColumn(
                name: "PasswordHash",
                schema: "Security",
                table: "Users",
                newName: "Password_Hash");

            migrationBuilder.AlterColumn<byte[]>(
                name: "Password_Salt",
                schema: "Security",
                table: "Users",
                type: "varbinary(20)",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(byte[]),
                oldType: "varbinary(20)",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<int>(
                name: "Password_HashIterations",
                schema: "Security",
                table: "Users",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<byte[]>(
                name: "Password_Hash",
                schema: "Security",
                table: "Users",
                type: "varbinary(20)",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(byte[]),
                oldType: "varbinary(20)",
                oldMaxLength: 20);
        }
    }
}
