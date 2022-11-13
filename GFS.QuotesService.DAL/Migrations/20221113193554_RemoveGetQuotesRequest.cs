using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GFS.QuotesService.DAL.Migrations
{
    public partial class RemoveGetQuotesRequest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GetQuotesRequest",
                table: "QuotesProviderAssets");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "GetQuotesRequest",
                table: "QuotesProviderAssets",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
