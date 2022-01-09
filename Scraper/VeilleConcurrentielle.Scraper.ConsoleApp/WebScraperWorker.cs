using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Diagnostics;
using VeilleConcurrentielle.EventOrchestrator.Lib.Clients.ServiceClients;
using VeilleConcurrentielle.Infrastructure.Core.Models;
using VeilleConcurrentielle.Infrastructure.Core.Models.Events;
using VeilleConcurrentielle.Infrastructure.Framework;
using VeilleConcurrentielle.ProductService.Lib.Clients.ServiceClients;

namespace VeilleConcurrentielle.Scraper.ConsoleApp
{
    public class WebScraperWorker : IWebScraperWorker
    {
        private readonly WorkerConfigOptions _workerConfig;
        private readonly ILogger<WebScraperWorker> _logger;
        private readonly IProductServiceClient _productServiceClient;
        private readonly IEventServiceClient _eventServiceClient;
        private readonly IPriceSearcher _priceSearcher;
        public WebScraperWorker(IOptions<WorkerConfigOptions> workerConfigOptions,
                            ILogger<WebScraperWorker> logger,
                            IProductServiceClient productServiceClient,
                            IEventServiceClient eventServiceClient,
                            IPriceSearcher priceSearcher
                            )
        {
            _workerConfig = workerConfigOptions.Value;
            _logger = logger;
            _productServiceClient = productServiceClient;
            _eventServiceClient = eventServiceClient;
            _priceSearcher = priceSearcher;
        }

        public async Task RunAsync()
        {
            _logger.LogInformation($"Run web scraper with config: \n{SerializationUtils.Serialize(_workerConfig)}");

            while (true)
            {
                _logger.LogInformation("Start new cycle of price scrapping");
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                try
                {
                    var response = await _productServiceClient.GetProductsToScrapAsync();
                    if (response != null)
                    {
                        await Parallel.ForEachAsync(response.Products, new ParallelOptions() { MaxDegreeOfParallelism = _workerConfig.MaxParallelCount }, async (product, cancelltionToken) =>
                        {
                            var config = _workerConfig.ShopConfigs.Where(e => e.CompetitorId == product.CompetitorId).FirstOrDefault();
                            if (config != null)
                            {
                                var price = _priceSearcher.FindPrice(product.ProductProfileUrl, config.XPath);
                                if (price != null)
                                {
                                    try
                                    {
                                        var eventResponse = await _eventServiceClient.PushEventAsync<PriceIdentifiedEvent, PriceIdentifiedEventPayload>(new EventOrchestrator.Lib.Clients.Models.PushEventClientRequest<PriceIdentifiedEvent, PriceIdentifiedEventPayload>()
                                        {
                                            Name = EventNames.PriceIdentified,
                                            Source = EventSources.Scraper,
                                            Payload = new PriceIdentifiedEventPayload()
                                            {
                                                CompetitorId = config.CompetitorId,
                                                CreatedAt = DateTime.Now,
                                                Price = price.Value,
                                                ProductId = product.ProductId,
                                                Quantity = config.DefaultQuantity,
                                                Source = PriceSources.PriceScraper
                                            }
                                        });
                                        _logger.LogInformation($"Published successfully new price for {product.ProductId} from {product.ProductProfileUrl} for {product.CompetitorId}");
                                    }
                                    catch (Exception ex)
                                    {
                                        _logger.LogError(ex, $"Failed to pubish new price for product  {product.ProductId} of {product.CompetitorId} ({product.ProductProfileUrl})");
                                    }
                                }
                            }
                        });
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Failed to scrap prices");
                }
                stopwatch.Stop();
                _logger.LogInformation($"End of price scrapping after {stopwatch.Elapsed.TotalSeconds} seconds ({stopwatch.Elapsed})");

                if (!_workerConfig.InfiniteRun)
                {
                    break;
                }

                _logger.LogInformation($"Pausing for {_workerConfig.NextRoundWaitTimeInSeconds} seconds before running new cycle");
                Thread.Sleep(_workerConfig.NextRoundWaitTimeInSeconds * 1000);
            }
        }
    }
}
