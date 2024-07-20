using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GFS.QuotesService.DAL.Migrations
{
    public partial class AssetEntity_Add_FinamEmitentIdentifier : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FinamEmitentIdentifier",
                table: "Assets",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FinamEmitentIdentifier",
                table: "Assets");
        }
    }
}
