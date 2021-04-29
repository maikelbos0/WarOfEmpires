using Microsoft.EntityFrameworkCore.Migrations;

namespace WarOfEmpires.Database.Migrations
{
    public partial class AllianceFixes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Alliances_Players_LeaderId",
                schema: "Alliances",
                table: "Alliances");

            migrationBuilder.AlterColumn<int>(
                name: "LeaderId",
                schema: "Alliances",
                table: "Alliances",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                schema: "Alliances",
                table: "Alliances",
                type: "nvarchar(4)",
                maxLength: 4,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddForeignKey(
                name: "FK_Alliances_Players_LeaderId",
                schema: "Alliances",
                table: "Alliances",
                column: "LeaderId",
                principalSchema: "Players",
                principalTable: "Players",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Alliances_Players_LeaderId",
                schema: "Alliances",
                table: "Alliances");

            migrationBuilder.AlterColumn<int>(
                name: "LeaderId",
                schema: "Alliances",
                table: "Alliances",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                schema: "Alliances",
                table: "Alliances",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(4)",
                oldMaxLength: 4);

            migrationBuilder.AddForeignKey(
                name: "FK_Alliances_Players_LeaderId",
                schema: "Alliances",
                table: "Alliances",
                column: "LeaderId",
                principalSchema: "Players",
                principalTable: "Players",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
