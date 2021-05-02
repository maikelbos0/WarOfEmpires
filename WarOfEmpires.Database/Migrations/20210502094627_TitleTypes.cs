using Microsoft.EntityFrameworkCore.Migrations;

namespace WarOfEmpires.Database.Migrations
{
    public partial class TitleTypes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TitleTypes",
                schema: "Players",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TitleTypes", x => x.Id);
                });

            migrationBuilder.InsertData(
                schema: "Players",
                table: "TitleTypes",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Peasant leader" },
                    { 2, "Bandit leader" },
                    { 3, "Warband leader" },
                    { 4, "Sub chieftain" },
                    { 5, "Chief" },
                    { 6, "Warlord" },
                    { 7, "Baron" },
                    { 8, "Viscount" },
                    { 9, "Earl" },
                    { 10, "Marquis" },
                    { 11, "Duke" },
                    { 12, "Prince" },
                    { 13, "King" },
                    { 14, "Emperor" },
                    { 15, "Overlord" },
                    { 16, "Grand overlord" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Players_Title",
                schema: "Players",
                table: "Players",
                column: "Title");

            migrationBuilder.AddForeignKey(
                name: "FK_Players_TitleTypes_Title",
                schema: "Players",
                table: "Players",
                column: "Title",
                principalSchema: "Players",
                principalTable: "TitleTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Players_TitleTypes_Title",
                schema: "Players",
                table: "Players");

            migrationBuilder.DropTable(
                name: "TitleTypes",
                schema: "Players");

            migrationBuilder.DropIndex(
                name: "IX_Players_Title",
                schema: "Players",
                table: "Players");
        }
    }
}
