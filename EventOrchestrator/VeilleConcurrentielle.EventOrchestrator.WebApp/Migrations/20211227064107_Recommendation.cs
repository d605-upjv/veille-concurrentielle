using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VeilleConcurrentielle.EventOrchestrator.WebApp.Migrations
{
    public partial class Recommendation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "EventSubscribers",
                columns: new[] { "Id", "ApplicationName", "EventName" },
                values: new object[] { "45037b0a-561d-44df-9d14-47bf2f21487d", "Aggregator", "NewRecommendationPushed" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "EventSubscribers",
                keyColumn: "Id",
                keyValue: "45037b0a-561d-44df-9d14-47bf2f21487d");
        }
    }
}
