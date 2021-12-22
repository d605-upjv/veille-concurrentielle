using Microsoft.Extensions.Options;
using System.Configuration;
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
        protected abstract string Controller { get; }
        public ServiceClientBase(HttpClient httpClient, IOptions<ServiceUrlsOptions> serviceUrlOptions)
        {
            _httpClient = httpClient;
            _serviceUrlOptions = serviceUrlOptions.Value;
            _serviceUrls = new Dictionary<ApplicationNames, string>
            {
                {ApplicationNames.EventOrchestrator, _serviceUrlOptions.EventUrl },
                {ApplicationNames.Aggregator, _serviceUrlOptions.AggregatorUrl },
                {ApplicationNames.ProductService, _serviceUrlOptions.ProductUrl }
            };
        }

        protected string GetServiceUrl(ApplicationNames applicationName)
        {
            return _serviceUrls[applicationName];
        }

        public async Task<TResponse> PostAsync<TRequest, TResponse>(string serviceUrl, TRequest request, string path = null) where TRequest : class where TResponse : class
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
            var response = await _httpClient.PostAsJsonAsync(urlBuilder.ToString(), request);
            response.EnsureSuccessStatusCode();
            var responseContent = await HttpClientUtils.ReadBody<TResponse>(response);
            return responseContent;
        }
    }
}
