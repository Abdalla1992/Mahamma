using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Mahamma.Api.Migrations
{
    public partial class addMeetingRole : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MeetingRole",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MeetingRole", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MeetingMemberRoles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    MeetingRoleId = table.Column<int>(type: "int", nullable: false),
                    MeetingMemberId = table.Column<int>(type: "int", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    DeletedStatus = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MeetingMemberRoles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MeetingMemberRoles_MeetingMembers_MeetingMemberId",
                        column: x => x.MeetingMemberId,
                        principalTable: "MeetingMembers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MeetingMemberRoles_MeetingRole_MeetingRoleId",
                        column: x => x.MeetingRoleId,
                        principalTable: "MeetingRole",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "MeetingRole",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "creator" },
                    { 3, "speaker" },
                    { 2, "presenter" },
                    { 4, "minuteofmeetingwriter" }
                });

            migrationBuilder.UpdateData(
                table: "TaskStatus",
                keyColumn: "Id",
                keyValue: 2,
                column: "Name",
                value: "InProgress");

            migrationBuilder.UpdateData(
                table: "TaskStatus",
                keyColumn: "Id",
                keyValue: 3,
                column: "Name",
                value: "InProgressWithDelay");

            migrationBuilder.UpdateData(
                table: "TaskStatus",
                keyColumn: "Id",
                keyValue: 4,
                column: "Name",
                value: "CompletedEarly");

            migrationBuilder.UpdateData(
                table: "TaskStatus",
                keyColumn: "Id",
                keyValue: 5,
                column: "Name",
                value: "CompletedOnTime");

            migrationBuilder.UpdateData(
                table: "TaskStatus",
                keyColumn: "Id",
                keyValue: 6,
                column: "Name",
                value: "CompletedLate");

            migrationBuilder.CreateIndex(
                name: "IX_MeetingMemberRoles_MeetingMemberId",
                table: "MeetingMemberRoles",
                column: "MeetingMemberId");

            migrationBuilder.CreateIndex(
                name: "IX_MeetingMemberRoles_MeetingRoleId",
                table: "MeetingMemberRoles",
                column: "MeetingRoleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MeetingMemberRoles");

            migrationBuilder.DropTable(
                name: "MeetingRole");

            migrationBuilder.UpdateData(
                table: "TaskStatus",
                keyColumn: "Id",
                keyValue: 2,
                column: "Name",
                value: "In Progress");

            migrationBuilder.UpdateData(
                table: "TaskStatus",
                keyColumn: "Id",
                keyValue: 3,
                column: "Name",
                value: "In Progress With Delay");

            migrationBuilder.UpdateData(
                table: "TaskStatus",
                keyColumn: "Id",
                keyValue: 4,
                column: "Name",
                value: "Completed Early");

            migrationBuilder.UpdateData(
                table: "TaskStatus",
                keyColumn: "Id",
                keyValue: 5,
                column: "Name",
                value: "Completed On Time");

            migrationBuilder.UpdateData(
                table: "TaskStatus",
                keyColumn: "Id",
                keyValue: 6,
                column: "Name",
                value: "Completed Late");
        }
    }
}
