using Microsoft.AspNetCore.Mvc;
using VeilleConcurrentielle.Infrastructure.Web;
using VeilleConcurrentielle.Aggregator.WebApp.Models;
using VeilleConcurrentielle.Aggregator.WebApp.Core.Services;

namespace VeilleConcurrentielle.Aggregator.WebApp.Controllers
{
    public class RecommendationAlertsController : ApiControllerBase
    {
        private readonly IRecommendationAlertService _recommendationAlertService;
        private readonly ILogger<RecommendationAlertsController> _logger;
        public RecommendationAlertsController(IRecommendationAlertService recommendationAlertService, ILogger<RecommendationAlertsController> logger)
        {
            _recommendationAlertService = recommendationAlertService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUnseenAsync()
        {
            try
            {
                var response = await _recommendationAlertService.GetAlUnseenAsync();
                if (response == null)
                {
                    return NotFound();
                }
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to get recommendation alerts");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost("seen/{id}")]
        public async Task<IActionResult> SetToSeenAsync(string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var response = await _recommendationAlertService.SetToSeenAsync(id);
                if (response == null)
                {
                    return NotFound();
                }
                return Ok(response);
            }catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to set recommendation alert as seen");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
