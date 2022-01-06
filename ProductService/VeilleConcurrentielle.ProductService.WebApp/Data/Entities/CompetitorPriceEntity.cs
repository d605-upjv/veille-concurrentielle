using Microsoft.EntityFrameworkCore;
using VeilleConcurrentielle.Infrastructure.Data;

namespace VeilleConcurrentielle.ProductService.WebApp.Data.Entities
{
    [Index(nameof(ProductId))]
    [Index(nameof(ProductId), nameof(CompetitorId))]
    [Index(nameof(ProductId), nameof(Price))]
    public class CompetitorPriceEntity : EntityBase
    {
        public string ProductId { get; set; }
        public string CompetitorId { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Source { get; set; }

        public ProductEntity Product { get; set; }
    }
}
