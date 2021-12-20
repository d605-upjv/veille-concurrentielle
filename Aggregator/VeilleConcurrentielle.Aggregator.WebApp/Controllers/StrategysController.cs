using Microsoft.AspNetCore.Mvc;
using VeilleConcurrentielle.Aggregator.WebApp.Data.Repositories;
using VeilleConcurrentielle.Aggregator.WebApp.Models;
using VeilleConcurrentielle.Infrastructure.Web;

namespace VeilleConcurrentielle.Aggregator.WebApp.Controllers
{
    public class StrategysController : ApiControllerBase
    {
        private readonly IStrategyRepository _strategyyRepository;
        private readonly ILogger<StrategysController> _logger;

        public StrategysController(IStrategyRepository strategyRepository, ILogger<StrategysController> logger)
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
                                .Select(c => new StrategyDto(c.Id, c.Name))
                                .ToList();
                return Ok(items);
            }catch(Exception ex)
            {
                _logger.LogError(ex, $"Failed to get strategies");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
