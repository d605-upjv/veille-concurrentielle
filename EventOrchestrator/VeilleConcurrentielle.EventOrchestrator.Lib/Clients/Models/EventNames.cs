using System.Runtime.Serialization;

namespace VeilleConcurrentielle.EventOrchestrator.Lib.Clients.Models
{
    public enum EventNames
    {
        [EnumMember(Value = "NewPriceSubmitted")]
        NewPriceSubmitted,
        [EnumMember(Value = "AddOrUpdateProductRequested")]
        AddOrUpdateProductRequested,
    }
}
