using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Mahamma.Identity.Api.Migrations
{
    public partial class RemoveMangeMinutOfMeetin : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "PageLocalization",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "PageLocalization",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "PageLocalization",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "PagePermission",
                keyColumn: "Id",
                keyValue: 58);

            migrationBuilder.DeleteData(
                table: "PagePermission",
                keyColumn: "Id",
                keyValue: 59);

            migrationBuilder.DeleteData(
                table: "PagePermission",
                keyColumn: "Id",
                keyValue: 60);

            migrationBuilder.DeleteData(
                table: "PagePermission",
                keyColumn: "Id",
                keyValue: 61);

            migrationBuilder.DeleteData(
                table: "PagePermission",
                keyColumn: "Id",
                keyValue: 62);

            migrationBuilder.DeleteData(
                table: "Page",
                keyColumn: "Id",
                keyValue: 9);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Page",
                columns: new[] { "Id", "CreationDate", "DeletedStatus", "Name" },
                values: new object[] { 9, new DateTime(2021, 10, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "ManageMinutesOfMeetings" });

            migrationBuilder.InsertData(
                table: "PageLocalization",
                columns: new[] { "Id", "CreationDate", "DeletedStatus", "DisplayName", "LanguageId", "PageId" },
                values: new object[] { 16, new DateTime(2021, 10, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "تنظيم الاجتماعات", 2, 8 });

            migrationBuilder.InsertData(
                table: "PagePermission",
                columns: new[] { "Id", "CreationDate", "DeletedStatus", "PageId", "PermissionId" },
                values: new object[] { 58, new DateTime(2021, 10, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 7, 38 });

            migrationBuilder.InsertData(
                table: "PageLocalization",
                columns: new[] { "Id", "CreationDate", "DeletedStatus", "DisplayName", "LanguageId", "PageId" },
                values: new object[,]
                {
                    { 17, new DateTime(2021, 10, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Manage Minutes Of Meetings", 1, 9 },
                    { 18, new DateTime(2021, 10, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "تنظيم النقاط الخاصة بالاجتماع", 2, 9 }
                });

            migrationBuilder.InsertData(
                table: "PagePermission",
                columns: new[] { "Id", "CreationDate", "DeletedStatus", "PageId", "PermissionId" },
                values: new object[,]
                {
                    { 59, new DateTime(2021, 10, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 9, 40 },
                    { 60, new DateTime(2021, 10, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 9, 39 },
                    { 61, new DateTime(2021, 10, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 9, 37 },
                    { 62, new DateTime(2021, 10, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 9, 38 }
                });
        }
    }
}
