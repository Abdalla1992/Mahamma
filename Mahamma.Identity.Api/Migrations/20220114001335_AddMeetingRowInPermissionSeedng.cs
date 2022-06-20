using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Mahamma.Identity.Api.Migrations
{
    public partial class AddMeetingRowInPermissionSeedng : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Permission",
                columns: new[] { "Id", "Name" },
                values: new object[] { 41, "AddMeeting" });

            migrationBuilder.InsertData(
                table: "PagePermission",
                columns: new[] { "Id", "CreationDate", "DeletedStatus", "PageId", "PermissionId" },
                values: new object[] { 63, new DateTime(2021, 10, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 8, 41 });

            migrationBuilder.InsertData(
                table: "PermissionLocalization",
                columns: new[] { "Id", "CreationDate", "DeletedStatus", "DisplayName", "LanguageId", "PermissionId" },
                values: new object[] { 71, new DateTime(2021, 11, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Add Meeting", 1, 41 });

            migrationBuilder.InsertData(
                table: "PermissionLocalization",
                columns: new[] { "Id", "CreationDate", "DeletedStatus", "DisplayName", "LanguageId", "PermissionId" },
                values: new object[] { 72, new DateTime(2021, 11, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "اضافة اجتماع", 2, 41 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "PagePermission",
                keyColumn: "Id",
                keyValue: 63);

            migrationBuilder.DeleteData(
                table: "PermissionLocalization",
                keyColumn: "Id",
                keyValue: 71);

            migrationBuilder.DeleteData(
                table: "PermissionLocalization",
                keyColumn: "Id",
                keyValue: 72);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 41);
        }
    }
}
