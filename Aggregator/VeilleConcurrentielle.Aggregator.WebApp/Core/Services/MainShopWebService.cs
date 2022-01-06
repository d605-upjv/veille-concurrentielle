using Microsoft.Extensions.Options;
using System.Text;
using System.Web;
using System.Xml.Linq;
using VeilleConcurrentielle.Aggregator.WebApp.Core.Configurations;
using VeilleConcurrentielle.Aggregator.WebApp.Core.Models;

namespace VeilleConcurrentielle.Aggregator.WebApp.Core.Services
{
    public class MainShopWebService : IMainShopWebService
    {
        private const string productIdPlaceHolderInUrl = "{productId}";
        private readonly HttpClient _httpClient;
        private readonly MainShopOptions _mainShopOptions;
        public MainShopWebService(HttpClient httpClient, IOptions<MainShopOptions> mainShopOptions)
        {
            _httpClient = httpClient;
            _mainShopOptions = mainShopOptions.Value;
        }
        public string CleanCDataForUpdate(string content)
        {
            XDocument document = XDocument.Parse(content, LoadOptions.None);
            document.Descendants()
                                    .Where(e => e.Attribute("notFilterable")?.Value == "true")
                                    .Remove();
            return document.ToString();
        }

        public async Task<MainShopProduct?> GetProductAsync(string productId, string productUrl)
        {
            var serviceUrl = GetProductWsUrl(productId);
            var response = await _httpClient.GetAsync(serviceUrl);
            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null;
            }
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            XDocument document = XDocument.Parse(content, LoadOptions.None);
            MainShopProduct product = new MainShopProduct();
            product.ShopProductId = productId;
            product.ShopProductUrl = productUrl;
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            product.Name = document.Descendants()
                                        .First(e => e.Name == "name").Element("language").Value;
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            product.Price = double.Parse(document.Descendants().First(e => e.Name == "price").Value);
            product.Quantity = int.Parse(document.Descendants().First(e => e.Name == "quantity").Value);
            var imageApiUrl = document.Descendants().First(e => e.Name == "id_default_image").Attributes()
                                    .Where(e => e.Name.LocalName == "href")
                                    .First().Value;
            product.ImageUrl = await GetProductImageUrl(imageApiUrl);
            return product;
        }

        public async Task<string?> UpdateProductPriceAsync(string productId, double newPrice)
        {
            var serviceUrl = GetProductWsUrl(productId);
            var response = await _httpClient.GetAsync(serviceUrl);
            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null;
            }
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            XDocument document = XDocument.Parse(content, LoadOptions.None);
            var priceNode = document.Descendants().First(e => e.Name == "price");
            priceNode.Value = newPrice.ToString();
            StringBuilder contentToSubmit = new StringBuilder();
            using var stringWriter = new StringWriter(contentToSubmit);
            document.Save(stringWriter);
            stringWriter.Flush();
            var finalContentToSubmit = CleanCDataForUpdate(contentToSubmit.ToString());
            var updateResponse = await _httpClient.PutAsync(serviceUrl, new StringContent(finalContentToSubmit, Encoding.UTF8, "application/xml"));
            updateResponse.EnsureSuccessStatusCode();
            return productId;
        }

        private string GetProductWsUrl(string productId)
        {
            UriBuilder uriBuilder = new UriBuilder(_mainShopOptions.ProductUrl.Replace(productIdPlaceHolderInUrl, productId));
            var query = HttpUtility.ParseQueryString(uriBuilder.Query);
            query["ws_key"] = _mainShopOptions.WsKey;
            uriBuilder.Query = query.ToString();
            return uriBuilder.ToString();
        }

        private async Task<string?> GetProductImageUrl(string apiLink)
        {
            UriBuilder uriBuilder = new UriBuilder(apiLink);
            var query = HttpUtility.ParseQueryString(uriBuilder.Query);
            query["ws_key"] = _mainShopOptions.WsKey;
            uriBuilder.Query = query.ToString();
            var serviceUrl = uriBuilder.ToString();
            var response = await _httpClient.GetAsync(serviceUrl);
            if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                if (errorContent.Contains("This image id does not exist"))
                {
                    return null;
                }
            }
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsByteArrayAsync();
            return "data:image/jpeg;base64," + Convert.ToBase64String(content);
        }
    }
}
