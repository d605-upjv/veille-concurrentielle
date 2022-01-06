using Microsoft.AspNetCore.Mvc;
using VeilleConcurrentielle.Aggregator.WebApp.Data.Repositories;
using VeilleConcurrentielle.Aggregator.WebApp.Models;
using VeilleConcurrentielle.Infrastructure.Web;

namespace VeilleConcurrentielle.Aggregator.WebApp.Controllers
{
    public class StrategiesController : ApiControllerBase
    {
        private readonly IStrategyRepository _strategyyRepository;
        private readonly ILogger<StrategiesController> _logger;

        public StrategiesController(IStrategyRepository strategyRepository, ILogger<StrategiesController> logger)
        {
            _strategyyRepository = strategyRepository;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var items = (await _strategyyRepository.GetAllAsync())
                                .Select(c => new GetAllStrategiessModels.GetAllStrategiesResponse.Strategy(c.Id, c.Name))
                                .ToList();
                return Ok(new GetAllStrategiessModels.GetAllStrategiesResponse(items));
            }catch(Exception ex)
            {
                _logger.LogError(ex, $"Failed to get strategies");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
