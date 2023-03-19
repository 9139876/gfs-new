using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GFS.QuotesService.DAL.Migrations
{
    public partial class AddTinkoffFirstCandleDate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "First1DayCandleDate",
                table: "AssetInfos",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "First1MinCandleDate",
                table: "AssetInfos",
                type: "timestamp with time zone",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "First1DayCandleDate",
                table: "AssetInfos");

            migrationBuilder.DropColumn(
                name: "First1MinCandleDate",
                table: "AssetInfos");
        }
    }
}
