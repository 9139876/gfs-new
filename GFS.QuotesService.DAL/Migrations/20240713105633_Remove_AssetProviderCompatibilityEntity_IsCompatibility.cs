using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GFS.QuotesService.DAL.Migrations
{
    public partial class Remove_AssetProviderCompatibilityEntity_IsCompatibility : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsCompatibility",
                table: "AssetProviderCompatibilities");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsCompatibility",
                table: "AssetProviderCompatibilities",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }
    }
}
