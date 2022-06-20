using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Mahamma.Identity.Api.Migrations
{
    public partial class RemoveViewChartPrmission : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "PageLocalization",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "PageLocalization",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "PageLocalization",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "PagePermission",
                keyColumn: "Id",
                keyValue: 48);

            migrationBuilder.DeleteData(
                table: "PermissionLocalization",
                keyColumn: "Id",
                keyValue: 46);

            migrationBuilder.DeleteData(
                table: "PermissionLocalization",
                keyColumn: "Id",
                keyValue: 47);

            migrationBuilder.DeleteData(
                table: "PermissionLocalization",
                keyColumn: "Id",
                keyValue: 48);

            migrationBuilder.DeleteData(
                table: "Page",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 29);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Page",
                columns: new[] { "Id", "CreationDate", "DeletedStatus", "Name" },
                values: new object[] { 6, new DateTime(2021, 10, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "DashboardProfile" });

            migrationBuilder.InsertData(
                table: "PageLocalization",
                columns: new[] { "Id", "CreationDate", "DeletedStatus", "DisplayName", "LanguageId", "PageId" },
                values: new object[] { 10, new DateTime(2021, 10, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "تنظيم الادوار", 2, 5 });

            migrationBuilder.InsertData(
                table: "Permission",
                columns: new[] { "Id", "Name" },
                values: new object[] { 29, "ViewCharts" });

            migrationBuilder.InsertData(
                table: "PermissionLocalization",
                columns: new[] { "Id", "CreationDate", "DeletedStatus", "DisplayName", "LanguageId", "PermissionId" },
                values: new object[] { 46, new DateTime(2021, 11, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "مسح دور", 2, 23 });

            migrationBuilder.InsertData(
                table: "PageLocalization",
                columns: new[] { "Id", "CreationDate", "DeletedStatus", "DisplayName", "LanguageId", "PageId" },
                values: new object[,]
                {
                    { 11, new DateTime(2021, 10, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Dashboard", 2, 6 },
                    { 12, new DateTime(2021, 10, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "لوحة القيادة", 2, 6 }
                });

            migrationBuilder.InsertData(
                table: "PagePermission",
                columns: new[] { "Id", "CreationDate", "DeletedStatus", "PageId", "PermissionId" },
                values: new object[] { 48, new DateTime(2021, 10, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 6, 29 });

            migrationBuilder.InsertData(
                table: "PermissionLocalization",
                columns: new[] { "Id", "CreationDate", "DeletedStatus", "DisplayName", "LanguageId", "PermissionId" },
                values: new object[,]
                {
                    { 47, new DateTime(2021, 11, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "View Dashboard", 1, 29 },
                    { 48, new DateTime(2021, 11, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "عرض لوحة القيادة", 2, 29 }
                });
        }
    }
}
