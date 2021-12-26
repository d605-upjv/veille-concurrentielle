using VeilleConcurrentielle.Infrastructure.Core.Models;
using VeilleConcurrentielle.Infrastructure.Core.Models.Events;
using VeilleConcurrentielle.Infrastructure.Framework;
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
        public ProductPriceService(ICompetitorPriceRepository competitorPriceRepository, IEventSenderService eventSenderService, IProductRepository productRepository, ILogger<ProductPriceService> logger)
        {
            _competitorPriceRepository = competitorPriceRepository;
            _eventSenderService = eventSenderService;
            _productRepository = productRepository;
            _logger = logger;
        }
        public async Task OnPriceIdentifedAsync(string refererEventId, PriceIdentifiedEventPayload request)
        {
            var productEntity = await _productRepository.GetByIdAsync(request.ProductId);
            if (productEntity == null)
            {
                _logger.LogError($"Failed to load product {request.ProductId} to process identified price\nRequest: {SerializationUtils.Serialize(request)}");
                return;
            }
            var isDifferntFromLastPrice = await _competitorPriceRepository.IsDifferntFromLastPriceAsync(request.ProductId, request.CompetitorId.ToString(), request.Price, request.Quantity);
            if (!isDifferntFromLastPrice)
            {
                _logger.LogInformation($"Price is not different form the last one\nRequest: {SerializationUtils.Serialize(request)}");
                return;
            }
            CompetitorPriceEntity entity = new CompetitorPriceEntity();
            entity.ProductId = request.ProductId;
            entity.Price = request.Price;
            entity.Quantity = request.Quantity;
            entity.CompetitorId = request.CompetitorId.ToString();
            entity.Source = request.Source.ToString();
            await _competitorPriceRepository.InsertAsync(entity);
            var minPrice = await GetMinPriceAsync(request.ProductId);
            var maxPrice = await GetMaxPriceAsync(request.ProductId);
            await _eventSenderService.SendProductAddedOrUpdatedEvent(refererEventId, productEntity, minPrice, maxPrice);
        }

        public async Task<ProductPrice?> GetMinPriceAsync(string productId)
        {
            var entity = await _competitorPriceRepository.GetMinPriceAsync(productId);
            if (entity != null)
            {
                return new ProductPrice()
                {
                    CompetitorId = EnumUtils.GetValueFromString<CompetitorIds>(entity.CompetitorId),
                    Price = entity.Price,
                    Quantity = entity.Quantity
                };
            }
            return null;
        }

        public async Task<ProductPrice?> GetMaxPriceAsync(string productId)
        {
            var entity = await _competitorPriceRepository.GetMaxPriceAsync(productId);
            if (entity != null)
            {
                return new ProductPrice()
                {
                    CompetitorId = EnumUtils.GetValueFromString<CompetitorIds>(entity.CompetitorId),
                    Price = entity.Price,
                    Quantity = entity.Quantity
                };
            }
            return null;
        }
    }
}
