using Microsoft.EntityFrameworkCore.Migrations;

namespace WarOfEmpires.Database.Migrations
{
    public partial class SecurityColumnNames : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserEvents_UserEventTypes_UserEventType_Id",
                schema: "Security",
                table: "UserEvents");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_UserStatus_UserStatus_Id",
                schema: "Security",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "UserStatus_Id",
                schema: "Security",
                table: "Users",
                newName: "Status");

            migrationBuilder.RenameIndex(
                name: "IX_Users_UserStatus_Id",
                schema: "Security",
                table: "Users",
                newName: "IX_Users_Status");

            migrationBuilder.RenameColumn(
                name: "UserEventType_Id",
                schema: "Security",
                table: "UserEvents",
                newName: "Type");

            migrationBuilder.RenameIndex(
                name: "IX_UserEvents_UserEventType_Id",
                schema: "Security",
                table: "UserEvents",
                newName: "IX_UserEvents_Type");

            migrationBuilder.AddForeignKey(
                name: "FK_UserEvents_UserEventTypes_Type",
                schema: "Security",
                table: "UserEvents",
                column: "Type",
                principalSchema: "Security",
                principalTable: "UserEventTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_UserStatus_Status",
                schema: "Security",
                table: "Users",
                column: "Status",
                principalSchema: "Security",
                principalTable: "UserStatus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserEvents_UserEventTypes_Type",
                schema: "Security",
                table: "UserEvents");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_UserStatus_Status",
                schema: "Security",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "Status",
                schema: "Security",
                table: "Users",
                newName: "UserStatus_Id");

            migrationBuilder.RenameIndex(
                name: "IX_Users_Status",
                schema: "Security",
                table: "Users",
                newName: "IX_Users_UserStatus_Id");

            migrationBuilder.RenameColumn(
                name: "Type",
                schema: "Security",
                table: "UserEvents",
                newName: "UserEventType_Id");

            migrationBuilder.RenameIndex(
                name: "IX_UserEvents_Type",
                schema: "Security",
                table: "UserEvents",
                newName: "IX_UserEvents_UserEventType_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserEvents_UserEventTypes_UserEventType_Id",
                schema: "Security",
                table: "UserEvents",
                column: "UserEventType_Id",
                principalSchema: "Security",
                principalTable: "UserEventTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_UserStatus_UserStatus_Id",
                schema: "Security",
                table: "Users",
                column: "UserStatus_Id",
                principalSchema: "Security",
                principalTable: "UserStatus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
