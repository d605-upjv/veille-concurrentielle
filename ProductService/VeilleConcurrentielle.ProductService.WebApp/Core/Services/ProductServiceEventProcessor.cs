using VeilleConcurrentielle.Infrastructure.Core.Models;
using VeilleConcurrentielle.Infrastructure.Core.Models.Events;
using VeilleConcurrentielle.Infrastructure.Core.Services;

namespace VeilleConcurrentielle.ProductService.WebApp.Core.Services
{
    public class ProductServiceEventProcessor : IEventProcessor
    {
        private readonly IProductsService _productService;
        private readonly IProductPriceService _productPriceService;
        public ProductServiceEventProcessor(IProductsService productService, IProductPriceService productPriceService)
        {
            _productService = productService;
            _productPriceService = productPriceService;
        }
        public async Task ProcessEventAsync(string eventId, EventNames eventName, EventPayload eventPayload)
        {
            switch (eventName)
            {
                case EventNames.AddOrUpdateProductRequested:
                    await this._productService.OnAddOrUPdateProductRequestedAsync(eventId, (AddOrUPdateProductRequestedEventPayload)eventPayload);
                    break;
                case EventNames.PriceIdentified:
                    await this._productPriceService.OnPriceIdentifedAsync(eventId, (PriceIdentifiedEventPayload)eventPayload);
                    break;
            }
        }
    }
}
