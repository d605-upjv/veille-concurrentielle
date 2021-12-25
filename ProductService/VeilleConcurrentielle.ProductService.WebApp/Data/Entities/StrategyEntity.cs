using Microsoft.EntityFrameworkCore;
using VeilleConcurrentielle.Infrastructure.Data;

namespace VeilleConcurrentielle.ProductService.WebApp.Data.Entities
{
    [Index(nameof(ProductId))]
    public class StrategyEntity : EntityBase
    {
        public string StrategyId { get; set; }
        public string ProductId { get; set; }

        public ProductEntity Product { get; set; }
    }
}
