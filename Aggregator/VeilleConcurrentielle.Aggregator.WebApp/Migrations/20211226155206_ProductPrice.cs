using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VeilleConcurrentielle.Aggregator.WebApp.Migrations
{
    public partial class ProductPrice : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProductAggregatePrices",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    ProductId = table.Column<string>(type: "TEXT", nullable: false),
                    CompetitorId = table.Column<string>(type: "TEXT", nullable: false),
                    Price = table.Column<double>(type: "REAL", nullable: false),
                    Quantity = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductAggregatePrices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductAggregatePrices_ProductAggregates_ProductId",
                        column: x => x.ProductId,
                        principalTable: "ProductAggregates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductAggregatePrices_ProductId",
                table: "ProductAggregatePrices",
                column: "ProductId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductAggregatePrices");
        }
    }
}
