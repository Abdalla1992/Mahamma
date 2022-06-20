using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Mahamma.Api.Migrations
{
    public partial class AddFolderEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Folder",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProjectId = table.Column<int>(type: "int", nullable: false),
                    TaskId = table.Column<int>(type: "int", nullable: true),
                    CreationDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    DeletedStatus = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Folder", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Folder_Project_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Project",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Folder_Task_TaskId",
                        column: x => x.TaskId,
                        principalTable: "Task",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
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
                name: "IX_Folder_ProjectId",
                table: "Folder",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Folder_TaskId",
                table: "Folder",
                column: "TaskId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Folder");

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
