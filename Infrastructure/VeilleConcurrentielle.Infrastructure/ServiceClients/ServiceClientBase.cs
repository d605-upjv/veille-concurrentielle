using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net.Http.Json;
using System.Text;
using VeilleConcurrentielle.Infrastructure.Core.Configurations;
using VeilleConcurrentielle.Infrastructure.Core.Models;
using VeilleConcurrentielle.Infrastructure.Framework;

namespace VeilleConcurrentielle.Infrastructure.ServiceClients
{
    public abstract class ServiceClientBase
    {
        private readonly HttpClient _httpClient;
        private readonly ServiceUrlsOptions _serviceUrlOptions;
        protected readonly Dictionary<ApplicationNames, string> _serviceUrls;
        protected readonly ILogger _logger;
        protected abstract string Controller { get; }
        public ServiceClientBase(HttpClient httpClient, IOptions<ServiceUrlsOptions> serviceUrlOptions, ILogger logger)
        {
            _httpClient = httpClient;
            _serviceUrlOptions = serviceUrlOptions.Value;
            _serviceUrls = new Dictionary<ApplicationNames, string>
            {
                {ApplicationNames.EventOrchestrator, _serviceUrlOptions.EventUrl },
                {ApplicationNames.Aggregator, _serviceUrlOptions.AggregatorUrl },
                {ApplicationNames.ProductService, _serviceUrlOptions.ProductUrl }
            };
            _logger = logger;
        }

        protected string GetServiceUrl(ApplicationNames applicationName)
        {
            return _serviceUrls[applicationName];
        }

        private string ComputeServiceUrl(string serviceUrl, string path)
        {
            StringBuilder urlBuilder = new StringBuilder();
            var baseUrl = serviceUrl;
            if (baseUrl.EndsWith('/'))
            {
                baseUrl = serviceUrl.Substring(0, serviceUrl.Length - 1);
            }
            urlBuilder.Append($"{baseUrl}/api/{Controller}");
            if (!string.IsNullOrWhiteSpace(path))
            {
                urlBuilder.Append($"/{path}");
            }
            return urlBuilder.ToString();
        }

        public async Task<TResponse?> PostAsync<TRequest, TResponse>(string serviceUrl, TRequest request, string path = null) where TRequest : class where TResponse : class
        {
            try
            {
                var url = ComputeServiceUrl(serviceUrl, path);
                var response = await _httpClient.PostAsJsonAsync(url, request);
                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return null;
                }
                response.EnsureSuccessStatusCode();
                var responseContent = await HttpClientUtils.ReadBody<TResponse>(response);
                return responseContent;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to call service: {serviceUrl}/{path}  (request: {SerializationUtils.Serialize(request)})");
                throw;
            }
        }

        public async Task<TResponse?> GetAsync<TResponse>(string serviceUrl, string path = null) where TResponse : class
        {
            try
            {
                var url = ComputeServiceUrl(serviceUrl, path);
                var response = await _httpClient.GetAsync(url);
                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return null;
                }
                response.EnsureSuccessStatusCode();
                var responseContent = await HttpClientUtils.ReadBody<TResponse>(response);
                return responseContent;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to call service: {serviceUrl}/{path}");
                throw;
            }
        }
    }
}
