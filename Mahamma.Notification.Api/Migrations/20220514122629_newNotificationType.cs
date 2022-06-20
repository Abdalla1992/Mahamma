using Microsoft.EntityFrameworkCore.Migrations;

namespace Mahamma.Notification.Api.Migrations
{
    public partial class newNotificationType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "NotificationType",
                columns: new[] { "Id", "Name" },
                values: new object[] { 28, "MinuteOfMeetingAdded" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "NotificationType",
                keyColumn: "Id",
                keyValue: 28);
        }
    }
}
