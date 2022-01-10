using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VeilleConcurrentielle.Aggregator.WebApp.Migrations
{
    public partial class ProductAggregate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProductAggregates",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Price = table.Column<double>(type: "REAL", nullable: false),
                    Quantity = table.Column<int>(type: "INTEGER", nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    ImageUrl = table.Column<string>(type: "TEXT", nullable: true),
                    MinPrice = table.Column<double>(type: "REAL", nullable: true),
                    MinPriceCompetitorId = table.Column<string>(type: "TEXT", nullable: true),
                    MinPriceQuantity = table.Column<int>(type: "INTEGER", nullable: true),
                    MaxPrice = table.Column<double>(type: "REAL", nullable: true),
                    MaxPriceCompetitorId = table.Column<string>(type: "TEXT", nullable: true),
                    MaxPriceQuantit = table.Column<int>(type: "INTEGER", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductAggregates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductAggregateCompetitorConfigs",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    ProductId = table.Column<string>(type: "TEXT", nullable: false),
                    CompetitorId = table.Column<string>(type: "TEXT", nullable: false),
                    SerializedHolder = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductAggregateCompetitorConfigs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductAggregateCompetitorConfigs_ProductAggregates_ProductId",
                        column: x => x.ProductId,
                        principalTable: "ProductAggregates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductAggregateStrategies",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    ProductId = table.Column<string>(type: "TEXT", nullable: false),
                    StrategyId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductAggregateStrategies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductAggregateStrategies_ProductAggregates_ProductId",
                        column: x => x.ProductId,
                        principalTable: "ProductAggregates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductAggregateCompetitorConfigs_ProductId",
                table: "ProductAggregateCompetitorConfigs",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductAggregateStrategies_ProductId",
                table: "ProductAggregateStrategies",
                column: "ProductId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductAggregateCompetitorConfigs");

            migrationBuilder.DropTable(
                name: "ProductAggregateStrategies");

            migrationBuilder.DropTable(
                name: "ProductAggregates");
        }
    }
}
