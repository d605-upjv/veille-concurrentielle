using Microsoft.Extensions.Options;
using VeilleConcurrentielle.Infrastructure.Core.Models;
using VeilleConcurrentielle.Infrastructure.Core.Models.Events;
using VeilleConcurrentielle.Infrastructure.Framework;
using VeilleConcurrentielle.ProductService.WebApp.Core.Configurations;
using VeilleConcurrentielle.ProductService.WebApp.Core.Models;
using VeilleConcurrentielle.ProductService.WebApp.Data.Entities;
using VeilleConcurrentielle.ProductService.WebApp.Data.Repositories;

namespace VeilleConcurrentielle.ProductService.WebApp.Core.Services
{
    public class ProductPriceService : IProductPriceService
    {
        private readonly ICompetitorPriceRepository _competitorPriceRepository;
        private readonly IEventSenderService _eventSenderService;
        private readonly IProductRepository _productRepository;
        private readonly ILogger<ProductPriceService> _logger;
        private readonly ProductPriceOptions _productPriceOptions;
        private readonly IRecommendationService _recommendationService;
        public ProductPriceService(ICompetitorPriceRepository competitorPriceRepository,
            IEventSenderService eventSenderService, IProductRepository productRepository,
            ILogger<ProductPriceService> logger,
            IOptions<ProductPriceOptions> productPriceOptions,
            IRecommendationService recommendationService)
        {
            _competitorPriceRepository = competitorPriceRepository;
            _eventSenderService = eventSenderService;
            _productRepository = productRepository;
            _logger = logger;
            _productPriceOptions = productPriceOptions.Value;
            _recommendationService = recommendationService;
        }
        public async Task OnPriceIdentifedAsync(string refererEventId, PriceIdentifiedEventPayload request)
        {
            var productEntity = await _productRepository.GetByIdAsync(request.ProductId);
            if (productEntity == null)
            {
                _logger.LogError($"Failed to load product {request.ProductId} to process identified price\nRequest: {SerializationUtils.Serialize(request)}");
                return;
            }
            var isDifferntFromLastPrice = await _competitorPriceRepository.IsDifferentFromLastPriceAsync(request.ProductId, request.CompetitorId.ToString(), request.Price, request.Quantity, request.CreatedAt);
            if (!isDifferntFromLastPrice)
            {
                _logger.LogInformation($"Price is not different form the last one or is already outdated\nRequest: {SerializationUtils.Serialize(request)}");
                return;
            }
            CompetitorPriceEntity entity = new CompetitorPriceEntity();
            entity.ProductId = request.ProductId;
            entity.Price = request.Price;
            entity.Quantity = request.Quantity;
            entity.CompetitorId = request.CompetitorId.ToString();
            entity.Source = request.Source.ToString();
            entity.CreatedAt = request.CreatedAt;
            await _competitorPriceRepository.InsertAsync(entity);
            var lastCompetitorPrices = await GetLastPricesAsync(request.ProductId);
            var recommendationResponse = await _recommendationService.GetRecommendationsAsync(new GetRecommendationRequest()
            {
                ProductId = productEntity.Id,
                Price = productEntity.Price,
                Quantity = productEntity.Quantity,
                Strategies = productEntity.Strategies.Select(e => EnumUtils.GetValueFromString<StrategyIds>(e.StrategyId)).ToList(),
                LastCompetitorPrices = lastCompetitorPrices
            });
            await _eventSenderService.SendProductAddedOrUpdatedEvent(refererEventId, productEntity, lastCompetitorPrices, recommendationResponse.Recommendations);
            await _eventSenderService.SendNewRecommendationPushedEvent(refererEventId, productEntity.Id, recommendationResponse.NewRecommendations);
        }

        public async Task<CompetitorProductPrices> GetLastPricesAsync(string productId)
        {
            CompetitorProductPrices competitorProductPrices = new CompetitorProductPrices()
            {
                Prices = new List<CompetitorProductPrices.CompetitorItemProductPrices>()
            };
            CompetitorProductPrices.ProductMinMaxPrice? minPrice = null;
            CompetitorProductPrices.ProductMinMaxPrice? maxPrice = null;
            var competitorIds = Enum.GetValues<CompetitorIds>();
            foreach (var competitorId in competitorIds)
            {
                var prices = (await _competitorPriceRepository.GetLastPricesAsync(productId, competitorId.ToString(), _productPriceOptions.HistoryPriceCount))
                                                .Select(e => new ProductPrice()
                                                {
                                                    Price = e.Price,
                                                    Quantity = e.Quantity,
                                                    CreatedAt = e.CreatedAt
                                                }).ToList();
                competitorProductPrices.Prices.Add(new CompetitorProductPrices.CompetitorItemProductPrices()
                {
                    CompetitorId = competitorId,
                    Prices = prices
                });
                var lastPrice = prices.FirstOrDefault();
                if (lastPrice != null)
                {
                    var lastMinMaxPrice = new CompetitorProductPrices.ProductMinMaxPrice()
                    {
                        CompetitorId = competitorId,
                        Price = lastPrice.Price,
                        Quantity = lastPrice.Quantity,
                        CreatedAt = lastPrice.CreatedAt
                    };
                    maxPrice = maxPrice == null ? lastMinMaxPrice :
                                        (lastMinMaxPrice.Price > maxPrice.Price) ? lastMinMaxPrice : maxPrice;
                    minPrice = minPrice == null ? lastMinMaxPrice :
                                        (lastMinMaxPrice.Price < minPrice.Price) ? lastMinMaxPrice : minPrice;
                }
            }
            competitorProductPrices.MinPrice = minPrice;
            competitorProductPrices.MaxPrice = maxPrice;
            return competitorProductPrices;
        }
    }
}
