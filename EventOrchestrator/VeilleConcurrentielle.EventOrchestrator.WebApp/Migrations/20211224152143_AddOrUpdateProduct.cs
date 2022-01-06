using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VeilleConcurrentielle.EventOrchestrator.WebApp.Migrations
{
    public partial class AddOrUpdateProduct : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EventId",
                table: "ReceivedEvents",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "EventSubscribers",
                columns: new[] { "Id", "ApplicationName", "EventName" },
                values: new object[] { "92e062c7-7372-43e2-957d-47c51e91bc16", "Aggregator", "ProductAddedOrUpdated" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "EventSubscribers",
                keyColumn: "Id",
                keyValue: "92e062c7-7372-43e2-957d-47c51e91bc16");

            migrationBuilder.DropColumn(
                name: "EventId",
                table: "ReceivedEvents");
        }
    }
}
