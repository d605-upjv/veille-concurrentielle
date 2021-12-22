using System.Runtime.Serialization;

namespace VeilleConcurrentielle.Infrastructure.Core.Models
{
    public enum ApplicationNames
    {
        [EnumMember(Value = "EventOrchestrator")]
        EventOrchestrator,
        [EnumMember(Value = "Aggregator")]
        Aggregator,
        [EnumMember(Value = "Aggregator")]
        ProductService,
    }
}
