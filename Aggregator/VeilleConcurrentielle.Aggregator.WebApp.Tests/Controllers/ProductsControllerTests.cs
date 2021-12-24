extern alias mywebapp;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using mywebapp::VeilleConcurrentielle.Aggregator.WebApp.Controllers;
using mywebapp::VeilleConcurrentielle.Aggregator.WebApp.Models;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using VeilleConcurrentielle.Aggregator.WebApp.Tests.Controllers.TestData;
using VeilleConcurrentielle.EventOrchestrator.Lib.Clients.Models;
using VeilleConcurrentielle.EventOrchestrator.Lib.Clients.ServiceClients;
using VeilleConcurrentielle.Infrastructure.Core.Models.Events;
using VeilleConcurrentielle.Infrastructure.Framework;
using Xunit;
using Xunit.Abstractions;

namespace VeilleConcurrentielle.Aggregator.WebApp.Tests.Controllers
{
    public class ProductsControllerTests
    {
        private readonly ITestOutputHelper _output;
        private readonly Mock<ILogger<ProductsController>> _loggerMocker;
        public ProductsControllerTests(ITestOutputHelper output)
        {
            _output = output;
            _loggerMocker = new Mock<ILogger<ProductsController>>();
        }

        [Fact]
        public async Task PostAddOrEditProduct_Integration()
        {
            await using var application = new AggregatorWebApp();
            using var client = application.CreateClient();
            var request = PostAddOrEditProductValidRequests.GetStandardValidRequest();
            var requestStr = SerializationUtils.Serialize(request);
            _output.WriteLine($"Submit PostAddOrUpdateProduct with: {requestStr}");
            var response = await client.PostAsJsonAsync("api/Products", request);
            response.EnsureSuccessStatusCode();
            var responseContent = await HttpClientUtils.ReadBody<AddOrEditProductModels.AddOrEditProductResponse>(response);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(responseContent);
            Assert.NotNull(responseContent.EventId);
        }

        [Theory]
        [ClassData(typeof(PostAddOrEditProductInvalidRequests))]
        public async Task PostAddOrEditProduct_Integration_WithMissingtMandatoryFields_ReturnsBadRequest(AddOrEditProductModels.AddOrEditProductRequest request)
        {
            await using var application = new AggregatorWebApp();
            using var client = application.CreateClient();
            var response = await client.PostAsJsonAsync("api/Products", request);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task PostAddOrEditProduct_CallsAppropriateServices()
        {
            var eventServiceClientMock = new Mock<IEventServiceClient>();
            eventServiceClientMock.Setup(s => s.PushEventAsync(It.IsAny<PushEventClientRequest<AddOrUpdateProductRequestedEvent, AddOrUPdateProductRequestedEventPayload>>()))
                                            .Returns((PushEventClientRequest<AddOrUpdateProductRequestedEvent, AddOrUPdateProductRequestedEventPayload> request) => {
                                                return Task.FromResult(new PushEventClientResponse<AddOrUpdateProductRequestedEvent, AddOrUPdateProductRequestedEventPayload>() { Event = new AddOrUpdateProductRequestedEvent() { Id = $"{request.Name}EventUniqueId" } });
                                            });
            ProductsController controller = new ProductsController(eventServiceClientMock.Object, _loggerMocker.Object);
            var request = PostAddOrEditProductValidRequests.GetStandardValidRequest();
            var response = await controller.PostAddOrEditProduct(request);
            eventServiceClientMock.Verify(s => s.PushEventAsync(It.IsAny<PushEventClientRequest<AddOrUpdateProductRequestedEvent, AddOrUPdateProductRequestedEventPayload>>()), Times.Once());
            Assert.IsType<OkObjectResult>(response);
        }
    }
}
