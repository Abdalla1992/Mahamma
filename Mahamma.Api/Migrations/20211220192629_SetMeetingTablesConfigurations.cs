using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Mahamma.Api.Migrations
{
    public partial class SetMeetingTablesConfigurations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MeetingAgendaTopics_Meeting_MeetingId",
                table: "MeetingAgendaTopics");

            migrationBuilder.DropForeignKey(
                name: "FK_MeetingMembers_Meeting_MeetingId",
                table: "MeetingMembers");

            migrationBuilder.DropForeignKey(
                name: "FK_MinutesOfMeeting_Meeting_MeetingId",
                table: "MinutesOfMeeting");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MinutesOfMeeting",
                table: "MinutesOfMeeting");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Meeting",
                table: "Meeting");

            migrationBuilder.RenameTable(
                name: "MinutesOfMeeting",
                newName: "MinutesOfMeetings");

            migrationBuilder.RenameTable(
                name: "Meeting",
                newName: "Meetings");

            migrationBuilder.RenameIndex(
                name: "IX_MinutesOfMeeting_MeetingId",
                table: "MinutesOfMeetings",
                newName: "IX_MinutesOfMeetings_MeetingId");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreationDate",
                table: "MeetingMembers",
                type: "datetime",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<string>(
                name: "Topic",
                table: "MeetingAgendaTopics",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreationDate",
                table: "MeetingAgendaTopics",
                type: "datetime",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "MinutesOfMeetings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "MinutesOfMeetings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreationDate",
                table: "MinutesOfMeetings",
                type: "datetime",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Meetings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date",
                table: "Meetings",
                type: "datetime",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreationDate",
                table: "Meetings",
                type: "datetime",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MinutesOfMeetings",
                table: "MinutesOfMeetings",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Meetings",
                table: "Meetings",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_MinutesOfMeetings_ProjectId",
                table: "MinutesOfMeetings",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_MinutesOfMeetings_TaskId",
                table: "MinutesOfMeetings",
                column: "TaskId");

            migrationBuilder.CreateIndex(
                name: "IX_Meetings_CompanyId",
                table: "Meetings",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Meetings_ProjectId",
                table: "Meetings",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Meetings_TaskId",
                table: "Meetings",
                column: "TaskId");

            migrationBuilder.CreateIndex(
                name: "IX_Meetings_WorkSpaceId",
                table: "Meetings",
                column: "WorkSpaceId");

            migrationBuilder.AddForeignKey(
                name: "FK_MeetingAgendaTopics_Meetings_MeetingId",
                table: "MeetingAgendaTopics",
                column: "MeetingId",
                principalTable: "Meetings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MeetingMembers_Meetings_MeetingId",
                table: "MeetingMembers",
                column: "MeetingId",
                principalTable: "Meetings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Meetings_Company_CompanyId",
                table: "Meetings",
                column: "CompanyId",
                principalTable: "Company",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Meetings_Project_ProjectId",
                table: "Meetings",
                column: "ProjectId",
                principalTable: "Project",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Meetings_Task_TaskId",
                table: "Meetings",
                column: "TaskId",
                principalTable: "Task",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Meetings_Workspace_WorkSpaceId",
                table: "Meetings",
                column: "WorkSpaceId",
                principalTable: "Workspace",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MinutesOfMeetings_Meetings_MeetingId",
                table: "MinutesOfMeetings",
                column: "MeetingId",
                principalTable: "Meetings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MinutesOfMeetings_Project_ProjectId",
                table: "MinutesOfMeetings",
                column: "ProjectId",
                principalTable: "Project",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MinutesOfMeetings_Task_TaskId",
                table: "MinutesOfMeetings",
                column: "TaskId",
                principalTable: "Task",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MeetingAgendaTopics_Meetings_MeetingId",
                table: "MeetingAgendaTopics");

            migrationBuilder.DropForeignKey(
                name: "FK_MeetingMembers_Meetings_MeetingId",
                table: "MeetingMembers");

            migrationBuilder.DropForeignKey(
                name: "FK_Meetings_Company_CompanyId",
                table: "Meetings");

            migrationBuilder.DropForeignKey(
                name: "FK_Meetings_Project_ProjectId",
                table: "Meetings");

            migrationBuilder.DropForeignKey(
                name: "FK_Meetings_Task_TaskId",
                table: "Meetings");

            migrationBuilder.DropForeignKey(
                name: "FK_Meetings_Workspace_WorkSpaceId",
                table: "Meetings");

            migrationBuilder.DropForeignKey(
                name: "FK_MinutesOfMeetings_Meetings_MeetingId",
                table: "MinutesOfMeetings");

            migrationBuilder.DropForeignKey(
                name: "FK_MinutesOfMeetings_Project_ProjectId",
                table: "MinutesOfMeetings");

            migrationBuilder.DropForeignKey(
                name: "FK_MinutesOfMeetings_Task_TaskId",
                table: "MinutesOfMeetings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MinutesOfMeetings",
                table: "MinutesOfMeetings");

            migrationBuilder.DropIndex(
                name: "IX_MinutesOfMeetings_ProjectId",
                table: "MinutesOfMeetings");

            migrationBuilder.DropIndex(
                name: "IX_MinutesOfMeetings_TaskId",
                table: "MinutesOfMeetings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Meetings",
                table: "Meetings");

            migrationBuilder.DropIndex(
                name: "IX_Meetings_CompanyId",
                table: "Meetings");

            migrationBuilder.DropIndex(
                name: "IX_Meetings_ProjectId",
                table: "Meetings");

            migrationBuilder.DropIndex(
                name: "IX_Meetings_TaskId",
                table: "Meetings");

            migrationBuilder.DropIndex(
                name: "IX_Meetings_WorkSpaceId",
                table: "Meetings");

            migrationBuilder.RenameTable(
                name: "MinutesOfMeetings",
                newName: "MinutesOfMeeting");

            migrationBuilder.RenameTable(
                name: "Meetings",
                newName: "Meeting");

            migrationBuilder.RenameIndex(
                name: "IX_MinutesOfMeetings_MeetingId",
                table: "MinutesOfMeeting",
                newName: "IX_MinutesOfMeeting_MeetingId");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreationDate",
                table: "MeetingMembers",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime");

            migrationBuilder.AlterColumn<string>(
                name: "Topic",
                table: "MeetingAgendaTopics",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreationDate",
                table: "MeetingAgendaTopics",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "MinutesOfMeeting",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "MinutesOfMeeting",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreationDate",
                table: "MinutesOfMeeting",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Meeting",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date",
                table: "Meeting",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreationDate",
                table: "Meeting",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MinutesOfMeeting",
                table: "MinutesOfMeeting",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Meeting",
                table: "Meeting",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MeetingAgendaTopics_Meeting_MeetingId",
                table: "MeetingAgendaTopics",
                column: "MeetingId",
                principalTable: "Meeting",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MeetingMembers_Meeting_MeetingId",
                table: "MeetingMembers",
                column: "MeetingId",
                principalTable: "Meeting",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MinutesOfMeeting_Meeting_MeetingId",
                table: "MinutesOfMeeting",
                column: "MeetingId",
                principalTable: "Meeting",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
