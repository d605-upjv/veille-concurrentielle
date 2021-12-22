using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VeilleConcurrentielle.Aggregator.WebApp.Migrations
{
    public partial class EventDispatcher : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SubmittedAt",
                table: "ReceivedEvents",
                newName: "DispatchedAt");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DispatchedAt",
                table: "ReceivedEvents",
                newName: "SubmittedAt");
        }
    }
}
