using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GFS.QuotesService.DAL.Migrations
{
    public partial class Repair_AssetProviderCompatibilityEntity_UniqueKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AssetProviderCompatibilities_AssetId_QuotesProviderType",
                table: "AssetProviderCompatibilities");

            migrationBuilder.CreateIndex(
                name: "IX_AssetProviderCompatibilities_AssetId_QuotesProviderType_Tim~",
                table: "AssetProviderCompatibilities",
                columns: new[] { "AssetId", "QuotesProviderType", "TimeFrame" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AssetProviderCompatibilities_AssetId_QuotesProviderType_Tim~",
                table: "AssetProviderCompatibilities");

            migrationBuilder.CreateIndex(
                name: "IX_AssetProviderCompatibilities_AssetId_QuotesProviderType",
                table: "AssetProviderCompatibilities",
                columns: new[] { "AssetId", "QuotesProviderType" },
                unique: true);
        }
    }
}
