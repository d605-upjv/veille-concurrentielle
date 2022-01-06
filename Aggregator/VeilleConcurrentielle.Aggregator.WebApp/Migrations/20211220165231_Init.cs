using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VeilleConcurrentielle.Aggregator.WebApp.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Competitors",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    LogoUrl = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Competitors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Strategies",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Strategies", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Competitors",
                columns: new[] { "Id", "LogoUrl", "Name" },
                values: new object[] { "ShopA", "https://www.icone-png.com/png/43/43296.png", "Shop A" });

            migrationBuilder.InsertData(
                table: "Competitors",
                columns: new[] { "Id", "LogoUrl", "Name" },
                values: new object[] { "ShopB", "https://www.icone-png.com/png/43/43302.png", "Shop A" });

            migrationBuilder.InsertData(
                table: "Strategies",
                columns: new[] { "Id", "Name" },
                values: new object[] { "FivePercentAboveMeanPrice", "5% plus cher que la moyenne" });

            migrationBuilder.InsertData(
                table: "Strategies",
                columns: new[] { "Id", "Name" },
                values: new object[] { "OverallAveragePrice", "Dans la moyenne" });

            migrationBuilder.InsertData(
                table: "Strategies",
                columns: new[] { "Id", "Name" },
                values: new object[] { "OverallCheaperPrice", "Le moins cher" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Competitors");

            migrationBuilder.DropTable(
                name: "Strategies");
        }
    }
}
