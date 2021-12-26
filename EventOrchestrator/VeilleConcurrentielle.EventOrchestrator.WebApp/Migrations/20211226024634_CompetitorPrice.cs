using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VeilleConcurrentielle.EventOrchestrator.WebApp.Migrations
{
    public partial class CompetitorPrice : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "EventSubscribers",
                columns: new[] { "Id", "ApplicationName", "EventName" },
                values: new object[] { "3c0e3b84-e3e6-4203-97c3-7f9159fd3bc0", "ProductService", "PriceIdentified" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "EventSubscribers",
                keyColumn: "Id",
                keyValue: "3c0e3b84-e3e6-4203-97c3-7f9159fd3bc0");
        }
    }
}
