using Microsoft.EntityFrameworkCore.Migrations;

namespace Mahamma.Api.Migrations
{
    public partial class editEntities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MeetingMember_Meeting_MeetingId",
                table: "MeetingMember");

            migrationBuilder.DropForeignKey(
                name: "FK_MinuteOfMeeting_Meeting_MeetingId",
                table: "MinuteOfMeeting");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MinuteOfMeeting",
                table: "MinuteOfMeeting");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MeetingMember",
                table: "MeetingMember");

            migrationBuilder.RenameTable(
                name: "MinuteOfMeeting",
                newName: "MinutesOfMeeting");

            migrationBuilder.RenameTable(
                name: "MeetingMember",
                newName: "MeetingMembers");

            migrationBuilder.RenameIndex(
                name: "IX_MinuteOfMeeting_MeetingId",
                table: "MinutesOfMeeting",
                newName: "IX_MinutesOfMeeting_MeetingId");

            migrationBuilder.RenameIndex(
                name: "IX_MeetingMember_MeetingId",
                table: "MeetingMembers",
                newName: "IX_MeetingMembers_MeetingId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MinutesOfMeeting",
                table: "MinutesOfMeeting",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MeetingMembers",
                table: "MeetingMembers",
                column: "Id");

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
                name: "PK_MeetingMembers",
                table: "MeetingMembers");

            migrationBuilder.RenameTable(
                name: "MinutesOfMeeting",
                newName: "MinuteOfMeeting");

            migrationBuilder.RenameTable(
                name: "MeetingMembers",
                newName: "MeetingMember");

            migrationBuilder.RenameIndex(
                name: "IX_MinutesOfMeeting_MeetingId",
                table: "MinuteOfMeeting",
                newName: "IX_MinuteOfMeeting_MeetingId");

            migrationBuilder.RenameIndex(
                name: "IX_MeetingMembers_MeetingId",
                table: "MeetingMember",
                newName: "IX_MeetingMember_MeetingId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MinuteOfMeeting",
                table: "MinuteOfMeeting",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MeetingMember",
                table: "MeetingMember",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MeetingMember_Meeting_MeetingId",
                table: "MeetingMember",
                column: "MeetingId",
                principalTable: "Meeting",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MinuteOfMeeting_Meeting_MeetingId",
                table: "MinuteOfMeeting",
                column: "MeetingId",
                principalTable: "Meeting",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
