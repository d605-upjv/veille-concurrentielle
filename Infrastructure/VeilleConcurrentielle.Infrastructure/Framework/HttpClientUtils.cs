using System.Text;
using System.Text.Json;

namespace VeilleConcurrentielle.Infrastructure.Framework
{
    public static class HttpClientUtils
    {
        public static HttpContent CreateHttpContent(object payload)
        {
            string json = JsonSerializer.Serialize(payload, new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
            return new StringContent(json, Encoding.UTF8, "application/json");
        }

        public static async Task<T> ReadBody<T>(HttpResponseMessage response)
        {
            var body = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(body, new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
        }
    }
}
