using System.Runtime.Serialization;

namespace VeilleConcurrentielle.Infrastructure.Core.Models
{
    public enum EventNames
    {
        [EnumMember(Value = "NewPriceSubmitted")]
        NewPriceSubmitted,
        [EnumMember(Value = "AddOrUpdateProductRequested")]
        AddOrUpdateProductRequested,
    }
}
