using Microsoft.AspNetCore.Mvc;
using VeilleConcurrentielle.Aggregator.WebApp.Core.Services;
using VeilleConcurrentielle.Aggregator.WebApp.Models;
using VeilleConcurrentielle.Infrastructure.Web;

namespace VeilleConcurrentielle.Aggregator.WebApp.Controllers
{
    public class MainShopController : ApiControllerBase
    {
        private readonly IMainShopProductService _mainShopProductService;
        private readonly IMainShopWebService _mainShopWebService;
        private readonly ILogger<MainShopController> _logger;
        public MainShopController(IMainShopProductService mainShopProductService, ILogger<MainShopController> logger, IMainShopWebService mainShopWebService)
        {
            _mainShopProductService = mainShopProductService;
            _logger = logger;
            _mainShopWebService = mainShopWebService;
        }

        [HttpGet]
        public async Task<IActionResult> GetMainShopProducAsync([FromQuery] string productUrl)
        {
            if (string.IsNullOrWhiteSpace(productUrl))
            {
                return BadRequest("productUrl query string is mandatory");
            }
            try
            {
                var product = await _mainShopProductService.GetProductAsync(productUrl);
                if (product == null)
                {
                    return NotFound();
                }
                return Ok(new GetMainShopProductModels.GetMainShopProductResponse()
                {
                    Product = product
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to get main shop product");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost("price")]
        public async Task<IActionResult> UpdateMainShopProductPriceAsync([FromBody] UpdateMainShopProductPriceModels.UpdaetMainShopProductPriceRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var productId = await _mainShopWebService.UpdateProductPriceAsync(request.ProductId, request.Price);
                if (productId == null)
                {
                    return NotFound();
                }
                return Ok(new UpdateMainShopProductPriceModels.UpdateMainShopProductPriceResponse()
                {
                    ProductId = productId
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to update main shop product price");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
