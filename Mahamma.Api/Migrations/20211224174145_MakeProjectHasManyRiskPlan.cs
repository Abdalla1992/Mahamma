using Microsoft.EntityFrameworkCore.Migrations;

namespace Mahamma.Api.Migrations
{
    public partial class MakeProjectHasManyRiskPlan : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ProjectRiskPlan_ProjectId",
                table: "ProjectRiskPlan");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectRiskPlan_ProjectId",
                table: "ProjectRiskPlan",
                column: "ProjectId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ProjectRiskPlan_ProjectId",
                table: "ProjectRiskPlan");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectRiskPlan_ProjectId",
                table: "ProjectRiskPlan",
                column: "ProjectId",
                unique: true);
        }
    }
}
