extern alias mywebapp;

using Moq;
using mywebapp::VeilleConcurrentielle.Aggregator.WebApp.Core.Services;
using mywebapp::VeilleConcurrentielle.Aggregator.WebApp.Data.Entities;
using mywebapp::VeilleConcurrentielle.Aggregator.WebApp.Data.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;
using VeilleConcurrentielle.Infrastructure.Core.Models;
using VeilleConcurrentielle.Infrastructure.Core.Models.Events;
using Xunit;

namespace VeilleConcurrentielle.Aggregator.WebApp.Tests.Core.Services
{
    public class RecommendationAlertServiceTests
    {
        [Fact]
        public async Task StoreNewRecommendationAlertAsync_CallsRequiredServices()
        {
            Mock<IRecommendationAlertRepository> recommendationAlertRepositoryMock = new Mock<IRecommendationAlertRepository>();
            IRecommendationAlertService recommendationService = new RecommendationAlertService(recommendationAlertRepositoryMock.Object);
            await recommendationService.StoreNewRecommendationAlertAsync("eventId", new NewRecommendationPushedEventPayload()
            {
                Recommendation = new ProductRecommendation()
            });
            recommendationAlertRepositoryMock.Verify(s => s.InsertAsync(It.IsAny<RecommendationAlertEntity>()));
        }

        [Fact]
        public async Task SetToSeenAsync_ReturnsNull_IfAlertIsNotFound()
        {
            string id = "AlertId";
            Mock<IRecommendationAlertRepository> recommendationAlertRepositoryMock = new Mock<IRecommendationAlertRepository>();
            recommendationAlertRepositoryMock.Setup(s => s.GetByIdAsync(id))
                                                    .Returns(Task.FromResult<RecommendationAlertEntity?>(null));
            IRecommendationAlertService recommendationService = new RecommendationAlertService(recommendationAlertRepositoryMock.Object);
            var alert = await recommendationService.SetToSeenAsync(id);
            Assert.Null(alert);
            recommendationAlertRepositoryMock.Verify(s => s.UpdateAsync(It.IsAny<RecommendationAlertEntity>()), Times.Never());
        }

        [Fact]
        public async Task SetToSeenAsync_ReturnsRecommendationAlert_IfFound()
        {
            string id = "AlertId";
            RecommendationAlertEntity existingAlert = new RecommendationAlertEntity()
            {
                Id = id
            };
            Mock<IRecommendationAlertRepository> recommendationAlertRepositoryMock = new Mock<IRecommendationAlertRepository>();
            recommendationAlertRepositoryMock.Setup(s => s.GetByIdAsync(id))
                                                    .Returns(Task.FromResult<RecommendationAlertEntity?>(existingAlert));
            IRecommendationAlertService recommendationService = new RecommendationAlertService(recommendationAlertRepositoryMock.Object);
            var alert = await recommendationService.SetToSeenAsync(id);
            Assert.NotNull(alert);
            recommendationAlertRepositoryMock.Verify(s => s.UpdateAsync(existingAlert), Times.Once());
        }

        [Fact]
        public async Task GetAlUnseenAsync_CallsCorrespondingRepositoryMethod()
        {
            string id = "AlertId";
            Mock<IRecommendationAlertRepository> recommendationAlertRepositoryMock = new Mock<IRecommendationAlertRepository>();
            recommendationAlertRepositoryMock.Setup(s => s.GetAllUnseenAsync())
                                                    .Returns(Task.FromResult(new List<RecommendationAlertEntity>()));
            IRecommendationAlertService recommendationService = new RecommendationAlertService(recommendationAlertRepositoryMock.Object);
            var items = await recommendationService.GetAlUnseenAsync();
            Assert.NotNull(items);
            recommendationAlertRepositoryMock.Verify(s => s.GetAllUnseenAsync(), Times.Once());
        }

        [Fact]
        public async Task GetAlUnseenCountAsync_CallsCorrespondingRepositoryMethod()
        {
            int unseenCount = 5;
            Mock<IRecommendationAlertRepository> recommendationAlertRepositoryMock = new Mock<IRecommendationAlertRepository>();
            recommendationAlertRepositoryMock.Setup(s => s.GetAllUnseenCountAsync())
                                                    .Returns(Task.FromResult(unseenCount));
            IRecommendationAlertService recommendationService = new RecommendationAlertService(recommendationAlertRepositoryMock.Object);
            var response = await recommendationService.GetAllUnseenCountAsync();
            Assert.NotNull(response);
            Assert.Equal(unseenCount, response.Count);
            recommendationAlertRepositoryMock.Verify(s => s.GetAllUnseenCountAsync(), Times.Once());
        }

        [Fact]
        public async Task GetAlUnseenCountByProductAsync_CallsCorrespondingRepositoryMethod()
        {
            Mock<IRecommendationAlertRepository> recommendationAlertRepositoryMock = new Mock<IRecommendationAlertRepository>();
            recommendationAlertRepositoryMock.Setup(s => s.GetAllUnseenCountByProductAsync())
                                                    .Returns(Task.FromResult(new List<(string ProductId, string ProductName, int Count)>()));
            IRecommendationAlertService recommendationService = new RecommendationAlertService(recommendationAlertRepositoryMock.Object);
            var response = await recommendationService.GetAllUnseenCountByProductAsync();
            Assert.NotNull(response);
            Assert.NotNull(response.Items);
            Assert.Empty(response.Items);
            recommendationAlertRepositoryMock.Verify(s => s.GetAllUnseenCountByProductAsync(), Times.Once());
        }

        [Fact]
        public async Task SetToSeenForProductAsync_CallsCorrespondingRepositoryMethod()
        {
            string productId = "ProductId";
            int affectedCount = 5;
            Mock<IRecommendationAlertRepository> recommendationAlertRepositoryMock = new Mock<IRecommendationAlertRepository>();
            recommendationAlertRepositoryMock.Setup(s => s.SetRecommendationAlertsForProductToSeenAsync(It.IsAny<string>()))
                                                    .Returns(Task.FromResult(affectedCount));
            IRecommendationAlertService recommendationService = new RecommendationAlertService(recommendationAlertRepositoryMock.Object);
            var response = await recommendationService.SetToSeenForProductAsync(productId);
            Assert.NotNull(response);
            Assert.Equal(affectedCount, response.AffectedCount);
            recommendationAlertRepositoryMock.Verify(s => s.SetRecommendationAlertsForProductToSeenAsync(productId), Times.Once());
        }
    }
}
