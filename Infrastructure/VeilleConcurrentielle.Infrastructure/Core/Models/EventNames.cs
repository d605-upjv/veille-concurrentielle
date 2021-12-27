using System.Runtime.Serialization;

namespace VeilleConcurrentielle.Infrastructure.Core.Models
{
    public enum EventNames
    {
        [EnumMember(Value = "Test")]
        Test,
        [EnumMember(Value = "PriceIdentified")]
        PriceIdentified,
        [EnumMember(Value = "AddOrUpdateProductRequested")]
        AddOrUpdateProductRequested,
        [EnumMember(Value = "ProductAddedOrUpdated")]
        ProductAddedOrUpdated,
        [EnumMember(Value = "NewRecommendationPushed")]
        NewRecommendationPushed
    }
}
