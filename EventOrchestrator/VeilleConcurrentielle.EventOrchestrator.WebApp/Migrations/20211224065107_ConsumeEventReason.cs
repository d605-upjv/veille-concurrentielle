using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VeilleConcurrentielle.EventOrchestrator.WebApp.Migrations
{
    public partial class ConsumeEventReason : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Reason",
                table: "EventConsumers",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_EventConsumers_EventId",
                table: "EventConsumers",
                column: "EventId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_EventConsumers_EventId",
                table: "EventConsumers");

            migrationBuilder.DropColumn(
                name: "Reason",
                table: "EventConsumers");
        }
    }
}
