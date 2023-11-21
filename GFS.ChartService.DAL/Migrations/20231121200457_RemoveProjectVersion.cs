using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GFS.ChartService.DAL.Migrations
{
    public partial class RemoveProjectVersion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProjectVersion",
                table: "ProjectInfos");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProjectVersion",
                table: "ProjectInfos",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
