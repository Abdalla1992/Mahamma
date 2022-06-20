using Microsoft.EntityFrameworkCore.Migrations;

namespace Mahamma.Identity.Api.Migrations
{
    public partial class AddPageIdInPageLocalization : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DispalyName",
                table: "PageLocalization",
                newName: "DisplayName");

            migrationBuilder.AddColumn<int>(
                name: "PageId",
                table: "PageLocalization",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "PageLocalization",
                keyColumn: "Id",
                keyValue: 1,
                column: "PageId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "PageLocalization",
                keyColumn: "Id",
                keyValue: 2,
                column: "PageId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "PageLocalization",
                keyColumn: "Id",
                keyValue: 3,
                column: "PageId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "PageLocalization",
                keyColumn: "Id",
                keyValue: 4,
                column: "PageId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "PageLocalization",
                keyColumn: "Id",
                keyValue: 5,
                column: "PageId",
                value: 3);

            migrationBuilder.UpdateData(
                table: "PageLocalization",
                keyColumn: "Id",
                keyValue: 6,
                column: "PageId",
                value: 3);

            migrationBuilder.UpdateData(
                table: "PageLocalization",
                keyColumn: "Id",
                keyValue: 7,
                column: "PageId",
                value: 4);

            migrationBuilder.UpdateData(
                table: "PageLocalization",
                keyColumn: "Id",
                keyValue: 8,
                column: "PageId",
                value: 4);

            migrationBuilder.UpdateData(
                table: "PageLocalization",
                keyColumn: "Id",
                keyValue: 9,
                column: "PageId",
                value: 5);

            migrationBuilder.UpdateData(
                table: "PageLocalization",
                keyColumn: "Id",
                keyValue: 10,
                column: "PageId",
                value: 5);

            migrationBuilder.CreateIndex(
                name: "IX_PageLocalization_PageId",
                table: "PageLocalization",
                column: "PageId");

            migrationBuilder.AddForeignKey(
                name: "FK_PageLocalization_Page_PageId",
                table: "PageLocalization",
                column: "PageId",
                principalTable: "Page",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PageLocalization_Page_PageId",
                table: "PageLocalization");

            migrationBuilder.DropIndex(
                name: "IX_PageLocalization_PageId",
                table: "PageLocalization");

            migrationBuilder.DropColumn(
                name: "PageId",
                table: "PageLocalization");

            migrationBuilder.RenameColumn(
                name: "DisplayName",
                table: "PageLocalization",
                newName: "DispalyName");
        }
    }
}
