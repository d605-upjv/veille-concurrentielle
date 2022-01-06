using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VeilleConcurrentielle.EventOrchestrator.WebApp.Migrations
{
    public partial class TestventConsumers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "EventSubscribers",
                columns: new[] { "Id", "ApplicationName", "EventName" },
                values: new object[] { "2e778f3b-a580-43e2-b292-774d5a070d92", "ProductService", "Test" });

            migrationBuilder.InsertData(
                table: "EventSubscribers",
                columns: new[] { "Id", "ApplicationName", "EventName" },
                values: new object[] { "7b68fa7b-0062-478e-ac70-111733f6cdee", "EventOrchestrator", "Test" });

            migrationBuilder.InsertData(
                table: "EventSubscribers",
                columns: new[] { "Id", "ApplicationName", "EventName" },
                values: new object[] { "a7d97a94-7d70-4415-bbbf-2956d5879b2b", "Aggregator", "Test" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "EventSubscribers",
                keyColumn: "Id",
                keyValue: "2e778f3b-a580-43e2-b292-774d5a070d92");

            migrationBuilder.DeleteData(
                table: "EventSubscribers",
                keyColumn: "Id",
                keyValue: "7b68fa7b-0062-478e-ac70-111733f6cdee");

            migrationBuilder.DeleteData(
                table: "EventSubscribers",
                keyColumn: "Id",
                keyValue: "a7d97a94-7d70-4415-bbbf-2956d5879b2b");
        }
    }
}
