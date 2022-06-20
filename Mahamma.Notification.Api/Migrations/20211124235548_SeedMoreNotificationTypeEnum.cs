using Microsoft.EntityFrameworkCore.Migrations;

namespace Mahamma.Notification.Api.Migrations
{
    public partial class SeedMoreNotificationTypeEnum : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "NotificationSendingStatus",
                keyColumn: "Id",
                keyValue: 2,
                column: "Name",
                value: "Sent");

            migrationBuilder.InsertData(
                table: "NotificationType",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 15, "AddWorkspace" },
                    { 16, "UpdateWorkspace" },
                    { 17, "AddSubTask" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "NotificationType",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "NotificationType",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "NotificationType",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.UpdateData(
                table: "NotificationSendingStatus",
                keyColumn: "Id",
                keyValue: 2,
                column: "Name",
                value: "Send");
        }
    }
}
