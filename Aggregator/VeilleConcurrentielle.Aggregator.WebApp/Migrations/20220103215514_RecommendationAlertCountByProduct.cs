using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VeilleConcurrentielle.Aggregator.WebApp.Migrations
{
    public partial class RecommendationAlertCountByProduct : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddForeignKey(
                name: "FK_RecommendationAlerts_ProductAggregates_ProductId",
                table: "RecommendationAlerts",
                column: "ProductId",
                principalTable: "ProductAggregates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RecommendationAlerts_ProductAggregates_ProductId",
                table: "RecommendationAlerts");
        }
    }
}
