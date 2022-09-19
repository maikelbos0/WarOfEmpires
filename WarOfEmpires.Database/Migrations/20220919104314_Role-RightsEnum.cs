using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WarOfEmpires.Database.Migrations
{
    public partial class RoleRightsEnum : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Rights",
                schema: "Alliances",
                table: "Roles",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Rights",
                schema: "Alliances",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rights", x => x.Id);
                });

            migrationBuilder.InsertData(
                schema: "Alliances",
                table: "Rights",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 0, "None" },
                    { 1, "Can invite" },
                    { 2, "Can manage roles" },
                    { 4, "Can delete chat messages" },
                    { 8, "Can kick members" },
                    { 16, "Can manage non aggression pacts" },
                    { 32, "Can manage wars" },
                    { 64, "Can bank" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Rights",
                schema: "Alliances");

            migrationBuilder.DropColumn(
                name: "Rights",
                schema: "Alliances",
                table: "Roles");
        }
    }
}
