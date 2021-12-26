using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VeilleConcurrentielle.ProductService.WebApp.Migrations
{
    public partial class OptimizeCompetitorPriceIndex : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_CompetitorPrices_ProductId",
                table: "CompetitorPrices",
                column: "ProductId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CompetitorPrices_ProductId",
                table: "CompetitorPrices");
        }
    }
}
