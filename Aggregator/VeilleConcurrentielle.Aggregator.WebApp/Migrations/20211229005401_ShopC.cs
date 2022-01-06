using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VeilleConcurrentielle.Aggregator.WebApp.Migrations
{
    public partial class ShopC : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Competitors",
                keyColumn: "Id",
                keyValue: "ShopB",
                column: "Name",
                value: "Shop B");

            migrationBuilder.InsertData(
                table: "Competitors",
                columns: new[] { "Id", "LogoUrl", "Name" },
                values: new object[] { "ShopC", "https://www.icone-png.com/png/33/32570.png", "Shop C" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Competitors",
                keyColumn: "Id",
                keyValue: "ShopC");

            migrationBuilder.UpdateData(
                table: "Competitors",
                keyColumn: "Id",
                keyValue: "ShopB",
                column: "Name",
                value: "Shop A");
        }
    }
}
