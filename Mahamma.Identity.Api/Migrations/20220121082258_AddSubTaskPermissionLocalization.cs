using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Mahamma.Identity.Api.Migrations
{
    public partial class AddSubTaskPermissionLocalization : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "PermissionLocalization",
                columns: new[] { "Id", "CreationDate", "DeletedStatus", "DisplayName", "LanguageId", "PermissionId" },
                values: new object[] { 73, new DateTime(2021, 11, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Add Sub Task", 2, 28 });

            migrationBuilder.InsertData(
                table: "PermissionLocalization",
                columns: new[] { "Id", "CreationDate", "DeletedStatus", "DisplayName", "LanguageId", "PermissionId" },
                values: new object[] { 74, new DateTime(2021, 11, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "اضافة مهمة فرعية", 2, 28 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "PermissionLocalization",
                keyColumn: "Id",
                keyValue: 73);

            migrationBuilder.DeleteData(
                table: "PermissionLocalization",
                keyColumn: "Id",
                keyValue: 74);
        }
    }
}
