using Microsoft.EntityFrameworkCore.Migrations;

namespace WarOfEmpires.Database.Migrations
{
    public partial class CasualtiesFixes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Casualties_AttackRounds_AttackRoundId",
                schema: "Attacks",
                table: "Casualties");

            migrationBuilder.AlterColumn<int>(
                name: "AttackRoundId",
                schema: "Attacks",
                table: "Casualties",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Casualties_TroopType",
                schema: "Attacks",
                table: "Casualties",
                column: "TroopType");

            migrationBuilder.AddForeignKey(
                name: "FK_Casualties_AttackRounds_AttackRoundId",
                schema: "Attacks",
                table: "Casualties",
                column: "AttackRoundId",
                principalSchema: "Attacks",
                principalTable: "AttackRounds",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Casualties_TroopTypes_TroopType",
                schema: "Attacks",
                table: "Casualties",
                column: "TroopType",
                principalSchema: "Attacks",
                principalTable: "TroopTypes",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Casualties_AttackRounds_AttackRoundId",
                schema: "Attacks",
                table: "Casualties");

            migrationBuilder.DropForeignKey(
                name: "FK_Casualties_TroopTypes_TroopType",
                schema: "Attacks",
                table: "Casualties");

            migrationBuilder.DropIndex(
                name: "IX_Casualties_TroopType",
                schema: "Attacks",
                table: "Casualties");

            migrationBuilder.AlterColumn<int>(
                name: "AttackRoundId",
                schema: "Attacks",
                table: "Casualties",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Casualties_AttackRounds_AttackRoundId",
                schema: "Attacks",
                table: "Casualties",
                column: "AttackRoundId",
                principalSchema: "Attacks",
                principalTable: "AttackRounds",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
