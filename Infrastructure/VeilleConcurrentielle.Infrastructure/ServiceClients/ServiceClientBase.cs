﻿using System.Configuration;
using System.Net.Http.Json;
using System.Text;
using VeilleConcurrentielle.Infrastructure.Framework;

namespace VeilleConcurrentielle.Infrastructure.ServiceClients
{
    public abstract class ServiceClientBase
    {
        private readonly HttpClient _httpClient;
        protected abstract string Controller { get; }
        private readonly string _baseUrl = null;
        protected string BaseUrl { get => _baseUrl; }
        public ServiceClientBase(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _baseUrl = ConfigurationManager.AppSettings[$"{Controller}.BaseUrl"];
        }

        public async Task<TResponse> PostAsync<TRequest, TResponse>(TRequest request, string url = null) where TRequest : class where TResponse : class
        {
            StringBuilder urlBuilder = new StringBuilder();
            if (!string.IsNullOrWhiteSpace(_baseUrl))
            {
                urlBuilder.Append(_baseUrl);
            }
            urlBuilder.Append($"/api/{Controller}");
            if (!string.IsNullOrWhiteSpace(url))
            {
                urlBuilder.Append($"/{url}");
            }
            var response = await _httpClient.PostAsJsonAsync(urlBuilder.ToString(), request);
            response.EnsureSuccessStatusCode();
            var responseContent = await HttpClientUtils.ReadBody<TResponse>(response);
            return responseContent;
        }
    }
}