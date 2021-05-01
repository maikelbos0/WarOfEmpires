using Microsoft.EntityFrameworkCore.Migrations;

namespace WarOfEmpires.Database.Migrations
{
    public partial class CaravansFixes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Players_PlayerId",
                schema: "Markets",
                table: "Transactions");

            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Players_PlayerId1",
                schema: "Markets",
                table: "Transactions");

            migrationBuilder.RenameColumn(
                name: "PlayerId1",
                schema: "Markets",
                table: "Transactions",
                newName: "SellerId");

            migrationBuilder.RenameColumn(
                name: "PlayerId",
                schema: "Markets",
                table: "Transactions",
                newName: "BuyerId");

            migrationBuilder.RenameIndex(
                name: "IX_Transactions_PlayerId1",
                schema: "Markets",
                table: "Transactions",
                newName: "IX_Transactions_SellerId");

            migrationBuilder.RenameIndex(
                name: "IX_Transactions_PlayerId",
                schema: "Markets",
                table: "Transactions",
                newName: "IX_Transactions_BuyerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Players_BuyerId",
                schema: "Markets",
                table: "Transactions",
                column: "BuyerId",
                principalSchema: "Players",
                principalTable: "Players",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Players_SellerId",
                schema: "Markets",
                table: "Transactions",
                column: "SellerId",
                principalSchema: "Players",
                principalTable: "Players",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Players_BuyerId",
                schema: "Markets",
                table: "Transactions");

            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Players_SellerId",
                schema: "Markets",
                table: "Transactions");

            migrationBuilder.RenameColumn(
                name: "SellerId",
                schema: "Markets",
                table: "Transactions",
                newName: "PlayerId1");

            migrationBuilder.RenameColumn(
                name: "BuyerId",
                schema: "Markets",
                table: "Transactions",
                newName: "PlayerId");

            migrationBuilder.RenameIndex(
                name: "IX_Transactions_SellerId",
                schema: "Markets",
                table: "Transactions",
                newName: "IX_Transactions_PlayerId1");

            migrationBuilder.RenameIndex(
                name: "IX_Transactions_BuyerId",
                schema: "Markets",
                table: "Transactions",
                newName: "IX_Transactions_PlayerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Players_PlayerId",
                schema: "Markets",
                table: "Transactions",
                column: "PlayerId",
                principalSchema: "Players",
                principalTable: "Players",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Players_PlayerId1",
                schema: "Markets",
                table: "Transactions",
                column: "PlayerId1",
                principalSchema: "Players",
                principalTable: "Players",
                principalColumn: "Id");
        }
    }
}
