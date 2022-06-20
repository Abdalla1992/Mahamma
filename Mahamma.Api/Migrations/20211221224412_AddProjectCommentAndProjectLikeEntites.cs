using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Mahamma.Api.Migrations
{
    public partial class AddProjectCommentAndProjectLikeEntites : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProjectComment",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProjectId = table.Column<int>(type: "int", nullable: true),
                    ProjectMemberId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: true),
                    ParentCommentId = table.Column<int>(type: "int", nullable: true),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreationDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    DeletedStatus = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectComment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectComment_Project_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Project",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProjectComment_ProjectComment_ParentCommentId",
                        column: x => x.ParentCommentId,
                        principalTable: "ProjectComment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProjectComment_ProjectMember_ProjectMemberId",
                        column: x => x.ProjectMemberId,
                        principalTable: "ProjectMember",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProjectLikeComment",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProjectMemberId = table.Column<int>(type: "int", nullable: false),
                    ProjectCommentId = table.Column<int>(type: "int", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    DeletedStatus = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectLikeComment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectLikeComment_ProjectComment_ProjectCommentId",
                        column: x => x.ProjectCommentId,
                        principalTable: "ProjectComment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProjectLikeComment_ProjectMember_ProjectMemberId",
                        column: x => x.ProjectMemberId,
                        principalTable: "ProjectMember",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProjectComment_ParentCommentId",
                table: "ProjectComment",
                column: "ParentCommentId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectComment_ProjectId",
                table: "ProjectComment",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectComment_ProjectMemberId",
                table: "ProjectComment",
                column: "ProjectMemberId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectLikeComment_ProjectCommentId",
                table: "ProjectLikeComment",
                column: "ProjectCommentId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectLikeComment_ProjectMemberId",
                table: "ProjectLikeComment",
                column: "ProjectMemberId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProjectLikeComment");

            migrationBuilder.DropTable(
                name: "ProjectComment");
        }
    }
}
