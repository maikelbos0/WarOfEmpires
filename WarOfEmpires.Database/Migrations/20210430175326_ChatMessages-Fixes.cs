using Microsoft.EntityFrameworkCore.Migrations;

namespace WarOfEmpires.Database.Migrations
{
    public partial class ChatMessagesFixes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChatMessages_Players_PlayerId",
                schema: "Alliances",
                table: "ChatMessages");

            migrationBuilder.AlterColumn<int>(
                name: "PlayerId",
                schema: "Alliances",
                table: "ChatMessages",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ChatMessages_Players_PlayerId",
                schema: "Alliances",
                table: "ChatMessages",
                column: "PlayerId",
                principalSchema: "Players",
                principalTable: "Players",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChatMessages_Players_PlayerId",
                schema: "Alliances",
                table: "ChatMessages");

            migrationBuilder.AlterColumn<int>(
                name: "PlayerId",
                schema: "Alliances",
                table: "ChatMessages",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_ChatMessages_Players_PlayerId",
                schema: "Alliances",
                table: "ChatMessages",
                column: "PlayerId",
                principalSchema: "Players",
                principalTable: "Players",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
