using System.ComponentModel.DataAnnotations;

namespace VeilleConcurrentielle.Aggregator.WebApp.Models
{
    public class UpdateMainShopProductPriceModels
    {
        public class UpdaetMainShopProductPriceRequest
        {
            [Required]
            public string ProductId { get; set; }
            [Required]
            public double Price { get; set; }
        }

        public class UpdateMainShopProductPriceResponse
        {
            public string ProductId { get; set; }
        }
    }
}
