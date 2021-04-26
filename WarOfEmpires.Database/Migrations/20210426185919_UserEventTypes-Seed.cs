using Microsoft.EntityFrameworkCore.Migrations;

namespace WarOfEmpires.Database.Migrations
{
    public partial class UserEventTypesSeed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                schema: "Security",
                table: "UserEventTypes",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { (byte)1, "Logged in" },
                    { (byte)17, "Email changed" },
                    { (byte)16, "Failed email change request" },
                    { (byte)15, "Email change requested" },
                    { (byte)14, "Failed deactivation" },
                    { (byte)13, "Deactivated" },
                    { (byte)12, "Failed password reset" },
                    { (byte)11, "Password reset" },
                    { (byte)18, "Failed email change" },
                    { (byte)10, "Password reset requested" },
                    { (byte)8, "Password changed" },
                    { (byte)7, "Logged out" },
                    { (byte)6, "Activation code sent" },
                    { (byte)5, "Failed activation" },
                    { (byte)4, "Activated" },
                    { (byte)3, "Registered" },
                    { (byte)2, "Failed log in" },
                    { (byte)9, "Failed password change" },
                    { (byte)19, "Failed password reset request" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "Security",
                table: "UserEventTypes",
                keyColumn: "Id",
                keyValue: (byte)1);

            migrationBuilder.DeleteData(
                schema: "Security",
                table: "UserEventTypes",
                keyColumn: "Id",
                keyValue: (byte)2);

            migrationBuilder.DeleteData(
                schema: "Security",
                table: "UserEventTypes",
                keyColumn: "Id",
                keyValue: (byte)3);

            migrationBuilder.DeleteData(
                schema: "Security",
                table: "UserEventTypes",
                keyColumn: "Id",
                keyValue: (byte)4);

            migrationBuilder.DeleteData(
                schema: "Security",
                table: "UserEventTypes",
                keyColumn: "Id",
                keyValue: (byte)5);

            migrationBuilder.DeleteData(
                schema: "Security",
                table: "UserEventTypes",
                keyColumn: "Id",
                keyValue: (byte)6);

            migrationBuilder.DeleteData(
                schema: "Security",
                table: "UserEventTypes",
                keyColumn: "Id",
                keyValue: (byte)7);

            migrationBuilder.DeleteData(
                schema: "Security",
                table: "UserEventTypes",
                keyColumn: "Id",
                keyValue: (byte)8);

            migrationBuilder.DeleteData(
                schema: "Security",
                table: "UserEventTypes",
                keyColumn: "Id",
                keyValue: (byte)9);

            migrationBuilder.DeleteData(
                schema: "Security",
                table: "UserEventTypes",
                keyColumn: "Id",
                keyValue: (byte)10);

            migrationBuilder.DeleteData(
                schema: "Security",
                table: "UserEventTypes",
                keyColumn: "Id",
                keyValue: (byte)11);

            migrationBuilder.DeleteData(
                schema: "Security",
                table: "UserEventTypes",
                keyColumn: "Id",
                keyValue: (byte)12);

            migrationBuilder.DeleteData(
                schema: "Security",
                table: "UserEventTypes",
                keyColumn: "Id",
                keyValue: (byte)13);

            migrationBuilder.DeleteData(
                schema: "Security",
                table: "UserEventTypes",
                keyColumn: "Id",
                keyValue: (byte)14);

            migrationBuilder.DeleteData(
                schema: "Security",
                table: "UserEventTypes",
                keyColumn: "Id",
                keyValue: (byte)15);

            migrationBuilder.DeleteData(
                schema: "Security",
                table: "UserEventTypes",
                keyColumn: "Id",
                keyValue: (byte)16);

            migrationBuilder.DeleteData(
                schema: "Security",
                table: "UserEventTypes",
                keyColumn: "Id",
                keyValue: (byte)17);

            migrationBuilder.DeleteData(
                schema: "Security",
                table: "UserEventTypes",
                keyColumn: "Id",
                keyValue: (byte)18);

            migrationBuilder.DeleteData(
                schema: "Security",
                table: "UserEventTypes",
                keyColumn: "Id",
                keyValue: (byte)19);
        }
    }
}
