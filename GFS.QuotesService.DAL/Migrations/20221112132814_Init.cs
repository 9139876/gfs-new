using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GFS.QuotesService.DAL.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Assets",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    MarketType = table.Column<int>(type: "integer", nullable: false),
                    AssetType = table.Column<int>(type: "integer", nullable: false),
                    Exchange = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    FIGI = table.Column<string>(type: "text", nullable: false),
                    Ticker = table.Column<string>(type: "text", nullable: false),
                    ClassCode = table.Column<string>(type: "text", nullable: false),
                    ISIN = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Assets", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AssetInfos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AssetId = table.Column<Guid>(type: "uuid", nullable: false),
                    Currency = table.Column<string>(type: "text", nullable: true),
                    MinPriceIncrement = table.Column<decimal>(type: "numeric", nullable: true),
                    Lot = table.Column<int>(type: "integer", nullable: true),
                    IpoDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Sector = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssetInfos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AssetInfos_Assets_AssetId",
                        column: x => x.AssetId,
                        principalTable: "Assets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "QuotesProviderAssets",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AssetId = table.Column<Guid>(type: "uuid", nullable: false),
                    QuotesProviderType = table.Column<int>(type: "integer", nullable: false),
                    GetQuotesRequest = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuotesProviderAssets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuotesProviderAssets_Assets_AssetId",
                        column: x => x.AssetId,
                        principalTable: "Assets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BackgroundWorkerTasks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    QuotesProviderAssetId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BackgroundWorkerTasks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BackgroundWorkerTasks_QuotesProviderAssets_QuotesProviderAs~",
                        column: x => x.QuotesProviderAssetId,
                        principalTable: "QuotesProviderAssets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Quotes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    QuotesProviderAssetId = table.Column<Guid>(type: "uuid", nullable: false),
                    TimeFrame = table.Column<int>(type: "integer", nullable: false),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Open = table.Column<decimal>(type: "numeric", nullable: false),
                    Hi = table.Column<decimal>(type: "numeric", nullable: false),
                    Low = table.Column<decimal>(type: "numeric", nullable: false),
                    Close = table.Column<decimal>(type: "numeric", nullable: false),
                    Volume = table.Column<decimal>(type: "numeric", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Quotes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Quotes_QuotesProviderAssets_QuotesProviderAssetId",
                        column: x => x.QuotesProviderAssetId,
                        principalTable: "QuotesProviderAssets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AssetInfos_AssetId",
                table: "AssetInfos",
                column: "AssetId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BackgroundWorkerTasks_QuotesProviderAssetId",
                table: "BackgroundWorkerTasks",
                column: "QuotesProviderAssetId");

            migrationBuilder.CreateIndex(
                name: "IX_Quotes_QuotesProviderAssetId_TimeFrame",
                table: "Quotes",
                columns: new[] { "QuotesProviderAssetId", "TimeFrame" });

            migrationBuilder.CreateIndex(
                name: "IX_QuotesProviderAssets_AssetId",
                table: "QuotesProviderAssets",
                column: "AssetId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AssetInfos");

            migrationBuilder.DropTable(
                name: "BackgroundWorkerTasks");

            migrationBuilder.DropTable(
                name: "Quotes");

            migrationBuilder.DropTable(
                name: "QuotesProviderAssets");

            migrationBuilder.DropTable(
                name: "Assets");
        }
    }
}
