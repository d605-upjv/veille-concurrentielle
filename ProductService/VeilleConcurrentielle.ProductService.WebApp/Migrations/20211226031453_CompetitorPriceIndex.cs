using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VeilleConcurrentielle.ProductService.WebApp.Migrations
{
    public partial class CompetitorPriceIndex : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CompetitorPrices_ProductId",
                table: "CompetitorPrices");

            migrationBuilder.AddColumn<string>(
                name: "Source",
                table: "CompetitorPrices",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_CompetitorPrices_ProductId_CompetitorId",
                table: "CompetitorPrices",
                columns: new[] { "ProductId", "CompetitorId" });

            migrationBuilder.CreateIndex(
                name: "IX_CompetitorPrices_ProductId_Price",
                table: "CompetitorPrices",
                columns: new[] { "ProductId", "Price" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CompetitorPrices_ProductId_CompetitorId",
                table: "CompetitorPrices");

            migrationBuilder.DropIndex(
                name: "IX_CompetitorPrices_ProductId_Price",
                table: "CompetitorPrices");

            migrationBuilder.DropColumn(
                name: "Source",
                table: "CompetitorPrices");

            migrationBuilder.CreateIndex(
                name: "IX_CompetitorPrices_ProductId",
                table: "CompetitorPrices",
                column: "ProductId");
        }
    }
}
