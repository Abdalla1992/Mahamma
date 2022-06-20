using Microsoft.EntityFrameworkCore.Migrations;

namespace Mahamma.Api.Migrations
{
    public partial class addMinuteOfMeetingType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MinuteOfMeetingLevel",
                table: "MinutesOfMeetings",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MinuteOfMeetingLevel",
                table: "MinutesOfMeetings");
        }
    }
}
