using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Mahamma.Notification.Api.Migrations
{
    public partial class AddNotficationSheduleEntityAndTypeEnum : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "NotificationSheduleType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificationSheduleType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NotificationShedule",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    From = table.Column<TimeSpan>(type: "time", nullable: false),
                    To = table.Column<TimeSpan>(type: "time", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    NotificationScheduleTypeId = table.Column<int>(type: "int", nullable: false),
                    WeekDayId = table.Column<int>(type: "int", nullable: true),
                    MonthDayId = table.Column<int>(type: "int", nullable: true),
                    CreationDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    DeletedStatus = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificationShedule", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NotificationShedule_NotificationSheduleType_NotificationScheduleTypeId",
                        column: x => x.NotificationScheduleTypeId,
                        principalTable: "NotificationSheduleType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "NotificationSheduleType",
                columns: new[] { "Id", "Name" },
                values: new object[] { 1, "Daily" });

            migrationBuilder.InsertData(
                table: "NotificationSheduleType",
                columns: new[] { "Id", "Name" },
                values: new object[] { 2, "Weekly" });

            migrationBuilder.InsertData(
                table: "NotificationSheduleType",
                columns: new[] { "Id", "Name" },
                values: new object[] { 3, "Monthly" });

            migrationBuilder.CreateIndex(
                name: "IX_NotificationShedule_NotificationScheduleTypeId",
                table: "NotificationShedule",
                column: "NotificationScheduleTypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NotificationShedule");

            migrationBuilder.DropTable(
                name: "NotificationSheduleType");
        }
    }
}
