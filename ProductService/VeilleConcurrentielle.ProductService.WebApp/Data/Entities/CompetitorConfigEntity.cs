using Microsoft.EntityFrameworkCore;
using VeilleConcurrentielle.Infrastructure.Data;

namespace VeilleConcurrentielle.ProductService.WebApp.Data.Entities
{
    [Index(nameof(ProductId))]
    public class CompetitorConfigEntity : EntityBase
    {
        public string CompetitorId { get; set; }
        public string ProductId { get; set; }
        public string SerializedHolder { get; set; }

        public ProductEntity Product { get; set; }
    }
}
