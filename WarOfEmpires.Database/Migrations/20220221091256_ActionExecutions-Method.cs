using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WarOfEmpires.Database.Migrations
{
    public partial class ActionExecutionsMethod : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Method",
                schema: "Auditing",
                table: "ActionExecutions",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Method",
                schema: "Auditing",
                table: "ActionExecutions");
        }
    }
}
