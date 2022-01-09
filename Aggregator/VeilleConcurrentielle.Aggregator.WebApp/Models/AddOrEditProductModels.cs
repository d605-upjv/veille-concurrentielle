using VeilleConcurrentielle.Infrastructure.Core.Models.Events;

namespace VeilleConcurrentielle.Aggregator.WebApp.Models
{
    public class AddOrEditProductModels
    {
        public class AddOrEditProductRequest : AddOrUPdateProductRequestedEventPayload
        {

        }

        public class AddOrEditProductResponse
        {
            public AddOrEditProductResponse(string eventId)
            {
                EventId = eventId;
            }
            public string EventId { get; set; }
        }
    }
}
