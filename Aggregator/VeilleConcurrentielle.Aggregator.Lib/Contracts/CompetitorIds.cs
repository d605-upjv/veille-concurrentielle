using System.Runtime.Serialization;

namespace VeilleConcurrentielle.Aggregator.Lib.Contracts
{
    public enum CompetitorIds
    {
        [EnumMember(Value = "ShopA")]
        ShopA,
        [EnumMember(Value = "ShopB")]
        ShopB
    }
}
