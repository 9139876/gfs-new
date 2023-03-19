using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GFS.QuotesService.DAL.Migrations
{
    public partial class ChangeUniqueTickerToUniqueFigi : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Assets_Ticker",
                table: "Assets");

            migrationBuilder.CreateIndex(
                name: "IX_Assets_FIGI",
                table: "Assets",
                column: "FIGI",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Assets_FIGI",
                table: "Assets");

            migrationBuilder.CreateIndex(
                name: "IX_Assets_Ticker",
                table: "Assets",
                column: "Ticker",
                unique: true);
        }
    }
}
