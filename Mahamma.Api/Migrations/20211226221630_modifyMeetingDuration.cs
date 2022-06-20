using Microsoft.EntityFrameworkCore.Migrations;

namespace Mahamma.Api.Migrations
{
    public partial class modifyMeetingDuration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DurationInMinutes",
                table: "Meetings",
                newName: "DurationUnitType");

            migrationBuilder.AddColumn<int>(
                name: "Duration",
                table: "Meetings",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Duration",
                table: "Meetings");

            migrationBuilder.RenameColumn(
                name: "DurationUnitType",
                table: "Meetings",
                newName: "DurationInMinutes");
        }
    }
}
