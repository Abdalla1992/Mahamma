using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Mahamma.Identity.Api.Migrations
{
    public partial class AddViewMeetingPermission : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Permission",
                columns: new[] { "Id", "Name" },
                values: new object[] { 36, "ViewMeeting" });

            migrationBuilder.InsertData(
                table: "PagePermission",
                columns: new[] { "Id", "CreationDate", "DeletedStatus", "PageId", "PermissionId" },
                values: new object[] { 54, new DateTime(2021, 10, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 8, 36 });

            migrationBuilder.InsertData(
                table: "PermissionLocalization",
                columns: new[] { "Id", "CreationDate", "DeletedStatus", "DisplayName", "LanguageId", "PermissionId" },
                values: new object[] { 61, new DateTime(2021, 11, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "View Meeting", 1, 36 });

            migrationBuilder.InsertData(
                table: "PermissionLocalization",
                columns: new[] { "Id", "CreationDate", "DeletedStatus", "DisplayName", "LanguageId", "PermissionId" },
                values: new object[] { 62, new DateTime(2021, 11, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "مشاهدة الاجتماع", 2, 36 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "PagePermission",
                keyColumn: "Id",
                keyValue: 54);

            migrationBuilder.DeleteData(
                table: "PermissionLocalization",
                keyColumn: "Id",
                keyValue: 61);

            migrationBuilder.DeleteData(
                table: "PermissionLocalization",
                keyColumn: "Id",
                keyValue: 62);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 36);
        }
    }
}
