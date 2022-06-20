using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Mahamma.Api.Migrations
{
    public partial class AddTaskAceptedRejetedStatusToTaskMember : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TaskAcceptedRejectedStatus",
                table: "TaskMember",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }



        protected override void Down(MigrationBuilder migrationBuilder)
        {
          

            migrationBuilder.DropColumn(
                name: "TaskAcceptedRejectedStatus",
                table: "TaskMember");
        }
    }
}
