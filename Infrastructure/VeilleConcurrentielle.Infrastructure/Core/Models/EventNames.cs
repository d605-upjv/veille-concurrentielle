using System.Runtime.Serialization;

namespace VeilleConcurrentielle.Infrastructure.Core.Models
{
    public enum EventNames
    {
        [EnumMember(Value = "Test")]
        Test,
        [EnumMember(Value = "NewPriceSubmitted")]
        NewPriceSubmitted,
        [EnumMember(Value = "AddOrUpdateProductRequested")]
        AddOrUpdateProductRequested,
    }
}
