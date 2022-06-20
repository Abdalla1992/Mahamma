using Microsoft.EntityFrameworkCore.Migrations;

namespace Mahamma.Api.Migrations
{
    public partial class addRating : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Rating",
                table: "TaskMember",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Rating",
                table: "Task",
                type: "float",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Rating",
                table: "TaskMember");

            migrationBuilder.DropColumn(
                name: "Rating",
                table: "Task");
        }
    }
}
