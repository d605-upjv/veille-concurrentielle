using Microsoft.AspNetCore.Mvc;
using VeilleConcurrentielle.Infrastructure.Web;
using VeilleConcurrentielle.ProductService.Lib.Servers.Models;
using VeilleConcurrentielle.ProductService.WebApp.Core.Services;

namespace VeilleConcurrentielle.ProductService.WebApp.Controllers
{
    public class ProductsController : ApiControllerBase
    {
        private readonly IProductsService _productsService;
        private readonly ILogger<ProductsController> _logger;
        public ProductsController(IProductsService productsService, ILogger<ProductsController> logger)
        {
            _productsService = productsService;
            _logger = logger;
        }

        [HttpGet("scrap")]
        public async Task<IActionResult> GetProductsToScrapAsync()
        {
            try
            {
                var items = await _productsService.GetProductsToScrap();
                return Ok(new GetProductsToScrapServerResponse() { Products = items });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to post AddOrEditProduct event");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
