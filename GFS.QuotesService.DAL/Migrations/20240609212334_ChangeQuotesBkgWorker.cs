using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GFS.QuotesService.DAL.Migrations
{
    public partial class ChangeQuotesBkgWorker : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TimeFrame",
                table: "AssetProviderCompatibilities",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "UpdateQuotesTasks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AssetByProviderId = table.Column<Guid>(type: "uuid", nullable: false),
                    LastQuoteDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UpdateQuotesTasks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UpdateQuotesTasks_AssetProviderCompatibilities_AssetByProvi~",
                        column: x => x.AssetByProviderId,
                        principalTable: "AssetProviderCompatibilities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UpdateQuotesTasks_AssetByProviderId",
                table: "UpdateQuotesTasks",
                column: "AssetByProviderId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UpdateQuotesTasks");

            migrationBuilder.DropColumn(
                name: "TimeFrame",
                table: "AssetProviderCompatibilities");
        }
    }
}
