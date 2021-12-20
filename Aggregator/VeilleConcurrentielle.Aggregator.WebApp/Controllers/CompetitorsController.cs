using Microsoft.AspNetCore.Mvc;
using VeilleConcurrentielle.Aggregator.WebApp.Data.Repositories;
using VeilleConcurrentielle.Aggregator.WebApp.Models;
using VeilleConcurrentielle.Infrastructure.Web;

namespace VeilleConcurrentielle.Aggregator.WebApp.Controllers
{
    public class CompetitorsController : ApiControllerBase
    {
        private readonly ICompetitorRepository _competitoryRepository;
        private readonly ILogger<CompetitorsController> _logger;

        public CompetitorsController(ICompetitorRepository competitorRepository, ILogger<CompetitorsController> logger)
        {
            _competitoryRepository = competitorRepository;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var items = (await _competitoryRepository.GetAllAsync())
                                .Select(c => new CompetitorDto(c.Id, c.Name, c.LogoUrl))
                                .ToList();
                return Ok(items);
            }catch(Exception ex)
            {
                _logger.LogError(ex, $"Failed to get competitors");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
