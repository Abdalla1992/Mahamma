using Microsoft.EntityFrameworkCore.Migrations;

namespace Mahamma.Notification.Api.Migrations
{
    public partial class addNotificationSedningType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "NotificationSendingType",
                keyColumn: "Id",
                keyValue: 3,
                column: "Name",
                value: "DeviceNotification");

            migrationBuilder.InsertData(
                table: "NotificationSendingType",
                columns: new[] { "Id", "Name" },
                values: new object[] { 4, "All" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "NotificationSendingType",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.UpdateData(
                table: "NotificationSendingType",
                keyColumn: "Id",
                keyValue: 3,
                column: "Name",
                value: "All");
        }
    }
}
