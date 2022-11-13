using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GFS.QuotesService.DAL.Migrations
{
    public partial class QuoteEntityUniqueIndex : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Quotes_QuotesProviderAssetId_TimeFrame_Date",
                table: "Quotes",
                columns: new[] { "QuotesProviderAssetId", "TimeFrame", "Date" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Quotes_QuotesProviderAssetId_TimeFrame_Date",
                table: "Quotes");
        }
    }
}
