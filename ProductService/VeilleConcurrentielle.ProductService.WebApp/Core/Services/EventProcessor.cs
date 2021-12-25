using VeilleConcurrentielle.Infrastructure.Core.Models;
using VeilleConcurrentielle.Infrastructure.Core.Models.Events;

namespace VeilleConcurrentielle.ProductService.WebApp.Core.Services
{
    public class EventProcessor : IEventProcessor
    {
        private readonly IProductsService _productService;
        public EventProcessor(IProductsService productService)
        {
            _productService = productService;
        }
        public async Task ProcessEventAsync(string eventId, EventNames eventName, EventPayload eventPayload)
        {
            switch (eventName)
            {
                case EventNames.AddOrUpdateProductRequested:
                    await this._productService.StoreProductAsync(eventId, (AddOrUPdateProductRequestedEventPayload)eventPayload);
                    break;
            }
        }
    }
}
